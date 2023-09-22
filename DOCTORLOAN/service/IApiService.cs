using DOCTORLOAN.Models.Users;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace DOCTORLOAN.service
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request);

        // Other methhod requiments
    }
}
