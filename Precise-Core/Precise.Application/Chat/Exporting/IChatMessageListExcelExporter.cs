using System.Collections.Generic;
using Precise.Chat.Dto;
using Precise.Dto;

namespace Precise.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(List<ChatMessageExportDto> messages);
    }
}
