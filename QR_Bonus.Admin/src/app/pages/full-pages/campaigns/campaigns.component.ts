import { Component, OnInit, ViewChild } from '@angular/core';
import { DynamicTableSettings, DynamicTableItemType, ActionType, TableStatusItem, DynamicTableComponent } from 'app/shared/components/dynamic-table/dynamic-table.component';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { TranslationPipe } from '../../../shared/pipes/translation.pipe';
import { CampaingStatusEnum } from 'app/shared/enums/campaing-status.enum';
import { CampaignModel } from 'app/shared/models/campaign.model';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmActionDialogComponent } from 'app/shared/components/popups/confirm-action-dialog/confirm-action-dialog.component';
import { ModalResponse } from 'app/shared/models/modal-response.model';
import { ToastrService } from 'ngx-toastr';
import { DynamicFilterItem, DFControlType } from 'app/shared/components/dynamic-filters/dynamic-filters.component';

@Component({
  selector: 'app-campaigns',
  templateUrl: './campaigns.component.html',
  styleUrls: ['./campaigns.component.scss']
})
export class CampaignsComponent implements OnInit {

  @ViewChild('table') table: DynamicTableComponent;

  public statuses: Array<TableStatusItem> = [
    {
      id: CampaingStatusEnum.Active,
      name: this.translationPipe.transform("admin_cmapaign_active"),
      color: 'red'
    },
    {
      id: CampaingStatusEnum.Completed,
      name: this.translationPipe.transform("admin_cmapaign_completed"),
      color: 'grey'
    },
    {
      id: CampaingStatusEnum.Draft,
      name: this.translationPipe.transform("admin_cmapaign_draft"),
      color: 'rgb(46, 187, 53)'
    },
  ];

  public filters: Array<DynamicFilterItem> = [
    {
      propName: 'id',
      type: DFControlType.Number,
      label: 'Id',
      placeholder: 'Id'
    },
    {
      propName: 'startDate',
      type: DFControlType.DateTime,
      isDateFrom: true,
      label: this.translationPipe.transform('admin_start_date'),
      placeholder: this.translationPipe.transform('admin_start_date')
    },
    {
      propName: 'endDate',
      type: DFControlType.DateTime,
      isDateTo: true,
      label: this.translationPipe.transform('admin_end_date'),
      placeholder: this.translationPipe.transform('admin_end_date')
    },
    {
      propName: 'campaignStatusIds',
      type: DFControlType.Select,
      items: this.statuses,
      multiple: true,
      label: this.translationPipe.transform('admin_campaign_status'),
      placeholder: this.translationPipe.transform('admin_campaign_status')
    },
    {
      propName: 'campaignIds',
      type: DFControlType.Select,
      source: Urls.Campaign,
      multiple: true,
      label: this.translationPipe.transform('admin_campaigns'),
      placeholder: this.translationPipe.transform('admin_campaigns')
    },
    {
      propName: 'regionIds',
      type: DFControlType.Select,
      source: Urls.Region,
      multiple: true,
      label: this.translationPipe.transform('admin_regions'),
      placeholder: this.translationPipe.transform('admin_regions')
    }
  ];

  public tableSettings: DynamicTableSettings = {
    source: Urls.Campaign,
    deleteUrl: Urls.Campaign,
    items: [
      {
        name: this.translationPipe.transform("admin_camaigns_action"),
        type: DynamicTableItemType.Button,
        customAction: {
          action: (row: CampaignModel) => this.customAction(row),
          isDisabled: (row: CampaignModel) => row.status === CampaingStatusEnum.Completed,
          backgroundColor: (row: CampaignModel) => {
            if(row.status === CampaingStatusEnum.Active) {
              return 'rgb(225, 6, 19)';
            };
            if(row.status === CampaingStatusEnum.Draft) {
              return 'rgb(46, 187, 53)';
            }
          },
          title: (row: CampaignModel) => {
            if(row.status === CampaingStatusEnum.Draft) {
              return this.translationPipe.transform("admin_campaign_start");
            }
            if(row.status === CampaingStatusEnum.Active) {
              return this.translationPipe.transform("admin_campaign_end");
            }
            if(row.status === CampaingStatusEnum.Completed) {
              return this.translationPipe.transform("admin_cmapaign_completed");
            }
            return "---";
          }
        }
      },
      {name: this.translationPipe.transform("admin_status"), status: (row) => this.statuses.find((x: TableStatusItem) => x.id === row.status), type: DynamicTableItemType.Status},
      {name: this.translationPipe.transform("admin_name"), prop: 'name', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform("admin_region"), prop: 'region.name', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform("admin_campaign_startDate"), prop: 'startDate', type: DynamicTableItemType.Date},
      {name: this.translationPipe.transform("admin_campaign_end_date"), prop: 'endDate', type: DynamicTableItemType.Date},
    ],
    actions: [ActionType.Edit, ActionType.Delete],
  }

  private async customAction(row: CampaignModel) {
    if(row.status === CampaingStatusEnum.Completed) {
      return;
    }
    let modalRef = this.modalService.open(ConfirmActionDialogComponent);
    modalRef.componentInstance.message = row.status === CampaingStatusEnum.Draft ? this.translationPipe.transform("admin_start_compaign") : this.translationPipe.transform("admin_end_compaign");
    modalRef.result.then(async (result: ModalResponse<void>) => {
      if(result.isSuccess) {
        let url: string = row.status === CampaingStatusEnum.Draft ? Urls.CampaignStart : Urls.CampaignEnd;
        let res = await this.http.put(url, {}, row.id).toPromise();
        if(res && res.isSuccess) {
          this.toastr.success(this.translationPipe.transform("admin_success"));
          this.table.getData();
        }
      }
    },
    (reason: any) => {})
    .catch(error => {
      console.error('Modal error:', error);
    })
  }

  constructor(
    private translationPipe: TranslationPipe,
    private modalService: NgbModal,
    private http: HttpService,
    public toastr: ToastrService,
  ) { }

  ngOnInit(): void {}

}
