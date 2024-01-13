using System.IdentityModel.Tokens.Jwt;

namespace MyTodo.Utils
{
    public class JwtDecoder
    {
        // 토큰 디코딩 함수
        public static JwtSecurityToken? DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            return jwtSecurityToken;
        }

        // 토큰에서 UserId(번호)얻기
        public static string GetUserIdFromClaims(string token)
        {
            var jwtSecurityToken = DecodeToken(token);
            var userId = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            return userId.ToString();
        }

        // 토큰 만료일 얻기
        public static DateTime GetExpirationDate(string token)
        {
            var jwtSecurityToken = DecodeToken(token);
            return jwtSecurityToken.ValidTo;
        }
    }
}
