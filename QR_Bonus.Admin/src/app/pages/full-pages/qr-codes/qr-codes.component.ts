import { Component, OnInit, ViewChild } from '@angular/core';
import { DynamicFilterItem, DFControlType } from 'app/shared/components/dynamic-filters/dynamic-filters.component';
import { ActionType, DynamicTableComponent, DynamicTableItemType, DynamicTableSettings, TableStatusItem } from 'app/shared/components/dynamic-table/dynamic-table.component';
import { SelectItem } from 'app/shared/models/select-item.model';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ScannerComponent } from '../scanner/scanner.component';
import { ModalResponse } from 'app/shared/models/modal-response.model';
import { QrCodeModel } from 'app/shared/models/qr-code.model';
import { ToastrService } from 'ngx-toastr';
import { ConfirmActionDialogComponent } from 'app/shared/components/popups/confirm-action-dialog/confirm-action-dialog.component';
import { RegionModel } from 'app/shared/models/region.model';
import { QrScannerDeviceComponent } from '../qr-scanner-device/qr-scanner-device.component';
import { QrScannerDevicePopupComponent } from 'app/pages/full-pages/qr-scanner-device-popup/qr-scanner-device-popup.component';
import { DownloadFileHelper } from 'app/shared/utility/file-download-helper';

@Component({
  selector: 'app-qr-codes',
  templateUrl: './qr-codes.component.html',
  styleUrls: ['./qr-codes.component.scss']
})
export class QrCodesComponent implements OnInit {
  @ViewChild('table') table: DynamicTableComponent;

  private statuses: Array<SelectItem> = [
    {
      id: null,
      name: this.translationPipe.transform('admin_all')
    },
    {
      id: true,
      name: this.translationPipe.transform('admin_has_prize'),
    },
    {
      id: false,
      name: this.translationPipe.transform('admin_without_prize')
    }
  ]

  public isExportedItems: Array<SelectItem> = [
    {
      id: null,
      name: this.translationPipe.transform('admin_all')
    },
    {
      id: true,
      name: this.translationPipe.transform('admin_is_qr_exported')
    },
    {
      id: false,
      name: this.translationPipe.transform('admin_is_qr_not_exported')
    }
  ]

  public filters: Array<DynamicFilterItem> = [
    {
      propName: 'prizeId',
      type: DFControlType.Select,
      source: Urls.Prize,
      displayId: true,
      label: this.translationPipe.transform('admin_prize_name'),
      placeholder: this.translationPipe.transform('admin_prize_name'),
    },
    {
      propName: 'customerId',
      type: DFControlType.Select,
      source: Urls.Customer,
      displayId: false,
      displayName: ['phoneNumber', 'firstName', 'lastName'],
      searchProp: 'phoneNumber',
      label: this.translationPipe.transform('admin_customer_phone_number_v2'),
      placeholder: this.translationPipe.transform('admin_customer_phone_number_v2')
    },
    {
      propName: 'userIds',
      type: DFControlType.Select,
      source: Urls.User,
      displayId: false,
      displayName: ['firstName', 'lastName'],
      multiple: true,
      searchProp: 'firstName',
      label: this.translationPipe.transform('admin_user'),
      placeholder: this.translationPipe.transform('admin_user')
    },
    {
      propName: 'winDate',
      type: DFControlType.DateTime,
      label: this.translationPipe.transform('admin_win_date'),
      placeholder: this.translationPipe.transform('admin_win_date')
    },
    {
      propName: 'hasPrize',
      type: DFControlType.Select,
      items: this.statuses,
      label: this.translationPipe.transform('admin_has_prize'),
      placeholder: this.translationPipe.transform('admin_has_prize')
    },
    {
      propName: 'isExported',
      type: DFControlType.Select,
      items: this.isExportedItems,
      label: this.translationPipe.transform('admin_is_qr_exported'),
      placeholder: this.translationPipe.transform('admin_is_qr_exported'),
      defaultValue: false
    },
    {
      propName: 'value',
      type: DFControlType.Text,
      label: this.translationPipe.transform('admin_value'),
      placeholder: this.translationPipe.transform('admin_value'),
    },
    {
      propName: 'regionIds',
      type: DFControlType.Select,
      source: Urls.Region,
      multiple: true,
      label: this.translationPipe.transform('admin_regions'),
      placeholder: this.translationPipe.transform('admin_regions')
    },
    {
      propName: 'prizeReceiveDateFrom',
      type: DFControlType.DateTime,
      isDateFrom: true,
      label: this.translationPipe.transform('admin_date_of_receiving_the_prize_from'),
      placeholder: this.translationPipe.transform('admin_date_of_receiving_the_prize_from')
    },
    {
      propName: 'prizeReceiveDateTo',
      type: DFControlType.DateTime,
      isDateTo: true,
      label: this.translationPipe.transform('admin_date_of_receiving_the_prize_to'),
      placeholder: this.translationPipe.transform('admin_date_of_receiving_the_prize_to')
    },
    {
      propName: 'isWinReceived',
      type: DFControlType.SwitchToggle,
      returnNullWhenValueIsFalse: true,
      label: this.translationPipe.transform('admin_is_prize_received'),
      placeholder: this.translationPipe.transform('admin_is_prize_received')
    },
    {
      propName: 'hasCustomerConfirmed',
      type: DFControlType.SwitchToggle,
      returnNullWhenValueIsFalse: true,
      label: this.translationPipe.transform('admin_received_by_customer'),
      placeholder: this.translationPipe.transform('admin_received_by_customer')
    }
  ];

  public tableSettings: DynamicTableSettings | null = null;

  private getStatus(data: QrCodeModel): TableStatusItem {
    if(!data.isWinReceived) {
      return {
        id: 1,
        name: this.translationPipe.transform("admin_the_prize_has_not_been_given"),
        color: 'red',
        icon: '❌'
      }
    }

    if(data.isWinReceived && !data.hasCustomerConfirmed) {
      return {
        id: 2,
        name: this.translationPipe.transform("admin_the_prize_has_been_given_by_courier"),
        color: 'rgb(46, 187, 53)',
        icon: '✅'
      }
    }

    if(data.hasCustomerConfirmed) {
      return {
        id: 3,
        name: this.translationPipe.transform("admin_confirmeed_by_customer"),
        color: 'rgb(46, 187, 53)',
        icon: '🟢'
      }
    }

    return {
      id: 4,
      name: '---'
    } as TableStatusItem;
  }

  private regions: Array<RegionModel> = [];

  private filter = {};

  constructor(
    private translationPipe: TranslationPipe,
    private modalService: NgbModal,
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private http: HttpService,
    public toastr: ToastrService
  ) { }

  async ngOnInit() {
    await this.getRegions();
    await this.genereteTableObject();

    this.activatedRoute.queryParams.subscribe((params: Params) => {
      this.filter = {...params};
    })
  }

  private async genereteTableObject() {
    this.tableSettings = {
      source: Urls.QrCode,
      items: [
        {
          name: this.translationPipe.transform('admin_camaigns_action'),
          type: DynamicTableItemType.Button,
          customAction: {
            action: (row: QrCodeModel) => this.receivePrize(row),
            isDisabled: (row: QrCodeModel) => row.isWinReceived || row.prizeId == undefined || row.customerId == undefined,
            hide: (row: QrCodeModel) => row.prizeId === undefined,
            backgroundColor: (row: QrCodeModel) => {
              if(!row.isWinReceived) {
                return 'rgb(46, 187, 53)';
              }
            },
            title: (row: QrCodeModel) => {
              if(row.isWinReceived && row.customerId !== undefined) {
                return this.translationPipe.transform('admin_prize_is_received')
              }
              if(!row.isWinReceived || row.customerId === undefined) {
                return this.translationPipe.transform('admin_prize_is_not_received')
              }
            }
          }
        },
        {name: this.translationPipe.transform("admin_status"), status: (row) => this.getStatus(row), type: DynamicTableItemType.Status},
        {name: this.translationPipe.transform('admin_courier_first_name'), prop: 'user.firstName', type: DynamicTableItemType.Text},
        {name: this.translationPipe.transform('admin_courier_last_name'), prop: 'user.lastName', type: DynamicTableItemType.Text},
        {name: this.translationPipe.transform('admin_date_of_delivery'), prop: 'prizeDeliveryDate', type: DynamicTableItemType.Date},
        {name: this.translationPipe.transform('admin_date_of_receipt'), prop: 'prizeReceiveDate', type: DynamicTableItemType.Date},
        {name: this.translationPipe.transform('admin_prize_id'), prop: 'prizeId', type: DynamicTableItemType.Number},
        {name: this.translationPipe.transform('admin_customer_id'), prop: 'customerId', type: DynamicTableItemType.Number},
        {name: this.translationPipe.transform('admin_customer_first_name'), prop: 'customer.firstName', type: DynamicTableItemType.Text},
        {name: this.translationPipe.transform('admin_customer_last_name'), prop: 'customer.lastName', type: DynamicTableItemType.Text},
        {name: this.translationPipe.transform('admin_customer_phone_number_v2'), prop: 'customer.phoneNumber', type: DynamicTableItemType.Text},
        {name: this.translationPipe.transform('admin_createdDate'), prop: 'createdDate', type: DynamicTableItemType.Date},
        {name: this.translationPipe.transform('admin_is_qr_exported'), prop: 'isExported', type: DynamicTableItemType.Boolean},
        {name: this.translationPipe.transform("admin_region"), delegatedValue: (row: QrCodeModel) => this.regions.find(x => x.id === row.regionId).name, type: DynamicTableItemType.DelegatedValue},
        {name: this.translationPipe.transform('admin_qr_code'), prop: 'value', type: DynamicTableItemType.QrCode},
        {name: this.translationPipe.transform('admin_value'), prop: 'value', type: DynamicTableItemType.Text},
      ],
      actions: [ActionType.Edit]
    }

  }

  public openScanner() {
    let modalRef = this.modalService.open(ScannerComponent);
    modalRef.result.then((result: ModalResponse<string>) => {
      if(result.isSuccess) {
        this.setQrCodeValueAndNavigate(result.data);
        this.router.navigate([], {
          relativeTo: this.activatedRoute,
          queryParams: {value: result.data},
          queryParamsHandling: 'merge',
        });
      }
    },
    (reason: any) => {})
    .catch(error => {
      console.error('Modal error:', error);
    })
  }

  private async receivePrize(qrCode: QrCodeModel) {
    let modalRef = this.modalService.open(ConfirmActionDialogComponent);
    modalRef.result.then((result: ModalResponse<void>) => {
      if(result.isSuccess) {
        this.RecievePrizeApiCall(qrCode);
      }
    })
  }

  private async RecievePrizeApiCall(qrCode: QrCodeModel) {
    let result = await this.http.post<void>(Urls.QrCodeWinRecieve.replace('id', qrCode.id.toString())).toPromise();
    if(result && result.isSuccess) {
      this.toastr.success(this.translationPipe.transform("admin_success"));
      this.table.getData();
    }
  }

  private async getRegions() {
    let res = await this.http.get<Array<RegionModel>>(Urls.Region).toPromise();
    if(res && res.isSuccess) {
      this.regions = res.value;
    }
  }

  public openQrSCanDevice() {
    let modalRef = this.modalService.open(QrScannerDevicePopupComponent);
    modalRef.result.then((result:ModalResponse<string>) => {
      if(result.isSuccess) {
        this.setQrCodeValueAndNavigate(result.data);
      }
    })
  }

  public setQrCodeValueAndNavigate(qrCode: string) {
    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: {value: qrCode, skip: 1},
      queryParamsHandling: 'merge',
    });
  }

  public async exportToExcel() {
    let res = await this.http.downloadGetRequest(Urls.QrcodeWinExportExcel, this.filter).toPromise();
    if(res) {
      DownloadFileHelper.DownloadBlobFile(res, 'qr-codes-export');
    }
  }

}
