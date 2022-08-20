using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyTraining1121AngularDemo.Customers
{
    [Table("Customers")]
    public class Customer : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        [StringLength(CustomerConsts.MaxNameLength, MinimumLength = CustomerConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(CustomerConsts.MaxEmailIdLength, MinimumLength = CustomerConsts.MinEmailIdLength)]
        public virtual string EmailId { get; set; }

        public virtual DateTime RegistrationDate { get; set; }

        [StringLength(CustomerConsts.MaxAddressLength, MinimumLength = CustomerConsts.MinAddressLength)]
        public virtual string Address { get; set; }

    }
}