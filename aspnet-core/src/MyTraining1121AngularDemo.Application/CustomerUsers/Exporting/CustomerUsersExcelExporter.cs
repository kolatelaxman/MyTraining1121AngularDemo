using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using MyTraining1121AngularDemo.DataExporting.Excel.NPOI;
using MyTraining1121AngularDemo.CustomerUsers.Dtos;
using MyTraining1121AngularDemo.Dto;
using MyTraining1121AngularDemo.Storage;

namespace MyTraining1121AngularDemo.CustomerUsers.Exporting
{
    public class CustomerUsersExcelExporter : NpoiExcelExporterBase, ICustomerUsersExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CustomerUsersExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCustomerUserForViewDto> customerUsers)
        {
            return CreateExcelPackage(
                "CustomerUsers.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("CustomerUsers"));

                    AddHeader(
                        sheet,
                        L("BillingAmount"),
                        (L("Customer")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, customerUsers,
                        _ => _.CustomerUser.BillingAmount,
                        _ => _.CustomerName,
                        _ => _.UserName
                        );

                });
        }
    }
}