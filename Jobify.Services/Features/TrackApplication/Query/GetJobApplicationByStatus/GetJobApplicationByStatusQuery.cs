using Jobify.Services.Commons.DTOs.Resoponses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Application.Features.TrackApplication.Query.GetJobApplicationByStatus
{
    public class GetJobApplicationByStatusQuery : IRequest<ApiResponse>
    {
        public string? Status { get; set; }

    }
}
