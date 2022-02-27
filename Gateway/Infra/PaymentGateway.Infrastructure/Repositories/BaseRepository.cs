using System;
using Ardalis.GuardClauses;
using PaymentGateway.Infrastructure.Context;

namespace PaymentGateway.Infrastructure.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext DbContext;

        protected BaseRepository(AppDbContext context)
        {
            DbContext = Guard.Against.Null(context, nameof(context));
        }
    }
}