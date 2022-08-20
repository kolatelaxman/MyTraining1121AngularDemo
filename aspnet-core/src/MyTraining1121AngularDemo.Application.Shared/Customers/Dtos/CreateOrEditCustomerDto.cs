using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyTraining1121AngularDemo.Customers.Dtos
{
    public class CreateOrEditCustomerDto : EntityDto<long?>
    {

        [Required]
        [StringLength(CustomerConsts.MaxNameLength, MinimumLength = CustomerConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(CustomerConsts.MaxEmailIdLength, MinimumLength = CustomerConsts.MinEmailIdLength)]
        public string EmailId { get; set; }

        public DateTime RegistrationDate { get; set; }

        [StringLength(CustomerConsts.MaxAddressLength, MinimumLength = CustomerConsts.MinAddressLength)]
        public string Address { get; set; }

    }
}