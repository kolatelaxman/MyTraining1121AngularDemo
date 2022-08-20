using MyTraining1121AngularDemo.Customers;
using MyTraining1121AngularDemo.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MyTraining1121AngularDemo.CustomerUsers.Exporting;
using MyTraining1121AngularDemo.CustomerUsers.Dtos;
using MyTraining1121AngularDemo.Dto;
using Abp.Application.Services.Dto;
using MyTraining1121AngularDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MyTraining1121AngularDemo.Storage;

namespace MyTraining1121AngularDemo.CustomerUsers
{
    [AbpAuthorize(AppPermissions.Pages_CustomerUsers)]
    public class CustomerUsersAppService : MyTraining1121AngularDemoAppServiceBase, ICustomerUsersAppService
    {
        private readonly IRepository<CustomerUser, long> _customerUserRepository;
        private readonly ICustomerUsersExcelExporter _customerUsersExcelExporter;
        private readonly IRepository<Customer, long> _lookup_customerRepository;
        private readonly IRepository<User, long> _lookup_userRepository;

        public CustomerUsersAppService(IRepository<CustomerUser, long> customerUserRepository, ICustomerUsersExcelExporter customerUsersExcelExporter, IRepository<Customer, long> lookup_customerRepository, IRepository<User, long> lookup_userRepository)
        {
            _customerUserRepository = customerUserRepository;
            _customerUsersExcelExporter = customerUsersExcelExporter;
            _lookup_customerRepository = lookup_customerRepository;
            _lookup_userRepository = lookup_userRepository;

        }

        public async Task<PagedResultDto<GetCustomerUserForViewDto>> GetAll(GetAllCustomerUsersInput input)
        {

            var filteredCustomerUsers = _customerUserRepository.GetAll()
                        .Include(e => e.CustomerFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinBillingAmountFilter != null, e => e.BillingAmount >= input.MinBillingAmountFilter)
                        .WhereIf(input.MaxBillingAmountFilter != null, e => e.BillingAmount <= input.MaxBillingAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerNameFilter), e => e.CustomerFk != null && e.CustomerFk.Name == input.CustomerNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var pagedAndFilteredCustomerUsers = filteredCustomerUsers
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var customerUsers = from o in pagedAndFilteredCustomerUsers
                                join o1 in _lookup_customerRepository.GetAll() on o.CustomerId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                                from s2 in j2.DefaultIfEmpty()

                                select new
                                {

                                    o.BillingAmount,
                                    Id = o.Id,
                                    CustomerName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                    UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                };

            var totalCount = await filteredCustomerUsers.CountAsync();

            var dbList = await customerUsers.ToListAsync();
            var results = new List<GetCustomerUserForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCustomerUserForViewDto()
                {
                    CustomerUser = new CustomerUserDto
                    {

                        BillingAmount = o.BillingAmount,
                        Id = o.Id,
                    },
                    CustomerName = o.CustomerName,
                    UserName = o.UserName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCustomerUserForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCustomerUserForViewDto> GetCustomerUserForView(long id)
        {
            var customerUser = await _customerUserRepository.GetAsync(id);

            var output = new GetCustomerUserForViewDto { CustomerUser = ObjectMapper.Map<CustomerUserDto>(customerUser) };

            if (output.CustomerUser.CustomerId != null)
            {
                var _lookupCustomer = await _lookup_customerRepository.FirstOrDefaultAsync((long)output.CustomerUser.CustomerId);
                output.CustomerName = _lookupCustomer?.Name?.ToString();
            }

            if (output.CustomerUser.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CustomerUser.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CustomerUsers_Edit)]
        public async Task<GetCustomerUserForEditOutput> GetCustomerUserForEdit(EntityDto<long> input)
        {
            var customerUser = await _customerUserRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCustomerUserForEditOutput { CustomerUser = ObjectMapper.Map<CreateOrEditCustomerUserDto>(customerUser) };

            if (output.CustomerUser.CustomerId != null)
            {
                var _lookupCustomer = await _lookup_customerRepository.FirstOrDefaultAsync((long)output.CustomerUser.CustomerId);
                output.CustomerName = _lookupCustomer?.Name?.ToString();
            }

            if (output.CustomerUser.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.CustomerUser.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCustomerUserDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CustomerUsers_Create)]
        protected virtual async Task Create(CreateOrEditCustomerUserDto input)
        {
            var customerUser = ObjectMapper.Map<CustomerUser>(input);

            if (AbpSession.TenantId != null)
            {
                customerUser.TenantId = (int?)AbpSession.TenantId;
            }

            await _customerUserRepository.InsertAsync(customerUser);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerUsers_Edit)]
        protected virtual async Task Update(CreateOrEditCustomerUserDto input)
        {
            var customerUser = await _customerUserRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, customerUser);

        }

        [AbpAuthorize(AppPermissions.Pages_CustomerUsers_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _customerUserRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCustomerUsersToExcel(GetAllCustomerUsersForExcelInput input)
        {

            var filteredCustomerUsers = _customerUserRepository.GetAll()
                        .Include(e => e.CustomerFk)
                        .Include(e => e.UserFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinBillingAmountFilter != null, e => e.BillingAmount >= input.MinBillingAmountFilter)
                        .WhereIf(input.MaxBillingAmountFilter != null, e => e.BillingAmount <= input.MaxBillingAmountFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CustomerNameFilter), e => e.CustomerFk != null && e.CustomerFk.Name == input.CustomerNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

            var query = (from o in filteredCustomerUsers
                         join o1 in _lookup_customerRepository.GetAll() on o.CustomerId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetCustomerUserForViewDto()
                         {
                             CustomerUser = new CustomerUserDto
                             {
                                 BillingAmount = o.BillingAmount,
                                 Id = o.Id
                             },
                             CustomerName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var customerUserListDtos = await query.ToListAsync();

            return _customerUsersExcelExporter.ExportToFile(customerUserListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_CustomerUsers)]
        public async Task<List<CustomerUserCustomerLookupTableDto>> GetAllCustomerForTableDropdown()
        {
            return await _lookup_customerRepository.GetAll()
                .Select(customer => new CustomerUserCustomerLookupTableDto
                {
                    Id = customer.Id,
                    DisplayName = customer == null || customer.Name == null ? "" : customer.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_CustomerUsers)]
        public async Task<List<CustomerUserUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new CustomerUserUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

    }
}