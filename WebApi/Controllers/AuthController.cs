using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Models;

namespace WebApi.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // 의존성 주입 로거, 설정, DB컨텍스트
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DBContext _dbContext;

        public AuthController(ILogger<AuthController> logger, IConfiguration configuration, DBContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            _logger.LogWarning("회원가입 실행");
            try
            {
                // 이미 아이디 존재할경우 에러 메시지 반환
                if (await _dbContext.Users.AnyAsync(u => u.UserName == model.UserName))
                {
                    return BadRequest(new { message = "이미 존재하는 아이디 입니다" });
                }

                if (ModelState.IsValid)
                {
                    // 유저 권한을 가져옴 DB에 권한 내역없다면 NULL로 삽입되므로 미리 INSERT 필요
                    // 기본은 1 Admin, 2 User
                    // DB컨텍스트 설정에서 자동 입력값 추가 마이그레이션시 자동삽입
                    Role role = await _dbContext.Roles.SingleOrDefaultAsync(r => r.Id == 1);

                    var user = new User { UserName = model.UserName, Password = BCrypt.Net.BCrypt.HashPassword(model.Password), Role = role };
                    _dbContext.Users.Add(user);
                    await _dbContext.SaveChangesAsync();
                    return Ok();
                }
                return BadRequest(new { message = "회원가입 처리중에 문제가 발생했습니다" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = "회원가입 처리중에 문제가 발생했습니다" });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User model)
        {
            _logger.LogWarning("로그인 실행");

            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _dbContext.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.UserName == model.UserName);

                    if(user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                    {
                        string accessToken = GenerateAccessToken(user);

                        if(string.IsNullOrEmpty(accessToken) == true)
                        {
                            return BadRequest(new { message = "인증 토큰 생성에 실패했습니다" });
                        }

                        string refreshToken = GenerateRefreshToken();

                        user.RefreshToken = refreshToken;
                        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                        await _dbContext.SaveChangesAsync();

                        return Ok(new { Token = accessToken, RefreshToken = refreshToken });
                    }
                    else
                    {
                        return BadRequest(new { message = "아이디 혹은 비밀번호가 일치하지 않습니다" });
                    }
                }
                return BadRequest(new { message = "로그인 중에 문제가 발생했습니다" });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new { message = "로그인 중에 문제가 발생했습니다" });

            }
        }

		[HttpPost("refresh")]
		public async Task<IActionResult> Refresh([FromBody] string refreshToken)
		{
			_logger.LogWarning("리프레시 토큰 발급 실행");
			try
			{
                var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiry > DateTime.UtcNow);

                if(user != null)
                {
					string accessToken = GenerateAccessToken(user);

					if (string.IsNullOrEmpty(accessToken) == true)
					{
						return BadRequest();
					}

					return Ok(new { Token = accessToken });
                }
                else
                {
                    // 로그인 정보 불일치시
                    return BadRequest();
                }
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return BadRequest(new { message = "인증 토큰 생성에 실패했습니다." });
			}
		}
		// 리프레시 토큰 발급 메서드
		private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        // JWT 토큰 발급 메서드
        private string GenerateAccessToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                // 토큰의 비밀 키 지정
                var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);

                // 클레임에는 유저네임(아이디), 권한, 유저번호를 저장
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Role, user.Role.Name),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    }),

                    // 토큰 유효시간은 1시간
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _configuration["JwtSettings:Issuer"],
                    Audience = _configuration["JwtSettings:Audience"]
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return tokenString;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
