using Abp.Domain.Entities.Auditing;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining1121AngularDemo.TodoItem
{
    public class TodoApp: FullAuditedEntity<Guid>
    {
        public string Text { get; set; }

     }
}
