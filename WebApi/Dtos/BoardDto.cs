namespace WebApi.Dtos
{
    public class BoardDto
    {
        public int BoardNo { get; set; }
        public string BoardTitle { get; set; }
        public string BoardContent { get; set; }
        public int? BoardCount { get; set; }
        public DateTime? BoardDate { get; set; }
        /// <summary>
        /// 게시글 리스트에 출력할 유저아이디(이름)
        /// </summary>
        public string? UserName { get; set; }
        /// <summary>
        /// 게시글 처리시 비교할 유저 고유 번호(DB)
        /// </summary>
		public int? UserId { get; set; }
		/// <summary>
		/// 게시글 처리시 쓸 유저 고유 번호(JWT)
		/// </summary>
		public int? Id { get; set; }
    }
}
