using Jobify.Services.Commons.Data;
using Jobify.Services.Commons.DTOs.Resoponses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Services.Features.Auth.Commands.RevokeToken
{
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, ApiResponse>
    {
        private readonly IApplicationDbContext _context;

        public RevokeTokenCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

            if (token != null && token.IsActive)
            {
                token.RevokedOn = DateTime.UtcNow;
                _context.RefreshTokens.Update(token);
                await _context.SaveChangesAsync(cancellationToken);
            }

            return new ApiResponse
            {
                Success = true,
                Message = "Refresh token revoked successfully"
            };
        }
    }
}
