using Jobify.Services.DTOs.requests;
using Jobify.Services.DTOs.Resoponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Services.IControllerServices
{
    public interface IAuthService
    {
        Task<ApiResponse> LoginAsync(LogInDTO request);
        Task<ApiResponse> RefreshTokenAsync(RefreshTokenRequestDTO request);
        Task<ApiResponse> RegisterAsync(RegisterDTO request);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
