using System;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Http;

namespace PaymentGateway.ApplicationServices.Helpers
{
    public interface IHttpContextProvider
    {
        string GetAppBaseUrl();
    }

    public class HttpContextProvider : IHttpContextProvider
    {
        private readonly HttpContext _current;

        public HttpContextProvider(IHttpContextAccessor accessor)
        {
            accessor = Guard.Against.Null(accessor, nameof(accessor));
            _current = accessor.HttpContext;
        }

        public string GetAppBaseUrl() => $"{_current.Request.Scheme}://{_current.Request.Host}{_current.Request.PathBase}";
    }
}