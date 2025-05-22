using Jobify.Core.Models;
using Jobify.Services.Commons.DTOs.requests;
using Jobify.Services.Commons.DTOs.Resoponses;
using Jobify.Services.Commons.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Jobify.Services.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<ApiResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    
}