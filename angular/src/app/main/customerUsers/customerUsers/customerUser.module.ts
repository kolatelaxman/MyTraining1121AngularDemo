import { NgModule } from '@angular/core';
import { AppSharedModule } from '@app/shared/app-shared.module';
import { AdminSharedModule } from '@app/admin/shared/admin-shared.module';
import { CustomerUserRoutingModule } from './customerUser-routing.module';
import { CustomerUsersComponent } from './customerUsers.component';
import { CreateOrEditCustomerUserModalComponent } from './create-or-edit-customerUser-modal.component';
import { ViewCustomerUserModalComponent } from './view-customerUser-modal.component';

@NgModule({
    declarations: [CustomerUsersComponent, CreateOrEditCustomerUserModalComponent, ViewCustomerUserModalComponent],
    imports: [AppSharedModule, CustomerUserRoutingModule, AdminSharedModule],
})
export class CustomerUserModule {}
