import { Component, OnInit } from '@angular/core';
import { DynamicFilterItem, DFControlType } from 'app/shared/components/dynamic-filters/dynamic-filters.component';
import { DynamicTableSettings, DynamicTableItemType, ActionType } from 'app/shared/components/dynamic-table/dynamic-table.component';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { Urls } from 'app/shared/services/http.service';

@Component({
  selector: 'app-regions',
  templateUrl: './regions.component.html',
  styleUrls: ['./regions.component.scss']
})
export class RegionsComponent implements OnInit {

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
      label: this.translationPipe.transform('admin_region_name'),
      placeholder: this.translationPipe.transform('admin_region_name')
    }
  ];

  public tableSettings: DynamicTableSettings = {
    source: Urls.Region,
    deleteUrl: Urls.Region,
    items: [
      {name: this.translationPipe.transform('admin_region_name'), prop: 'name', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform('admin_phone_code'), prop: 'areaCode', type: DynamicTableItemType.Text},
    ],
    actions: [ActionType.Edit]
  }

  constructor(
    private translationPipe: TranslationPipe
  ) { }

  ngOnInit(): void {}

}
