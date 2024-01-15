using WebApi.Models;

namespace MyTodo.Utils
{
    public partial class WebAPIs
    {
        public async Task<HttpResponseMessage> GetTodoList(int userId)
        {
            var response = await _httpClient.GetAsync($"/api/todo?userId={userId}");
            return response;
        }

        public async Task<HttpResponseMessage> PutTodo(Todo model)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/todo/", model);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteTodo(int todoNo, int userId)
        {
            var response = await _httpClient.DeleteAsync($"/api/todo/{todoNo}/{userId}");
            return response;
        }

        public async Task<HttpResponseMessage> UpdateTodo(Todo model)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/todo", model);
            return response;
        }
    }
}
