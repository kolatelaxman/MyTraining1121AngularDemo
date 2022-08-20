import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CustomerUsersComponent } from './customerUsers.component';

const routes: Routes = [
    {
        path: '',
        component: CustomerUsersComponent,
        pathMatch: 'full',
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class CustomerUserRoutingModule {}
