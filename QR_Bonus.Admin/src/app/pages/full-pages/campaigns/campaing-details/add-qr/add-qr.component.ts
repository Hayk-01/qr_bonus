import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DropDownDataModel } from 'app/shared/control-value-accessors/dropdown/dropdown.component';
import { PrizeModel } from 'app/shared/models/prize.model';
import { ProductPointModel } from 'app/shared/models/product-point.model';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { Urls } from 'app/shared/services/http.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-qr',
  templateUrl: './add-qr.component.html',
  styleUrls: ['./add-qr.component.scss']
})
export class AddQrComponent implements OnInit {
  public data: ProductPointModel;

  public campaignName: string | null = null;

  public selectedValue: {count: number, prize: PrizeModel} = {
    count: null,
    prize: null
  };

  public totalCount:number | null = null;

  public bonusCount: number = 0;

  public prizeCount: number = 0;

  public regionId: number = null;

  public prizesDropdownData: DropDownDataModel = {} as DropDownDataModel;

  public touched: boolean = false;

  public form: FormGroup;

  public message: string = '';

  public now = (new Date()).toISOString();

  constructor(
    public activeModal: NgbActiveModal,
    public toastr: ToastrService,
    private translationPipe: TranslationPipe,
  ) { }

  ngOnInit(): void {
    this.prizesDropdownData = {
      source: Urls.Prize,
      additionalFilters: {isLeaderboard : false, regionIds: [this.regionId]}
    }

    this.form = new FormGroup({
      productCampaignId: new FormControl(this.data.productCampaignId, [Validators.required]),
      qrCodeCounts: new FormArray([]),
      activationDate: new FormControl(null, [Validators.required])
    })
  }

  public get qrCodeCounts() {
    return this.form.get("qrCodeCounts") as FormArray;
  }

  public totalCountInput() {
    if(!this.totalCount) {
      this.toastr.error(this.translationPipe.transform('admin_please_enter_total_count'));
      this.message = '';
      return;
    }

    if(this.totalCount < this.prizeCount) {
      this.toastr.error(this.translationPipe.transform('admin_totoal_count_cant_be_less_then_prize_count'));
      this.message = '';
      return;
    }
    this.calculateCountsAndGenerateMessage();
  }

  public addValue() {
    if(this.bonusCount - parseInt(this.selectedValue.count.toString()) < 0) {
      this.toastr.error(this.translationPipe.transform('admin_add_qr_codes_error_message'));
      this.selectedValue = {
        count: null,
        prize: null
      };
      return;
    }

    if(this.selectedValue.prize && this.selectedValue.count) {
      let prizeDublicate = this.qrCodeCounts.value.find(x => x.prizeId === this.selectedValue.prize.id);
      if(prizeDublicate !== undefined) {
        this.qrCodeCounts.controls.forEach(x => {
          if(x.value.prizeId === this.selectedValue.prize.id) {
            x.value.count = x.value.count*1 + this.selectedValue.count*1;
            x.patchValue(x.value);
          }
        })
      } else {
        this.qrCodeCounts.push(
          new FormGroup({
            prizeId: new FormControl(this.selectedValue.prize ? this.selectedValue.prize.id: null),
            prize: new FormControl(this.selectedValue.prize ?? null),
            count: new FormControl(this.selectedValue.count),
          })
        )
      }
    }

    this.selectedValue = {
      count: null,
      prize: null
    };

    this.calculateCountsAndGenerateMessage();

  }

  public remove(index: number) {
    this.qrCodeCounts.removeAt(index);
    this.calculateCountsAndGenerateMessage();
  }

  public generateQrCodes() {
    this.touched = true;
    if(this.form.invalid) return;

    if (Date.now() > new Date(this.form.value.activationDate).getTime()) {
      this.toastr.error(this.translationPipe.transform('admin_activation_date_and_time_cant_be_more_than_current_date_and_time'));
      return
    }

    let reqBody = {...this.form.value};

    if(this.bonusCount > 0) {
      reqBody.qrCodeCounts.push({
        count: this.bonusCount,
        prize: null,
        prizeId: null
      })
    }

    this.activeModal.close({isSuccess: true, data: reqBody});
  }

  public calculateCountsAndGenerateMessage() {
    this.prizeCount = 0;
    this.bonusCount = 0;
    this.qrCodeCounts.value.forEach(item => {
      this.prizeCount = this.prizeCount + parseInt(item.count);
    })
    this.bonusCount = this.totalCount - this.prizeCount;

    this.message = this.translationPipe.transform('admin_there_are_totalCount_in_total_prizeCount_with_a_prize_and_bounsCount_with_a_bonus');
    this.message = this.message.replace('totalCount', this.totalCount.toString());
    this.message = this.message.replace('prizeCount', this.prizeCount.toString());
    this.message = this.message.replace('bounsCount', this.bonusCount.toString());
  }

  public get disableGenerateButton():boolean {
    if(!this.totalCount) {
      return true;
    }

    if(this.form.value.qrCodeCounts.length === 0 && !this.totalCount) {
      return true;
    }

    if(this.totalCount < this.prizeCount) {
      return true;
    }

    if(this.form.get('activationDate').invalid) {
      return true;
    }

    return false;
  }

}
