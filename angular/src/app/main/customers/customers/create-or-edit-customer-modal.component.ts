import { Component, ViewChild, Injector, Output, EventEmitter, OnInit, ElementRef} from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';
import { CustomersServiceProxy, CreateOrEditCustomerDto, CustomerUserCustomerLookupTableDto, CustomerUserUserLookupTableDto, CustomerUsersServiceProxy, CreateOrEditCustomerUserDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';



@Component({
    selector: 'createOrEditCustomerModal',
    templateUrl: './create-or-edit-customer-modal.component.html'
})
export class CreateOrEditCustomerModalComponent extends AppComponentBase implements OnInit{
   
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;
     
    allUsers: CustomerUserUserLookupTableDto[];
    customer: CreateOrEditCustomerDto = new CreateOrEditCustomerDto();
    customerUser: CreateOrEditCustomerUserDto = new CreateOrEditCustomerUserDto();
   
    customerName = '';
    userName = '';


    constructor(
        injector: Injector,
        private _customersServiceProxy: CustomersServiceProxy,
        private _customerUserServiceProxy:CustomerUsersServiceProxy,
             private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }
    
    show(customerId?: number): void {
    

        if (!customerId) {
            this.customer = new CreateOrEditCustomerDto();
            this.customer.id = customerId;
            this.customer.registrationDate = this._dateTimeService.getStartOfDay();


            this.active = true;
            this.modal.show();
        } else {
            this._customersServiceProxy.getCustomerForEdit(customerId).subscribe(result => {
                this.customer = result.customer;

               

                this.active = true;
                this.modal.show();
            });
        }
        
        this._customerUserServiceProxy.getAllUserForTableDropdown().subscribe((result)=>{
            this.allUsers=result;
        })
        
    }

    save(): void {
            this.saving = true;
            
            this._customersServiceProxy.createOrEdit(this.customer)
             .pipe(finalize(() => { this.saving = false;}))
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
    
     ngOnInit(): void {
        
     }    
}
