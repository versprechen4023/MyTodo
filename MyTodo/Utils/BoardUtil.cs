namespace MyTodo.Utils
{
    public partial class WebAPIs
    {
        public async Task<HttpResponseMessage> GetBoardList(int? pageNumber, int pageSize, int pageBlock)
        {
            var response = await _httpClient.GetAsync($"/api/board?pageNumber={pageNumber}&pageSize={pageSize}&pageBlock={pageBlock}");
            return response;
        }
    }
}
