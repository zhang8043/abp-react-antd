using Abp.Runtime.Validation;
using Precise.Dto;

namespace Precise.DataItems.Dtos
{
    public class GetItemsDetailEntitysInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

    }
}
