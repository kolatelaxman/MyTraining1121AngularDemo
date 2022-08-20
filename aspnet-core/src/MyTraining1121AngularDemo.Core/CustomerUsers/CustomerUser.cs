using MyTraining1121AngularDemo.Customers;
using MyTraining1121AngularDemo.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace MyTraining1121AngularDemo.CustomerUsers
{
    [Table("CustomerUsers")]
    public class CustomerUser : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public virtual decimal BillingAmount { get; set; }

        public virtual long? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer CustomerFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

    }
}