namespace WebApi.Dtos
{
    public class PagingDto<T>
    {
        public List<T> Items { get; set; } // 현재 페이지의 모델 아이템들
        public int TotalCount { get; set; } // 게시판 전체 글 개수
        public int PageSize { get; set; } // 한 페이지에서 보이는 글 개수
        public int PageNumber { get; set; } // 현재 페이지
        public int PageBlock { get; set; } // 한 화면에 보여줄 페이지 개수
        public int PageCount // 전체 페이지 개수
        {
            get
            {
                return TotalCount / PageSize + (TotalCount % PageSize == 0 ? 0 : 1);
            }
        }

        public int StartPage // 시작하는 페이지 번호 계산
        {
            get
            {
                return (PageNumber - 1) / PageBlock * PageBlock + 1;
            }
        }
        public int EndPage // 끝나는 페이지 번호 계산
        {
            get
            {
                var page = StartPage + PageBlock - 1;
                if (page > PageCount)
                {
                    page = PageCount;
                }
                return page;
            }
        }
    }
}
