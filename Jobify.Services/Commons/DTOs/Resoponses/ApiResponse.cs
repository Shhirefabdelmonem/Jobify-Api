using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jobify.Services.Commons.DTOs.Resoponses
{
    public class ApiResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = null!;
        public int StatusCode { get; set; }
        public object? Data { get; set; } = null;
        public ApiResponse() { }

        public ApiResponse(bool success, string message, object? data, int statusCode = 200)
        {
            Success = success;
            Message = message;
            StatusCode = statusCode;
            Data = data;
        }
    }
}
