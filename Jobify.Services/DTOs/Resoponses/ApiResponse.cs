using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Jobify.Services.DTOs.Resoponses
{
    public  class ApiResponse
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = null!;
        public object? Data { get; set; } = null;
        public ApiResponse() { }

        public ApiResponse(bool success, string message, object? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
