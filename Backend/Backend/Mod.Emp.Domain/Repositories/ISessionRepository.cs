using CommercialSystem.Shared.Domain.Repositories;
using Mod.Emp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mod.Emp.Domain.Repositories
{
    public interface ISessionRepository<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        Task<TResponse> ValidateCredentialsAsync(TRequest request);
    }
}
