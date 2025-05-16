using Jobify.Core.Interfaces.IRepos;
using Jobify.Core.Models;
using Jobify.Services.DTOs;
using Jobify.Services.DTOs.requests;
using Jobify.Services.DTOs.Resoponses;
using Jobify.Services.IControllerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Services.ControllerServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;

        public AuthService(UserManager<AppUser> userManager, IConfiguration configuration, IUnitOfWork unitOfWork, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
        }
        public async Task<ApiResponse> LoginAsync(LogInDTO request)
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
            var accessToken = GenerateJwtToken(user);
            var refreshToken = await GenerateRefreshTokenAsync(user);
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

        public async Task<ApiResponse> RefreshTokenAsync(RefreshTokenRequestDTO request)
        {
            var refreshToken = await _unitOfWork.Token.GetByTokenAsync(request.RefreshToken);
            if (refreshToken == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "Invalid refresh token"
                };
            }

            var user = await _userManager.FindByIdAsync(refreshToken.UserId);
            if (user == null)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            // Generate new access token
            var newAccessToken = GenerateJwtToken(user);

            // Optional: Rotate refresh token
            refreshToken.RevokedOn = DateTime.UtcNow;
            await _unitOfWork.Token.UpdateAsync(refreshToken);
            var newRefreshToken = await GenerateRefreshTokenAsync(user);

            await _unitOfWork.CompleteAsync();

            return new ApiResponse
            {
                Success = true,
                Message = "Token refreshed successfully",
                Data = new
                {
                    AccessToken = newAccessToken.Token,
                    RefreshToken = newRefreshToken.Token,
                    AccessTokenExpires = newAccessToken.Expires
                }

            };


        }


        public async Task<ApiResponse> RegisterAsync(RegisterDTO request)
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
            var accessToken = GenerateJwtToken(user);
            var refreshToken = await GenerateRefreshTokenAsync(user);

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

        public async Task<ApiResponse> RevokeRefreshTokenAsync(string refreshToken)
        {
            var token = await _unitOfWork.Token.GetByTokenAsync(refreshToken);
            if (token != null && token.IsActive)
            {
                token.RevokedOn = DateTime.UtcNow;
                await _unitOfWork.Token.UpdateAsync(token);
                await _unitOfWork.CompleteAsync();
            }
            return new ApiResponse
            {
                Success = true,
                Message = "Refresh token revoked successfully"
            };

        }

        private (string Token, DateTime Expires) GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenLifetimeInMinutes);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }


        private async Task<RefreshToken> GenerateRefreshTokenAsync(IdentityUser user)
        {
            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresOn = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeInDays),
                CreatedOn = DateTime.UtcNow,


            };

            await _unitOfWork.Token.AddAsync(refreshToken);
            await _unitOfWork.CompleteAsync();

            return refreshToken;
        }
    }
}
