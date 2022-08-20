import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                children: [
                    
                    {
                        path: 'customerUsers/customerUsers',
                        loadChildren: () => import('./customerUsers/customerUsers/customerUser.module').then(m => m.CustomerUserModule),
                        data: { permission: 'Pages.CustomerUsers' }
                    },
                
                    
                    {
                        path: 'customers/customers',
                        loadChildren: () => import('./customers/customers/customer.module').then(m => m.CustomerModule),
                        data: { permission: 'Pages.Customers' }
                    },

                    {
                        path: 'todo-app/todo-app',
                        loadChildren: () => import('./todo-app/todo-app.module').then((m) => m.todoitemModule),
                     },
                
                    {
                        path: 'dashboard',
                        loadChildren: () => import('./dashboard/dashboard.module').then((m) => m.DashboardModule),
                        data: { permission: 'Pages.Tenant.Dashboard' },
                    },
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: '**', redirectTo: 'dashboard' },
                ],
            },
        ]),
    ],
    exports: [RouterModule],
})
export class MainRoutingModule {}
