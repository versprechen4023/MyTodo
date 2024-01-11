namespace MyTodo.Utils
{
    /// <summary>
    /// 모든 WebApi를 관리하는 분할 클래스
    /// </summary>
    public partial class WebAPIs
    {
        // 의존성 주입 WebApi접근을 위한 HttpClient, 쿠키접근을 위한 Accessor, 로거 
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<WebAPIs> _logger;

        public WebAPIs(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ILogger<WebAPIs> logger)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
    }
}
