using System.Collections.Generic;
using MyTraining1121AngularDemo.Customers.Dtos;
using MyTraining1121AngularDemo.Dto;

namespace MyTraining1121AngularDemo.Customers.Exporting
{
    public interface ICustomersExcelExporter
    {
        FileDto ExportToFile(List<GetCustomerForViewDto> customers);
    }
}