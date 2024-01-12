namespace WebApi.Dtos
{
    public class BoardDto
    {
        public int BoardNo { get; set; }
        public string BoardTitle { get; set; }
        public string BoardContent { get; set; }
        public int? BoardCount { get; set; }
        public DateTime? BoardDate { get; set; }
        public string UserName { get; set; }
    }
}
