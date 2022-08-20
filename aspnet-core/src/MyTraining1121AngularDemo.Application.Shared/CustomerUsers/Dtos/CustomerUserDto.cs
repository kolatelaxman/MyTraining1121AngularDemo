using System;
using Abp.Application.Services.Dto;

namespace MyTraining1121AngularDemo.CustomerUsers.Dtos
{
    public class CustomerUserDto : EntityDto<long>
    {
        public decimal BillingAmount { get; set; }

        public long? CustomerId { get; set; }

        public long? UserId { get; set; }

    }
}