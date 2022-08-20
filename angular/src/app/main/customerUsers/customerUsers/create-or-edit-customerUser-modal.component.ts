import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import {
    CustomerUsersServiceProxy,
    CreateOrEditCustomerUserDto,
    CustomerUserCustomerLookupTableDto,
    CustomerUserUserLookupTableDto,
} from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTime } from 'luxon';

import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    selector: 'createOrEditCustomerUserModal',
    templateUrl: './create-or-edit-customerUser-modal.component.html',
                
})
export class CreateOrEditCustomerUserModalComponent extends AppComponentBase implements OnInit {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    customerUser: CreateOrEditCustomerUserDto = new CreateOrEditCustomerUserDto();

    customerName = '';
    userName = '';

    allCustomers: CustomerUserCustomerLookupTableDto[];
    allUsers: CustomerUserUserLookupTableDto[];

    constructor(
        injector: Injector,
        private _customerUsersServiceProxy: CustomerUsersServiceProxy,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    show(customerUserId?: number): void {
        if (!customerUserId) {
            this.customerUser = new CreateOrEditCustomerUserDto();
            this.customerUser.id = customerUserId;
            this.customerName = '';
            this.userName = '';

            this.active = true;
            this.modal.show();
        } else {
            this._customerUsersServiceProxy.getCustomerUserForEdit(customerUserId).subscribe((result) => {
                this.customerUser = result.customerUser;

                this.customerName = result.customerName;
                this.userName = result.userName;

                this.active = true;
                this.modal.show();
            });
        }
        this._customerUsersServiceProxy.getAllCustomerForTableDropdown().subscribe((result) => {
            this.allCustomers = result;
        });
        this._customerUsersServiceProxy.getAllUserForTableDropdown().subscribe((result) => {
            this.allUsers = result;
        });
    }

    save(): void {
        this.saving = true;

        this._customerUsersServiceProxy
            .createOrEdit(this.customerUser)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.notify.info(this.l('SavedSuccessfully'));
                this.close();
                this.modalSave.emit(null);
            });
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }

    ngOnInit(): void {}
}
