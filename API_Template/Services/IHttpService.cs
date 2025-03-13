
namespace API_Template.Services
{
    public interface IHttpService
    {
        Task DeleteAsync(string endpoint);
        Task<T> GetAsync<T>(string endpoint);
        Task<T> PostAsync<T>(string endpoint, object data);
        Task<T> PutAsync<T>(string endpoint, object data);
    }
}