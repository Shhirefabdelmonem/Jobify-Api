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

namespace Jobify.Services.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(UserManager<AppUser> userManager, JwtSettings jwtSettings, ITokenService tokenService)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            var accessToken = _tokenService.GenerateJwtToken(user);
            var refreshToken = await _tokenService.GenerateRefreshTokenAsync(user);

            return new ApiResponse
            {
                Success = true,
                Message = "Login successful",
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
