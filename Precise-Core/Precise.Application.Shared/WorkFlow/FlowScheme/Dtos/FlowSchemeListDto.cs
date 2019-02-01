using Abp.Application.Services.Dto;
using Abp.Domain.Entities;

namespace Precise.WorkFlow.Dtos
{
    public class FlowSchemeListDto : FullAuditedEntityDto<string>, IPassivable
    {

        public string SchemeCode { get; set; }

        public string SchemeName { get; set; }

        public string SchemeType { get; set; }

        public string SchemeVersion { get; set; }

        public string SchemeCanUser { get; set; }

        public string SchemeContent { get; set; }

        public string FrmId { get; set; }

        public int FrmType { get; set; }

        public int AuthorizeType { get; set; }

        public int SortCode { get; set; }

        public bool IsActive { get; set; }

        public string Description { get; set; }
    }
}