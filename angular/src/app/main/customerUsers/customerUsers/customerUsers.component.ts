import { AppConsts } from '@shared/AppConsts';
import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CustomerUsersServiceProxy, CustomerUserDto } from '@shared/service-proxies/service-proxies';
import { NotifyService } from 'abp-ng2-module';
import { AppComponentBase } from '@shared/common/app-component-base';
import { TokenAuthServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditCustomerUserModalComponent } from './create-or-edit-customerUser-modal.component';

import { ViewCustomerUserModalComponent } from './view-customerUser-modal.component';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { Table } from 'primeng/table';
import { Paginator } from 'primeng/paginator';
import { LazyLoadEvent } from 'primeng/api';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { filter as _filter } from 'lodash-es';
 
import { DateTimeService } from '@app/shared/common/timing/date-time.service';

@Component({
    templateUrl: './customerUsers.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()],
})
export class CustomerUsersComponent extends AppComponentBase {
    @ViewChild('createOrEditCustomerUserModal', { static: true })
    createOrEditCustomerUserModal: CreateOrEditCustomerUserModalComponent;
    @ViewChild('viewCustomerUserModalComponent', { static: true })
    viewCustomerUserModal: ViewCustomerUserModalComponent;

    @ViewChild('dataTable', { static: true }) dataTable: Table;
    @ViewChild('paginator', { static: true }) paginator: Paginator;

    advancedFiltersAreShown = false;
    filterText = '';
    maxBillingAmountFilter: number;
    maxBillingAmountFilterEmpty: number;
    minBillingAmountFilter: number;
    minBillingAmountFilterEmpty: number;
    customerNameFilter = '';
    userNameFilter = '';

    constructor(
        injector: Injector,
        private _customerUsersServiceProxy: CustomerUsersServiceProxy,
        private _notifyService: NotifyService,
        private _tokenAuth: TokenAuthServiceProxy,
        private _activatedRoute: ActivatedRoute,
        private _fileDownloadService: FileDownloadService,
        private _dateTimeService: DateTimeService
    ) {
        super(injector);
    }

    getCustomerUsers(event?: LazyLoadEvent) {
        if (this.primengTableHelper.shouldResetPaging(event)) {
            this.paginator.changePage(0);
            if (this.primengTableHelper.records && this.primengTableHelper.records.length > 0) {
                return;
            }
        }

        this.primengTableHelper.showLoadingIndicator();

        this._customerUsersServiceProxy
            .getAll(
                this.filterText,
                this.maxBillingAmountFilter == null ? this.maxBillingAmountFilterEmpty : this.maxBillingAmountFilter,
                this.minBillingAmountFilter == null ? this.minBillingAmountFilterEmpty : this.minBillingAmountFilter,
                this.customerNameFilter,
                this.userNameFilter,
                this.primengTableHelper.getSorting(this.dataTable),
                this.primengTableHelper.getSkipCount(this.paginator, event),
                this.primengTableHelper.getMaxResultCount(this.paginator, event)
            )
            .subscribe((result) => {
                this.primengTableHelper.totalRecordsCount = result.totalCount;
                this.primengTableHelper.records = result.items;
                this.primengTableHelper.hideLoadingIndicator();
            });
    }

    reloadPage(): void {
        this.paginator.changePage(this.paginator.getPage());
    }

    createCustomerUser(): void {
        this.createOrEditCustomerUserModal.show();
    }

    deleteCustomerUser(customerUser: CustomerUserDto): void {
        this.message.confirm('', this.l('AreYouSure'), (isConfirmed) => {
            if (isConfirmed) {
                this._customerUsersServiceProxy.delete(customerUser.id).subscribe(() => {
                    this.reloadPage();
                    this.notify.success(this.l('SuccessfullyDeleted'));
                });
            }
        });
    }

    exportToExcel(): void {
        this._customerUsersServiceProxy
            .getCustomerUsersToExcel(
                this.filterText,
                this.maxBillingAmountFilter == null ? this.maxBillingAmountFilterEmpty : this.maxBillingAmountFilter,
                this.minBillingAmountFilter == null ? this.minBillingAmountFilterEmpty : this.minBillingAmountFilter,
                this.customerNameFilter,
                this.userNameFilter
            )
            .subscribe((result) => {
                this._fileDownloadService.downloadTempFile(result);
            });
    }
}
