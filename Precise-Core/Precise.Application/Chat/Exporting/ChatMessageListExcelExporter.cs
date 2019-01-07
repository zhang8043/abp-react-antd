using System.Collections.Generic;
using System.Linq;
using Precise.Chat.Dto;
using Precise.DataExporting.Excel.EpPlus;
using Precise.Dto;
using Precise.Storage;

namespace Precise.Chat.Exporting
{
    public class ChatMessageListExcelExporter : EpPlusExcelExporterBase, IChatMessageListExcelExporter
    {
        public ChatMessageListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager)
        {

        }

        public FileDto ExportToFile(List<ChatMessageExportDto> messages)
        {
            var tenancyName = messages.Count > 0 ? messages.First().TargetTenantName : L("Anonymous");
            var userName = messages.Count > 0 ? messages.First().TargetUserName : L("Anonymous");

            return CreateExcelPackage(
                $"Chat_{tenancyName}_{userName}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Messages"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ChatMessage_From"),
                        L("ChatMessage_To"),
                        L("Message"),
                        L("ReadState"),
                        L("CreationTime")
                    );

                    AddObjects(
                        sheet, 2, messages,
                        _ => _.Side == ChatSide.Receiver ? (_.TargetTenantName + "/" + _.TargetUserName) : L("You"),
                        _ => _.Side == ChatSide.Receiver ? L("You") : (_.TargetTenantName + "/" + _.TargetUserName),
                        _ => _.Message,
                        _ => _.Side == ChatSide.Receiver ? _.ReadState : _.ReceiverReadState,
                        _ => _.CreationTime
                    );

                    //Formatting cells
                    var timeColumn = sheet.Column(5);
                    timeColumn.Style.Numberformat.Format = "yyyy-mm-dd hh:mm:ss";
                });
        }
    }
}