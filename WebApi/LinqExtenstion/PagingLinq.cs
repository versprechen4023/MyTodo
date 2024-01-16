using Microsoft.EntityFrameworkCore;
using WebApi.Dtos;

namespace WebApi.LinqExtenstion
{
    public static class PagingLinq
    {
        public static async Task<PagingDto<T>> ToPagingAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize, int pageBlock)
        {
            // 전체 게시글 수를 카운트 함수를 이용해 가져온다
            var totalCount = await query.CountAsync();

            // Skip(n) 배열에서 n개만큼 건너뛰고 출력 시퀀스를 만든다
            // Take(n) 배열에서 처음 n개만큼 원소를꺼내 출력 시퀀스를 만든다

            // 계산식 2페이지 기준 (ex (2-1) * 10 = 10) 만큼 (Skip)잘라낸다 이후 20번째부터 30번째까지 (Take)가져온다(20~30)
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagingDto<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                PageBlock = pageBlock
            };
        }
    }
}
