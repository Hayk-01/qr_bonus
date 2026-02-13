import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DropDownDataModel } from 'app/shared/control-value-accessors/dropdown/dropdown.component';
import { CampaingStatusEnum } from 'app/shared/enums/campaing-status.enum';
import { AddOrUpdateCampaignPrizeModel } from 'app/shared/models/add-or-update-campaign-prize.model';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-leader-board-edit',
  templateUrl: './leader-board-edit.component.html',
  styleUrls: ['./leader-board-edit.component.scss']
})
export class LeaderBoardEditComponent implements OnInit {

  public campaignStatus:number = CampaingStatusEnum.Draft;

  public CampaingStatusEnum = CampaingStatusEnum;

  public regionId: number = null;

  public data: AddOrUpdateCampaignPrizeModel = {
    campaignId: null,
    prizesAsc: []
  }

  public camaignPrizesInitial: Array<number> = [];

  public prizeDropdownData: DropDownDataModel = {} as DropDownDataModel

  public selectedPrizeId: number = null;

  constructor(
    public activeModal: NgbActiveModal,
    private http: HttpService,
    public toastr: ToastrService,
    private translationPipe: TranslationPipe,
  ) { }

  ngOnInit(): void {

    this.prizeDropdownData = {
      source: Urls.Prize,
      additionalFilters: {isLeaderboard : true, regionIds: [this.regionId]}
    }

    if(this.data && this.data.prizesAsc.length > 0) {
      this.camaignPrizesInitial = this.data.prizesAsc.map(x => x);
    }
  }

  public AddItem() {
    this.data.prizesAsc.push(this.selectedPrizeId);
    this.selectedPrizeId = null;
  }

  public RemoveItem(index: number) {
    this.data.prizesAsc.splice(index, 1);
  }

  public async save() {
    let res = await this.http.post(Urls.CampaignLeaderBoardPrizes, this.data).toPromise();
    if(res && res.isSuccess) {
      this.toastr.success(this.translationPipe.transform("admin_success"));
      this.activeModal.close({isSuccess: true});
      return;
    }
    this.activeModal.close({isSuccess: false});
  }

  public isEditable(prizeId: number) {
    if(this.campaignStatus == CampaingStatusEnum.Draft) {
      return true;
    }
    return this.camaignPrizesInitial.find(id => id === prizeId) === undefined;
  }

}
