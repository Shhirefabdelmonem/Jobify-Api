using MediatR;
using Jobify.Services.Commons.DTOs.requests;
using Jobify.Services.Commons.DTOs.Resoponses;
using Microsoft.AspNetCore.Identity;
using Jobify.Core.Models;
using Jobify.Services.Commons.Interfaces;

namespace Jobify.Services.Features.Auth.Commands.Login
{
    public class LoginCommand : IRequest<ApiResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}