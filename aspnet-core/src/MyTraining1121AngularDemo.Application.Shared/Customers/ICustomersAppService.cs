using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MyTraining1121AngularDemo.Customers.Dtos;
using MyTraining1121AngularDemo.Dto;

namespace MyTraining1121AngularDemo.Customers
{
    public interface ICustomersAppService : IApplicationService
    {
        Task<PagedResultDto<GetCustomerForViewDto>> GetAll(GetAllCustomersInput input);

        Task<GetCustomerForViewDto> GetCustomerForView(long id);

        Task<GetCustomerForEditOutput> GetCustomerForEdit(EntityDto<long> input);

        Task CreateOrEdit(CreateOrEditCustomerDto input);

        Task Delete(EntityDto<long> input);

        Task<FileDto> GetCustomersToExcel(GetAllCustomersForExcelInput input);

    }
}