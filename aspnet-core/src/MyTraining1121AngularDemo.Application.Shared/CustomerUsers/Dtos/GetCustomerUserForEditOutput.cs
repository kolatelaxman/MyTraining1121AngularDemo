using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MyTraining1121AngularDemo.CustomerUsers.Dtos
{
    public class GetCustomerUserForEditOutput
    {
        public CreateOrEditCustomerUserDto CustomerUser { get; set; }

        public string CustomerName { get; set; }

        public string UserName { get; set; }

    }
}