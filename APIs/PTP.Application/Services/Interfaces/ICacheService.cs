using PTP.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PTP.Application.Services.Interfaces
{
    public interface ICacheService
    {
        bool IsConnected();
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

        Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class;

        Task<List<T>?> GetByPrefixAsync<T>(string prefixKey, CancellationToken cancellationToken = default) where T : class;

        Task<List<T>?> GetByPrefixAsync<T>(string prefixKey, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class;

        Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class;
        Task SetByPrefixAsync<T>(string prefixKey, List<T> values, CancellationToken cancellationToken = default) where T : BaseEntity;

        Task RemoveAsync(string key, CancellationToken cancellationToken = default);

        Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
    }
}
