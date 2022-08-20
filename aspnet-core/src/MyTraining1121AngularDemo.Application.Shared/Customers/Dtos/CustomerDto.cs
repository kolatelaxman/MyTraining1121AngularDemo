using System;
using Abp.Application.Services.Dto;

namespace MyTraining1121AngularDemo.Customers.Dtos
{
    public class CustomerDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string EmailId { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool SelectId { get; set; }

        public string Address { get; set; }

    }
}