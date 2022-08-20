import { AppConsts } from '@shared/AppConsts';
import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { GetCustomerUserForViewDto, CustomerUserDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';

@Component({
    selector: 'viewCustomerUserModal',
    templateUrl: './view-customerUser-modal.component.html',
})
export class ViewCustomerUserModalComponent extends AppComponentBase {
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: GetCustomerUserForViewDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new GetCustomerUserForViewDto();
        this.item.customerUser = new CustomerUserDto();
    }

    show(item: GetCustomerUserForViewDto): void {
        this.item = item;
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
