using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyTraining1121AngularDemo.CustomerUsers.Dtos;
using MyTraining1121AngularDemo.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace MyTraining1121AngularDemo.CustomerUsers
{
    public interface ICustomerUsersAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomerUserForViewDto>> GetAll(GetAllCustomerUsersInput input);

        Task<GetCustomerUserForViewDto> GetCustomerUserForView(long id);

        Task<GetCustomerUserForEditOutput> GetCustomerUserForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomerUserDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCustomerUsersToExcel(GetAllCustomerUsersForExcelInput input);

        Task<List<CustomerUserCustomerLookupTableDto>> GetAllCustomerForTableDropdown();

        Task<List<CustomerUserUserLookupTableDto>> GetAllUserForTableDropdown();

    }
}