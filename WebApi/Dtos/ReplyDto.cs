

namespace WebApi.Dtos
{
	public class ReplyDto
	{
		public int ReplyNo { get; set; }
		public string ReplyContent { get; set; }
		public DateTime? ReplyDate { get; set; } = DateTime.Now;
		public int? UserId { get; set; }
		public string? UserName { get; set; }
	}
}
