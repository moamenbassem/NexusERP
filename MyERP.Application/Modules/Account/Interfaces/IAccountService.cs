using MyERP.Application.Modules.Account.DTOs;
using MyERP.Application.Modules.HR.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyERP.Application.Modules.Account.Interfaces
{
    public interface IAccountService
    {
        public Task<AppUserDto> RegisterNewUserAsync(RegisterAppUserDto dto);
        public Task<EmployeeDto> RegisterNewStaffAsync(RegisterEmployeeDto dto, int UserId);
        public Task AssignNewAdminAsync(int Id, int UserId);
        public Task<UserTokenDto> LoginAsync(LoginDto dto);
        public Task<IEnumerable<AppUserDto>> GetAllUsersAsync();
        public Task<IEnumerable<EmployeeDto>> GetAllStaffAsync();
        public Task<IEnumerable<AppUserDto>> GetAllCustomersAsync();


    }
}
