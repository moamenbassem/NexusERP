using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyERP.Application.Interfaces;
using MyERP.Application.Modules.Account.DTOs;
using MyERP.Application.Modules.Account.Interfaces;
using MyERP.Application.Modules.Account.Mappers;
using MyERP.Application.Modules.AuditLog.Interfaces;
using MyERP.Application.Modules.HR.DTOs;
using MyERP.Domain.Entities.CRM;
using MyERP.Domain.Entities.HR;
using MyERP.Domain.Entities.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyERP.Infrastructure.Modules.Account.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IUnitOfWork myunit;
        private readonly IAuditLogService auditLogService;

        public AccountService(UserManager<AppUser> _userManager, IConfiguration _configuration,IUnitOfWork _myunit,IAuditLogService _auditLogService)
        {
            userManager = _userManager;
            configuration = _configuration;
            myunit = _myunit;
            auditLogService = _auditLogService;
        }
        public async Task<AppUserDto> RegisterNewUserAsync(RegisterAppUserDto dto)
        {
            //var hasAnyUser = await userManager.Users.AnyAsync();
            //if (!hasAnyUser)
            //{
            //    var adminEmp = dto.ToEmployee();
            //    var res = await userManager.CreateAsync(adminEmp, dto.Password);
            //    if (res.Succeeded)
            //    {
            //        await userManager.AddToRoleAsync(adminEmp, "Admin");
            //        return adminEmp.ToDto();
            //    }
            //    throw new Exception("Failed to create initial Admin Employee.");
            //}

            var customer = dto.ToCustomer();
            IdentityResult result = await userManager.CreateAsync(customer, dto.Password);
            if (result.Succeeded)
            {

                await userManager.AddToRoleAsync(customer, "User");
                var CustomerDto = customer.ToDto();
                var roles = await userManager.GetRolesAsync(customer);
                CustomerDto.Roles = string.Join(",", roles);
                return CustomerDto;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    throw new Exception($"{error.Description}" );
                }
                return null;
            }
        }
        public async Task AssignNewAdminAsync (int Id, int UserId)
        {
            var emp = await myunit.EmployeeRepo.GetByIdAsync(Id);
            if (emp == null) throw new Exception($"Employee of ID:{Id} Cannot be found ");

            await userManager.AddToRoleAsync(emp, "Admin");
            await auditLogService.LogAsync(UserId, "Assigning system Admin", "Employee", emp.Id, "Assigning system Admin");

        }
        public async Task<EmployeeDto> RegisterNewStaffAsync(RegisterEmployeeDto dto, int UserId)
        {
            var staff = dto.ToEmployee();
            IdentityResult result = await userManager.CreateAsync(staff, dto.Password);
            if (result.Succeeded)
            {

                await userManager.AddToRoleAsync(staff, "Staff");
                await auditLogService.LogAsync(UserId, "Adding Staff Member", "Employee", staff.Id, "Adding Staff Member");
                await myunit.Commit();
                var EmpDto = staff.ToDto();
                var roles = await userManager.GetRolesAsync(staff);
                EmpDto.Roles = string.Join(",", roles);
                return EmpDto;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    throw new Exception($"{error.Description}" );
                }
                return null;
            }
        }


        // Change return type to a DTO or a dynamic object containing the string token
        public async Task<UserTokenDto> LoginAsync(LoginDto dto)
        {
            var user = await userManager.FindByNameAsync(dto.UserName);

            // 1. Validation Logic
            if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password))
            {
                // Throw a custom exception or return null
                throw new Exception("Invalid Username or Password");
            }

            // 2. Claims & Token Logic (Keep your existing code here...)
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // user.Id is int, needs ToString()
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            // Security Key Setup
            // Retrieve the secret string from appsettings.json and convert it to a byte array
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            // Create signing credentials using the key and the HMAC SHA256 algorithm
            // This "seals" the token so it cannot be tampered with by the client
            var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token Construction
            // This assembles the JWT with the payload (claims), metadata (issuer/audience), and the signature
            var token = new JwtSecurityToken(
                claims: claims,
                issuer: configuration["JWT:Issuer"],      // Who issued the token (Your API)
                audience: configuration["JWT:Audience"],    // Who the token is for (Your Frontend)
                expires: DateTime.Now.AddHours(1),        // Token expiration time
                signingCredentials: sc                    // The digital signature
            );

            // 3. Return the data, not an HTTP action
            return new UserTokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }
        public async Task<IEnumerable<AppUserDto>> GetAllUsersAsync()
        {
        var Users = await myunit.AppUserRepo.GetAllAsync();

            if (Users == null || !Users.Any())
            {
                throw new Exception("There are no staff members registered.");
            }
            var UsersDto = new List<AppUserDto>();
            foreach (var User in Users)
            {
                var roles = await userManager.GetRolesAsync(User);
                var UserDto = User.ToDto();
                UserDto.Roles = string.Join(",", roles);
                UsersDto.Add(UserDto);
            }
            return UsersDto;
        }
        public async Task<EmployeeDto> GetOneStaffAsync(int id)
        {
        var employee = await myunit.EmployeeRepo.GetByIdAsync(id);

            if (employee == null)
            {
                throw new Exception("There are no staff members registered.");
            }
            var EmpDto = employee.ToDto();
            var roles = await userManager.GetRolesAsync(employee);
            EmpDto.Roles = string.Join(",", roles);
            return EmpDto;
        }
        public async Task<IEnumerable<EmployeeDto>> GetAllStaffAsync()
        {
        var employees = await myunit.EmployeeRepo.GetAllAsync();

            if (employees == null || !employees.Any())
            {
                throw new Exception("There are no staff members registered.");
            }
            var EmployeesDto = new List<EmployeeDto>();
            foreach( var employee in employees) 
            {
                var roles = await userManager.GetRolesAsync(employee);
                var EmpDto = employee.ToDto();
                EmpDto.Roles = string.Join(",", roles);
                EmployeesDto.Add(EmpDto);
            }

            return EmployeesDto;
        }
        public async Task<IEnumerable<AppUserDto>> GetAllCustomersAsync()
        {
            //var customers = await userManager.Users.OfType<Customer>().ToListAsync();
            var customers = await myunit.CustomerRepo.GetAllAsync();
            if (customers == null || !customers.Any()) throw new Exception("There're No Customers registered.");
            var customersDto = new List<AppUserDto>();
            foreach (var User in customers)
            {
                var roles = await userManager.GetRolesAsync(User);
                var UserDto = User.ToDto();
                UserDto.Roles = string.Join(",", roles);
                customersDto.Add(UserDto);
            }
            return customersDto;        
        }


    }

}
