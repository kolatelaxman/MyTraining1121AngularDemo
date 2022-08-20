using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using MyTraining1121AngularDemo.TodoItem;
using MyTraining1121AngularDemo.TodoitemDto;
using MyTraining1121AngularDemo.TodoitemDto.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTraining1121AngularDemo.Todoitem
{
    public class TodoAppService : MyTraining1121AngularDemoAppServiceBase, ITodoAppService
    {
        private readonly IRepository<TodoApp, Guid> _todoItemRepository;

        public TodoAppService(IRepository<TodoApp, Guid> todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }
        public async Task<List<TodoItemDto>> GetListAsync()
        {
            var items = await _todoItemRepository.GetAllListAsync();

            return items
                .Select(item => new TodoItemDto
                {
                    Id = item.Id,
                    Text = item.Text
                }).ToList();
        }

        public async Task<TodoItemDto> CreateAsync(string text)
        {
            var todoItem = await _todoItemRepository.InsertAsync(
                new TodoApp { Text = text }
            );

            return new TodoItemDto
            {
                Id = todoItem.Id,
                Text = todoItem.Text
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            await _todoItemRepository.DeleteAsync(id);
        }


        public async Task DeleteMultipleTodoitemAsyc(List<Guid> TodoIds)
        {
            foreach (var todoitem in TodoIds)
            {
                await _todoItemRepository.DeleteAsync(todoitem);
            }
        }

    }
}
