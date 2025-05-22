using Jobify.Core.Models;
using Jobify.Services.Commons.DTOs.Resoponses;
using Jobify.Services.Commons.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Services.Features.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ApiResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public RegisterCommandHandler(
            UserManager<AppUser> userManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userManager.FindByEmailAsync(request.Username);
            if (userExists != null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "User already exists"
                };
            }

            var user = new AppUser
            {
                UserName = request.Username,
                Email = request.Username,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Registration failed: {errors}"
                };
            }

            // Generate tokens after successful registration
            var accessToken = _tokenService.GenerateJwtToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);

            return new ApiResponse
            {
                Success = true,
                Message = "Registration successful",
                Data = new
                {
                    AccessToken = accessToken.Token,
                    RefreshToken = refreshToken.Token,
                    AccessTokenExpires = accessToken.Expires
                }
            };
        }
    }
}
