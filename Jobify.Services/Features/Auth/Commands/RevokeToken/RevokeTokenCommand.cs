using Jobify.Services.Commons.Data;
using Jobify.Services.Commons.DTOs.Resoponses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Services.Features.Auth.Commands.RevokeToken
{
    public class RevokeTokenCommand : IRequest<ApiResponse>
    {
        public string RefreshToken { get; set; }
    }

    
}