using WebApi.Models;

namespace MyTodo.Utils
{
    public partial class WebAPIs
    {
        public async Task<HttpResponseMessage> Register(User model)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/register", model);
            return response;
        }
    }
}
