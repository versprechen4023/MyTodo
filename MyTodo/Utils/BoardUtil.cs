using WebApi.Models;

namespace MyTodo.Utils
{
    public partial class WebAPIs
    {
        public async Task<HttpResponseMessage> GetBoardList(int? pageNumber, int pageSize, int pageBlock)
        {
            var response = await _httpClient.GetAsync($"/api/board?pageNumber={pageNumber}&pageSize={pageSize}&pageBlock={pageBlock}");
            return response;
        }

        public async Task<HttpResponseMessage> GetBoardDetail(int boardNo)
        {
            var response = await _httpClient.GetAsync($"/api/board/{boardNo}");
            return response;
        }

		public async Task<HttpResponseMessage> PutBoard(Board model)
        {
			var response = await _httpClient.PostAsJsonAsync("/api/board/", model);
			return response;
		}

        public async Task<HttpResponseMessage> DeleteBoard(int boardNo, int userId)
        {
            var response = await _httpClient.DeleteAsync($"/api/board/{boardNo}/{userId}");
            return response;
        }

        public async Task<HttpResponseMessage> UpdateBoard(Board model)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/board", model);
            return response;
        }

	}
}
