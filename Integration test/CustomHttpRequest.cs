using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;


namespace Integration_test
{
    internal class CustomHttpRequestData
    {
        public static HttpRequest GetHttpRequest()
        {
            var context = new DefaultHttpContext();
            return context.Request;
        }
    }
}
