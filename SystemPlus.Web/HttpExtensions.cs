﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemPlus.Web
{
    public static class HttpExtensions
    {
        public static string UserAgent(this HttpRequest request)
        {
            return request.Headers["User-Agent"];
        }

        public static string Referer(this HttpRequest request)
        {
            return request.Headers["Referer"];
        }

        public static string GetIpAddress(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}