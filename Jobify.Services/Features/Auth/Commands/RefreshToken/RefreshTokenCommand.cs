using Jobify.Core.Models;
using Jobify.Services.Commons.Data;
using Jobify.Services.Commons.DTOs.requests;
using Jobify.Services.Commons.DTOs.Resoponses;
using Jobify.Services.Commons.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jobify.Services.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<ApiResponse>
    {
        public RefreshTokenRequestDTO Request { get; set; }
    }

   
}