import { Component, OnInit } from '@angular/core';
import { DynamicFilterItem, DFControlType } from 'app/shared/components/dynamic-filters/dynamic-filters.component';
import { DynamicTableSettings, DynamicTableItemType, ActionType } from 'app/shared/components/dynamic-table/dynamic-table.component';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { DownloadFileHelper } from 'app/shared/utility/file-download-helper';
import { ActivatedRoute, Params } from '@angular/router';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {

  private filter = {};

  public filters: Array<DynamicFilterItem> = [
    {
      propName: 'prizeIds',
      type: DFControlType.Select,
      multiple: true,
      source: Urls.Prize,
      label: this.translationPipe.transform('admin_prize_name'),
      placeholder: this.translationPipe.transform('admin_prize_name'),
      displayId: true
    },
    {
      propName: 'fromWinDate',
      type: DFControlType.DateTime,
      isDateFrom: true,
      label: this.translationPipe.transform('admin_win_date_from'),
      placeholder: this.translationPipe.transform('admin_win_date_from')
    },
    {
      propName: 'toWinDate',
      type: DFControlType.DateTime,
      isDateTo: true,
      label: this.translationPipe.transform('admin_win_date_to'),
      placeholder: this.translationPipe.transform('admin_win_date_to')
    },
    {
      propName: 'fromCreatedDate',
      type: DFControlType.DateTime,
      isDateFrom: true,
      label: this.translationPipe.transform('admin_create_date_from'),
      placeholder: this.translationPipe.transform('admin_create_date_from')
    },
    {
      propName: 'toCreatedDate',
      type: DFControlType.DateTime,
      isDateTo: true,
      label: this.translationPipe.transform('admin_create_date_to'),
      placeholder: this.translationPipe.transform('admin_create_date_to')
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
    source: Urls.ReportPrize,
    items: [
      {name: this.translationPipe.transform('admin_prize_id'), prop: 'prizeId', type: DynamicTableItemType.Number},
      {name: this.translationPipe.transform('admin_prize_name'), prop: 'prizeName', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform('admin_total_created'), prop: 'qrTotalQuantity', type: DynamicTableItemType.Number},
      {name: this.translationPipe.transform('admin_total_won'), prop: 'winTotalQuantity', type: DynamicTableItemType.Number},
      {name: this.translationPipe.transform('admin_total_claimed'), prop: 'claimedPrizeQuantity', type: DynamicTableItemType.Number},
      {name: this.translationPipe.transform('admin_total_unclaimed'), prop: 'unclaimedPrizeQuantity', type: DynamicTableItemType.Number},
    ],
    hideId: true
  }

  constructor(
    private translationPipe: TranslationPipe,
    private activatedRoute: ActivatedRoute,
    private http: HttpService
  ) { }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe((params: Params) => {
      this.filter = {...params};
    })
  }

  public async exportReport() {
    let res = await this.http.downloadGetRequest(Urls.ReportPrizeExport, this.filter).toPromise();
    if(res) {
      DownloadFileHelper.DownloadBlobFile(res, 'report');
    }
  }

}
