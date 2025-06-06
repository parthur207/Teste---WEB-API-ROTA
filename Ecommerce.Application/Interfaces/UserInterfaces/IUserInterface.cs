﻿using Ecommerce.Application.DTOs;
using Ecommerce.Domain.Models;

namespace Ecommerce.Application.Interfaces.UserInterfaces
{
    public interface IUserInterface
    {
        Task<(bool, string)> AddUser(CreateUserModel model);
        //Task<(bool, string)> PutPassword(UpdateUserPasswordModel model, int UserId);

        Task<(bool, string)> LoginUser(UserLoginModel model);

        Task<(bool, string)> PutUserData(UpdateUserDataModel model, int UserId);

        Task<(bool, UserDataTokenDTO?)> GetDataUserByEmail(string email);

        Task<(bool, string)> UpdatePasswordUser(UpdateUserPasswordModel model, int UserId);
    }
}
