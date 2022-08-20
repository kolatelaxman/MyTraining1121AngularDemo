using System.Collections.Generic;
using MyTraining1121AngularDemo.CustomerUsers.Dtos;
using MyTraining1121AngularDemo.Dto;

namespace MyTraining1121AngularDemo.CustomerUsers.Exporting
{
    public interface ICustomerUsersExcelExporter
    {
        FileDto ExportToFile(List<GetCustomerUserForViewDto> customerUsers);
    }
}