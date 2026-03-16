using MyERP.Application.Modules.Account.DTOs;
using MyERP.Application.Modules.HR.DTOs;
using MyERP.Domain.Entities.CRM;
using MyERP.Domain.Entities.HR;
using MyERP.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Account.Mappers
{
    public static class AccountMapper
    {
        public static AppUser ToUser(this RegisterAppUserDto dto)
        {
            return new AppUser()
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName  = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                BirthDate = dto.BirthDate,
                Address = dto.Address,
                Gender = dto.Gender
            };
        }
        public static Customer ToCustomer(this RegisterAppUserDto dto)
        {
            return new Customer()
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName  = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                BirthDate = dto.BirthDate,
                Address = dto.Address,
                Gender = dto.Gender,
                LifetimeValue = 0
            };
        }
        public static Employee ToEmployee(this RegisterAppUserDto dto)
        {
            return new Employee()
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName  = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                BirthDate = dto.BirthDate,
                Address = dto.Address,
                Gender = dto.Gender
            };
        }
        public static Employee ToEmployee(this RegisterEmployeeDto dto)
        {
            return new Employee()
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName  = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                BirthDate = dto.BirthDate,
                Address = dto.Address,
                Gender = dto.Gender,
                PayRate = dto.PayRate,
                PayType = dto.PayType
            };
        }
        public static AppUserDto ToDto(this AppUser user)
        {
            return new AppUserDto
            {
                FullName = user.FullName,
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber, 
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Address = user.Address,
            };
        }
        public static EmployeeDto ToDto(this Employee user)
        {
            return new EmployeeDto
            {
                FullName = user.FullName,
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber, 
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Address = user.Address,
                PayRate = user.PayRate,
                PayType = user.PayType,
                JoinDate = user.JoinDate,
                EmployeeStatus = user.EmployeeStatus,
                
            };
        }

    }
}
