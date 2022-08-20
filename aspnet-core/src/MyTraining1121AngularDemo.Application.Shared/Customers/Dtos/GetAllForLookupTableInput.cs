using Abp.Application.Services.Dto;

namespace MyTraining1121AngularDemo.Customers.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}