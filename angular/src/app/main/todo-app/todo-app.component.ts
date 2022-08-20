import { Component, OnInit } from '@angular/core';
import { TodoItemDto, TodoServiceProxy } from '@shared/service-proxies/service-proxies';
import * as _ from 'lodash';

@Component({
  selector: 'app-todo-app',
  templateUrl: './todo-app.component.html',
  styleUrls: ['./todo-app.component.css']
})
export class TodoAppComponent implements OnInit {
  [x: string]: any;

  todoItems: TodoItemDto[];
  newTodoText: string;
 

     
  isDeleted = [];
  

  constructor(
       private todoService: TodoServiceProxy)
  { }

  ngOnInit(): void {
    this.todoService.getList().subscribe(response => {
      this.todoItems = response;
    });
  }
  
  create(): void{
    this.todoService.create(this.newTodoText).subscribe((result) => {
      this.todoItems = this.todoItems.concat(result);
      this.newTodoText = null;
    });
  }

  delete(id: string): void {
    this.todoService.delete(id).subscribe(() => {
      this.todoItems = this.todoItems.filter(item => item.id !== id);
      this.todoService.delete('Deleted the todo item.');

     });
  }  

  value(val:number) {

    console.log("val= ",val);

    var u= this.isDeleted.push(val);

    console.log("u=",u);

    return u;

}
 

deletetodoSelected() {

  console.log(this.isDeleted);

  this.todoService.deleteMultipleTodoitemAsyc(this.isDeleted).subscribe(() => {

    //this.notify.info(this.l('SuccessfullyDeleted'));


      this.notify.info(this.l('SuccessfullyDeleted'))
      this.isDeleted.forEach(element => {

          _.remove(this.todoItems, element);

      });

  });

  window.location.reload();

}



}
