using Jobify.Core.Models;
using Jobify.Services.Commons.Data;
using Jobify.Services.Commons.DTOs.Resoponses;
using Jobify.Services.Commons.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Services.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ApiResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService _tokenService;

        public RefreshTokenCommandHandler(
            UserManager<AppUser> userManager,
            IApplicationDbContext context,
            JwtSettings jwtSettings,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _context = context;
            _jwtSettings = jwtSettings;
            _tokenService = tokenService;
        }

        public async Task<ApiResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == request.Request.RefreshToken);

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
            var newAccessToken = _tokenService.GenerateJwtToken(user);

            // Rotate refresh token
            refreshToken.RevokedOn = DateTime.UtcNow;
            _context.RefreshTokens.Update(refreshToken);

            var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync(user);
            await _context.SaveChangesAsync(cancellationToken);

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

    }
}
