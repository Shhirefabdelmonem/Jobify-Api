using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobify.Services.Commons.DTOs.Resoponses;
using MediatR;

namespace Jobify.Services.Features.Profile.Commands.GetProfile
{
    public class GetProfileQuery : IRequest<ApiResponse>
    {
        // No properties needed as we get user from authentication context
    }
}
