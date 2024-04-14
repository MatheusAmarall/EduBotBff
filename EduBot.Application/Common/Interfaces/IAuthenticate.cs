﻿using EduBot.Application.Common.DTOs;
using EduBot.Domain.Entities;

namespace EduBot.Application.Common.Interfaces {
    public interface IAuthenticate {
        Task<UserInfoResultDto?> GetUserInfoAsync(string email);
        Task<bool> Authenticate(string email, string password);
        Task<string> RegisterUser(User user);
        Task Logout();
    }
}
