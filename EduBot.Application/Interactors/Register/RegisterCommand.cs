﻿using MediatR;

namespace EduBot.Application.Interactors.Register {
    public class RegisterCommand : IRequest<bool> {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}