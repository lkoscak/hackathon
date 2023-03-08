using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseApiContext.ServiceResponse
{
    public class ServiceResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }
    }
}