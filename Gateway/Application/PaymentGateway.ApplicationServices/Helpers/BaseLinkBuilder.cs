using System;
using Ardalis.GuardClauses;

namespace PaymentGateway.ApplicationServices.Helpers
{
    public abstract class BaseLinkBuilder
    {
        protected readonly string AppBaseUrl;

        protected BaseLinkBuilder(IHttpContextProvider contextProvider)
        {
            contextProvider = Guard.Against.Null(contextProvider, nameof(contextProvider));
            AppBaseUrl = contextProvider.GetAppBaseUrl();
        }
    }
}