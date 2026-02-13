import { Component, OnInit } from '@angular/core';
import { DynamicFilterItem, DFControlType } from 'app/shared/components/dynamic-filters/dynamic-filters.component';
import { DynamicTableSettings, DynamicTableItemType, ActionType } from 'app/shared/components/dynamic-table/dynamic-table.component';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { Urls } from 'app/shared/services/http.service';

@Component({
  selector: 'app-gifts',
  templateUrl: './gifts.component.html',
  styleUrls: ['./gifts.component.scss']
})
export class GiftsComponent implements OnInit {

  public filters: Array<DynamicFilterItem> = [
    {
      propName: 'id',
      type: DFControlType.Number,
      label: 'Id',
      placeholder: 'Id'
    },
    {
      propName: 'name',
      type: DFControlType.Text,
      label: this.translationPipe.transform('admin_name'),
      placeholder: this.translationPipe.transform('admin_name')
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
      propName: 'isLeaderboard',
      type: DFControlType.SwitchToggle,
      returnNullWhenValueIsFalse: true,
      label: this.translationPipe.transform('admin_is_leaderboard_winnig'),
      placeholder: this.translationPipe.transform('admin_is_leaderboard_winnig')
    },
  ];

  public tableSettings: DynamicTableSettings = {
    source: Urls.Prize,
    deleteUrl: Urls.Prize,
    items: [
      {name: this.translationPipe.transform('admin_name'), prop: 'name', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform("admin_region"), prop: 'region.name', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform('admin_is_leaderboard_winnig'), prop: 'isLeaderboard', type: DynamicTableItemType.Boolean},
      {name: this.translationPipe.transform('admin_prize_image'), prop: 'link', type: DynamicTableItemType.Image},
    ],
    actions: [ActionType.Edit, ActionType.Delete]
  }

  constructor(
    private translationPipe: TranslationPipe
  ) { }

  ngOnInit(): void {}

}
