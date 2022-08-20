using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyTraining1121AngularDemo.Customers.Exporting;
using MyTraining1121AngularDemo.Customers.Dtos;
using MyTraining1121AngularDemo.Dto;
using Abp.Application.Services.Dto;
using MyTraining1121AngularDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MyTraining1121AngularDemo.Storage;

namespace MyTraining1121AngularDemo.Customers
{
    [AbpAuthorize(AppPermissions.Pages_Customers)]
    public class CustomersAppService : MyTraining1121AngularDemoAppServiceBase, ICustomersAppService
    {
        private readonly IRepository<Customer, long> _customerRepository;
        private readonly ICustomersExcelExporter _customersExcelExporter;

        public CustomersAppService(IRepository<Customer, long> customerRepository, ICustomersExcelExporter customersExcelExporter)
        {
            _customerRepository = customerRepository;
            _customersExcelExporter = customersExcelExporter;

        }

        public async Task<PagedResultDto<GetCustomerForViewDto>> GetAll(GetAllCustomersInput input)
        {

            var filteredCustomers = _customerRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.EmailId.Contains(input.Filter) || e.Address.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailIdFilter), e => e.EmailId == input.EmailIdFilter)
                        .WhereIf(input.MinRegistrationDateFilter != null, e => e.RegistrationDate >= input.MinRegistrationDateFilter)
                        .WhereIf(input.MaxRegistrationDateFilter != null, e => e.RegistrationDate <= input.MaxRegistrationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address == input.AddressFilter);

            var pagedAndFilteredCustomers = filteredCustomers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customers = from o in pagedAndFilteredCustomers
                            select new
                            {

                                o.Name,
                                o.EmailId,
                                o.RegistrationDate,
                                o.Address,
                                Id = o.Id
                            };

            var totalCount = await filteredCustomers.CountAsync();

            var dbList = await customers.ToListAsync();
            var results = new List<GetCustomerForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerForViewDto()
                {
                    Customer = new CustomerDto
                    {

                        Name = o.Name,
                        EmailId = o.EmailId,
                        RegistrationDate = o.RegistrationDate,
                        Address = o.Address,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomerForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCustomerForViewDto> GetCustomerForView(long id)
        {
            var customer = await _customerRepository.GetAsync(id);

            var output = new GetCustomerForViewDto { Customer = ObjectMapper.Map<CustomerDto>(customer) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Customers_Edit)]
        public async Task<GetCustomerForEditOutput> GetCustomerForEdit(EntityDto<long> input)
        {
            var customer = await _customerRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerForEditOutput { Customer = ObjectMapper.Map<CreateOrEditCustomerDto>(customer) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Customers_Create)]
        protected virtual async Task Create(CreateOrEditCustomerDto input)
        {
            var customer = ObjectMapper.Map<Customer>(input);

            if (AbpSession.TenantId != null)
            {
                customer.TenantId = (int?)AbpSession.TenantId;
            }

            await _customerRepository.InsertAsync(customer);

        }

        [AbpAuthorize(AppPermissions.Pages_Customers_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerDto input)
        {
            var customer = await _customerRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customer);

        }

        [AbpAuthorize(AppPermissions.Pages_Customers_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCustomersToExcel(GetAllCustomersForExcelInput input)
        {

            var filteredCustomers = _customerRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.EmailId.Contains(input.Filter) || e.Address.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailIdFilter), e => e.EmailId == input.EmailIdFilter)
                        .WhereIf(input.MinRegistrationDateFilter != null, e => e.RegistrationDate >= input.MinRegistrationDateFilter)
                        .WhereIf(input.MaxRegistrationDateFilter != null, e => e.RegistrationDate <= input.MaxRegistrationDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address == input.AddressFilter);

            var query = (from o in filteredCustomers
                         select new GetCustomerForViewDto()
                         {
                             Customer = new CustomerDto
                             {
                                 Name = o.Name,
                                 EmailId = o.EmailId,
                                 RegistrationDate = o.RegistrationDate,
                                 Address = o.Address,
                                 Id = o.Id
                             }
                         });

            var customerListDtos = await query.ToListAsync();

            return _customersExcelExporter.ExportToFile(customerListDtos);
        }



        public async Task DeleteMultipleCustomerAsyc(List<int> customerIds)
        {
            foreach (var customer in customerIds)
            {
                await _customerRepository.DeleteAsync(customer);
            }
        }


    }
}