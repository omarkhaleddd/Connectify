using Talabat.APIs.DTO;

namespace Talabat.APIs.Helpers
{
    public class Pagination<T>
    {
      //  private IReadOnlyList<ProductToReturnDto> mappedProduct;

        //public Pagination(int pageSize, int pageIndex, IReadOnlyList<T> data,int count)
        //{
        //    PageSize = pageSize;
        //    PageIndex = pageIndex;
        //    Data = data;
        //    Count = count;
        //}

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }
}
