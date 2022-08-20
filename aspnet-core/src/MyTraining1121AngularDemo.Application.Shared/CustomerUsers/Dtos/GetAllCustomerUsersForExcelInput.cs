using Abp.Application.Services.Dto;
using System;

namespace MyTraining1121AngularDemo.CustomerUsers.Dtos
{
    public class GetAllCustomerUsersForExcelInput
    {
        public string Filter { get; set; }

        public decimal? MaxBillingAmountFilter { get; set; }
        public decimal? MinBillingAmountFilter { get; set; }

        public string CustomerNameFilter { get; set; }

        public string UserNameFilter { get; set; }

    }
}