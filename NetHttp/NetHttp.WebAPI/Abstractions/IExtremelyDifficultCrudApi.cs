using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;

namespace NetHttp.WebAPI.Abstractions
{
    public interface IExtremelyDifficultCrudApi<TYourType, in TKey> where TYourType : class
    {
        [Get("")]
        Task<ApiResponse<List<TYourType>>> GetAll();

        [Get("/{key}")]
        Task<ApiResponse<TYourType>> GetById(TKey key);
        
        [Post("")]
        Task<ApiResponse<TYourType>> Create([Body] TYourType payload);

        [Put("/{key}")]
        Task<ApiResponse<TYourType>> Update(TKey key, [Body] TYourType payload);

        [Delete("/{key}")]
        Task Delete(TKey key);
    }
}