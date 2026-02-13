import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddProductPopupComponent } from './add-product-popup/add-product-popup.component';
import { ModalResponse } from 'app/shared/models/modal-response.model';
import { ProductPointModel } from 'app/shared/models/product-point.model';
import { ToastrService } from 'ngx-toastr';
import { TranslationPipe } from '../../../../shared/pipes/translation.pipe';
import { ActivatedRoute, Router } from '@angular/router';
import { PageType } from 'app/shared/enums/page-type.enum';
import { CampaignModel } from 'app/shared/models/campaign.model';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { AddQrComponent } from './add-qr/add-qr.component';
import { CampaingStatusEnum } from 'app/shared/enums/campaing-status.enum';
import { DynamicTableItemType, DynamicTableSettings } from 'app/shared/components/dynamic-table/dynamic-table.component';
import { LeaderBoardEditComponent } from './leader-board-edit/leader-board-edit.component';
import { LanguagesEnum } from 'app/shared/enums/languages.enum';
import { SelectItem } from 'app/shared/models/select-item.model';
import { AddQrCodeModel } from 'app/shared/models/add-qr-code.mode';
import { FormErrorMessageProviderService } from 'app/shared/services/form-error-message-provider.service';
import { AddOrUpdateCampaignPrizeModel } from 'app/shared/models/add-or-update-campaign-prize.model';
import { PositionPrizeModel } from 'app/shared/models/position-prize.model';
import { DownloadFileHelper } from 'app/shared/utility/file-download-helper';
import { PaginationConfig } from 'app/shared/components/pagination/pagination.config';
import { ConfirmActionDialogComponent } from 'app/shared/components/popups/confirm-action-dialog/confirm-action-dialog.component';
import { StorageService, StorageValue } from 'app/shared/services/storage.service';
import { DropDownDataModel } from 'app/shared/control-value-accessors/dropdown/dropdown.component';

@Component({
  selector: 'app-campaing-details',
  templateUrl: './campaing-details.component.html',
  styleUrls: ['./campaing-details.component.scss']
})
export class CampaingDetailsComponent implements OnInit, OnDestroy {
  public id: string = this.activatedRoute.snapshot.paramMap.get("id");

  public type = this.activatedRoute.snapshot.paramMap.get("type");

  public PageType = PageType;

  public data: CampaignModel = {} as CampaignModel;

  public touched:boolean = false;

  public active: number = 1;

  public CampaingStatusEnum = CampaingStatusEnum;

  public showQroCodes:boolean = false;

  public datesValidators = {
    fromDateMin: (new Date()).toISOString(),
    fromDateMax: null,
    toDateMin: (new Date()).toISOString(),
  }

  public regionDowpDown: DropDownDataModel = {
    source: Urls.Region
  }

  public form: FormGroup = new FormGroup({
    id: new FormControl(null),
    translations: new FormArray([
      new FormGroup({
        languageId: new FormControl(LanguagesEnum.English),
        name: new FormControl(null, [Validators.required])
      }),
      new FormGroup({
        languageId: new FormControl(LanguagesEnum.Georgian),
        name: new FormControl(null, [Validators.required])
      }),
    ]),
    startDate: new FormControl(null, [Validators.required]),
    endDate: new FormControl(null, [Validators.required]),
    productPoints: new FormArray([]),
    regionId: new FormControl(null, [Validators.required])
  })

  public qrCodeTableSettings: DynamicTableSettings = {} as DynamicTableSettings;

  public productCampaignIdAndProductName: Array<SelectItem> = []; //// combine array of product name and ProductCampaign-id

  public selectedProductCampaignId: number = 0;

  public isExported:boolean | null = false;

  public showLeaderBoard: boolean = true;

  private timeout: any;

  public leaderBoardPrizes: AddOrUpdateCampaignPrizeModel = {} as AddOrUpdateCampaignPrizeModel;

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

  constructor(
    public location: Location,
    private modalService: NgbModal,
    public toastr: ToastrService,
    private translationPipe: TranslationPipe,
    private activatedRoute: ActivatedRoute,
    private http: HttpService,
    private router: Router,
    public formErrorMessageProviderService: FormErrorMessageProviderService
  ) { }

  ngOnInit(): void {
    if(this.type === PageType.edit) {
      this.getData();
      this.getLeaderBoardPrizes();
    }

    this.form.valueChanges.subscribe(res => {
      if(res.endDate !== null) {
        this.datesValidators.fromDateMax = res.endDate;
      }
      if(res.startDate !== null) {
        this.datesValidators.toDateMin = res.startDate;
      }
    })
  }

  public get name() {
    if(this.data && this.data.translations && this.data.translations.length > 0) {
      return this.data.translations.find(x => x.languageId === StorageService.get(StorageValue.LanguageId)).name;
    }
  }

  private async getData() {
    let res = await this.http.get<CampaignModel>(Urls.Campaign, {}, parseInt(this.id)).toPromise();
    if(res && res.isSuccess) {
      this.data = res.value;
      this.form.patchValue(res.value);
      this.productPoints.setValue([]);
      res.value.productPoints.forEach(x => {
        this.productPoints.push(
          new FormGroup({
            productId: new FormControl(x.productId),
            product: new FormControl(x.product),
            point: new FormControl(x.point),
            productCampaignId: new FormControl(x.productCampaignId)
          })
        )

        this.productCampaignIdAndProductName.push({
          id: x.productCampaignId,
          name: x.product.name
        })

      });

      if(this.productCampaignIdAndProductName.length > 0) {
        this.selectedProductCampaignId = this.productCampaignIdAndProductName[0].id as number;
      }

      this.data.translations.forEach(item => {
        if(item.languageId === LanguagesEnum.English) {
          this.translations.controls[0].patchValue(item);
        }
        if(item.languageId === LanguagesEnum.Georgian) {
          this.translations.controls[1].patchValue(item);
        }
      })

      this.qrCodeTableSettings  = {
        source: Urls.QrCode,
        additionalFilters: {productCampaignId: this.selectedProductCampaignId, isExported: false},
        items: [
          {name: this.translationPipe.transform('admin_value'), prop: 'value', type: DynamicTableItemType.Text},
          {name: this.translationPipe.transform('admin_prize_id'), prop: 'prizeId', type: DynamicTableItemType.Number},
          {name: this.translationPipe.transform('admin_customer_id'), prop: 'customerId', type: DynamicTableItemType.Number},
          {name: this.translationPipe.transform('admin_is_qr_exported'), prop: 'isExported', type: DynamicTableItemType.Boolean},
          {name: this.translationPipe.transform('admin_qr_code'), prop: 'value', type: DynamicTableItemType.QrCode},
        ]
      }
      this.qrCodeTableSettings = {...this.qrCodeTableSettings};
      this.showQroCodes = true;
    }
  }

  public get translations() {
    return this.form.get("translations") as FormArray;
  }

  public get productPoints() {
    return this.form.get("productPoints") as FormArray;
  }

  public AddProductInCampaign() {
    let modalRef = this.modalService.open(AddProductPopupComponent, {size: 'lg'});
    modalRef.componentInstance.regionId = this.data.regionId;
    modalRef.result.then(async (result: ModalResponse<ProductPointModel>) => {
      if(result.isSuccess) {
        let dublicate = this.productPoints.value.find((x: ProductPointModel) => x.product.id == result.data.product.id);
        if(dublicate !== undefined) {
          this.toastr.info(this.translationPipe.transform("admin_product_already_is_in_the_list"));
          return;
        }
        this.productPoints.push(
          new FormGroup({
            productId: new FormControl(result.data.productId),
            product: new FormControl(result.data.product),
            point: new FormControl(result.data.point),
          })
        )
      }
    },
    (reason: any) => {})
    .catch(error => {
      console.error('Modal error:', error);
    })
  }

  public EditProductInCampaign(item: ProductPointModel, index: number) {
    let modalRef = this.modalService.open(AddProductPopupComponent, {size: 'lg'});
    modalRef.componentInstance.regionId = this.data.regionId;
    modalRef.componentInstance.data = {...item};
    modalRef.componentInstance.pageType = PageType.edit;
    modalRef.result.then(async (result: ModalResponse<ProductPointModel>) => {
      if(result.isSuccess) {
        this.productPoints.at(index).patchValue(result.data);
      }
    },
    (reason: any) => {})
    .catch(error => {
      console.error('Modal error:', error);
    });
  }

  public remove(index: number) {
    let modal = this.modalService.open(ConfirmActionDialogComponent);
    modal.result.then((res: ModalResponse<void>) => {
      if(res.isSuccess) {
        this.productPoints.removeAt(index);
      }
    },
    (reason: any) => {})
    .catch(error => {
      console.error('Modal error:', error);
    })
  }

  public AddQr(item: ProductPointModel) {
    let modalRef = this.modalService.open(AddQrComponent, {size: 'lg'});
    modalRef.componentInstance.regionId = this.data.regionId;
    modalRef.componentInstance.data = item;
    modalRef.componentInstance.campaignName = this.data.name;
    modalRef.result.then((res: ModalResponse<AddQrCodeModel>) => {
      if(res.isSuccess) {
        this.generateQr(res.data);
      }
    },
    (reason: any) => {})
    .catch(error => {
      console.error('Modal error:', error);
    })
  }

  private async generateQr(reqBody: AddQrCodeModel) {
    let res = await this.http.post(Urls.QrCode, reqBody).toPromise();
    if(res && res.isSuccess) {
      this.toastr.success(this.translationPipe.transform("admin_success"));
    }
  }

  private async getLeaderBoardPrizes() {
    let res = await this.http.get<Array<PositionPrizeModel>>(Urls.CampaignLeaderBoardPrizesById.replace('campaignId', this.id)).toPromise();
    if(res && res.isSuccess) {
      this.leaderBoardPrizes = {campaignId: parseInt(this.id), prizesAsc: []};
      res.value.forEach(element => {
        this.leaderBoardPrizes.prizesAsc.push(element.prize.id);
      });
    }
  }

  public openLeaderBoardEditPopup() {
    let modalRef = this.modalService.open(LeaderBoardEditComponent, {size: 'lg'});
    modalRef.componentInstance.regionId = this.data.regionId;
    if(this.leaderBoardPrizes.prizesAsc && this.leaderBoardPrizes.prizesAsc.length > 0) {
      modalRef.componentInstance.data.prizesAsc = this.leaderBoardPrizes.prizesAsc.map(x => x);
    }
    modalRef.componentInstance.data.campaignId = this.leaderBoardPrizes.campaignId;
    modalRef.result.then((res) => {
      if(res.isSuccess) {
        this.getData();
        this.getLeaderBoardPrizes();
        this.showLeaderBoard = false;
        this.timeout = setTimeout(() => {
          this.showLeaderBoard = true;
        }, 0)
      }
    },
    (reason: any) => {})
    .catch(error => {
      console.error('Modal error:', error);
    })
  }

  public async onSubmit() {
    this.touched = true;
    if(this.form.invalid) {
      return;
    }

    let reqBody: CampaignModel = this.form.value;

    let res: any;
    if(this.type === PageType.new) {
      res = await this.http.post(Urls.Campaign, reqBody).toPromise();
    } else {
      res = await this.http.put(Urls.Campaign, reqBody, reqBody.id).toPromise();
    }

    if(res && res.isSuccess) {
      this.toastr.success(this.translationPipe.transform("admin_success"));
      this.location.back();
    }
  }

  public setQuerriesAndNavigate() {
    this.qrCodeTableSettings.additionalFilters = {productCampaignId: this.selectedProductCampaignId}
    this.qrCodeTableSettings = {...this.qrCodeTableSettings};
    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: {productCampaignId: this.selectedProductCampaignId, isExported: this.isExported, skip: PaginationConfig.skip, take: PaginationConfig.take},
      queryParamsHandling: 'merge',
    });
  }

  public async downloadQrCodes() {
    let res = await this.http.downloadGetRequest(Urls.QrCodeExportCsv, {productCampaignId: this.selectedProductCampaignId, isExported: this.isExported}).toPromise();
    if(res) {
      DownloadFileHelper.DownloadCSVFile(res, 'qr-codes');
    }
  }

  ngOnDestroy(): void {
    clearTimeout(this.timeout);
  }

}
