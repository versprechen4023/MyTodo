using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebApi.Dtos;
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

        public async Task<HttpResponseMessage> Login(User model)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", model);
            return response;
        }

        /// <summary>
        /// 토큰 헤더 세팅
        /// </summary>
        /// <param name="token"></param>
		public void SetAccessToken(string token)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
		}

        /// <summary>
        /// 토큰 유효기간 검증
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
		public bool IsTokenExpired(string token)
		{

			if (token.IsNullOrEmpty())
			{
				return true;
			}

			var expirationDate = JwtDecoder.GetExpirationDate(token);

			return expirationDate < DateTime.UtcNow;
		}

		/// <summary>
		/// 리프레시 토큰 재발급
		/// </summary>
		/// <returns></returns>
		public async Task<bool> RefreshToken()
		{
			var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["RefreshToken"];

			if (string.IsNullOrEmpty(refreshToken))
			{
				return false;
			}

			var response = await _httpClient.PostAsJsonAsync("/api/auth/refresh", refreshToken);

			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();
				var token = JsonConvert.DeserializeObject<TokenDto>(result);

				_httpContextAccessor.HttpContext.Response.Cookies.Append("AccessToken", token.Token, new CookieOptions
				{
					HttpOnly = true,
					SameSite = SameSiteMode.Strict
				});
				return true;
			}

			return false;
		}
		public async Task<HttpResponseMessage> GetUserAction()
		{
            var response = await _httpClient.GetAsync("/api/user/useraction");
			return response;
        }
	}
}
