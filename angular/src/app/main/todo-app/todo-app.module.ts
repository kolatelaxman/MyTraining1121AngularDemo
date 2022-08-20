import {NgModule} from '@angular/core';
import {AppSharedModule} from '@app/shared/app-shared.module';
import {AdminSharedModule} from '@app/admin/shared/admin-shared.module';
 
import { TodoAppComponent } from './todo-app.component';
import { TodoitemRoutingModule } from './todo-app.routing.module';



@NgModule({
    declarations: [TodoAppComponent
    ],
    imports: [AppSharedModule, TodoitemRoutingModule , AdminSharedModule ],
    
})
export class todoitemModule {
}
