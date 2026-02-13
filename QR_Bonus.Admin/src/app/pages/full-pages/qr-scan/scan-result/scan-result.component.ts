import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmActionDialogComponent } from 'app/shared/components/popups/confirm-action-dialog/confirm-action-dialog.component';
import { ModalResponse } from 'app/shared/models/modal-response.model';
import { QrCodeModel } from 'app/shared/models/qr-code.model';
import { RegionModel } from 'app/shared/models/region.model';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { StorageService, StorageValue } from 'app/shared/services/storage.service';
import { ToastrService } from 'ngx-toastr';
import { Location } from '@angular/common';

@Component({
  selector: 'app-scan-result',
  templateUrl: './scan-result.component.html',
  styleUrls: ['./scan-result.component.scss']
})
export class ScanResultComponent implements OnInit {

  public data: QrCodeModel | null = null;

  public languageId: number = StorageService.get(StorageValue.LanguageId);

  public loading:boolean = false;

  public qrCodeNotUsedYet:boolean = false;

  constructor(
    private activatedRoute: ActivatedRoute,
    private httpService: HttpService,
    private modalService: NgbModal,
    public toastr: ToastrService,
    private translationPipe: TranslationPipe,
    public location: Location,
  ) { }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe((params: Params) => {
      if(params.hasOwnProperty('value')) {
        this.getData(params.value);
      }
    })
  }

  private async getData(value: string) {
    this.loading = true;

    try {
      let res = await this.httpService.get<QrCodeModel>(Urls.QrCode, {value}, null, true).toPromise();
      if(res && res.isSuccess) {
        this.data = res.value[0];
        if(this.data.customer == undefined || this.data.customer == null || this.data.prizeId == undefined || this.data.prizeId == null) {
          this.qrCodeNotUsedYet = true;
          this.data = null;
          this.loading = false;
        }
      } else {
        this.loading = false;
      }
    } catch (error) {
      this.loading = false;
    }

    this.loading = false;
  }

  public get regionName() {
    if(!this.data.customer.region || !this.data.customer.region.translations) return "---";
    return this.data.customer.region.translations.find(x => x.languageId === this.languageId).name;
  }

  public async receivePrize() {
    if(this.data.isWinReceived) return;
    let modalRef = this.modalService.open(ConfirmActionDialogComponent);
    modalRef.result.then((result: ModalResponse<void>) => {
      if(result.isSuccess) {
        this.RecievePrizeApiCall();
      }
    })
  }

  private async RecievePrizeApiCall() {
    let result = await this.httpService.post<void>(Urls.QrCodeWinRecieve.replace('id', this.data.id.toString())).toPromise();
    if(result && result.isSuccess) {
      this.toastr.success(this.translationPipe.transform("admin_success"));
      this.data.isWinReceived = true;
    }
  }

}
