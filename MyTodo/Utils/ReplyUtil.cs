﻿using WebApi.Dtos;
using WebApi.Models;

namespace MyTodo.Utils
{
    public partial class WebAPIs
    {
        public async Task<HttpResponseMessage> PutReply(Reply model)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/reply/", model);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteReply(int replyNo, int userId)
        {
			var response = await _httpClient.DeleteAsync($"/api/reply/{replyNo}/{userId}");
			return response;
		}
    }
}
