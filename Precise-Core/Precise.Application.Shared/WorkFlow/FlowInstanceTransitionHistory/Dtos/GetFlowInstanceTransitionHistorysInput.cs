using Abp.Runtime.Validation;
using Precise.Dto;

namespace Precise.WorkFlow.Dtos
{
    public class GetFlowInstanceTransitionHistorysInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
