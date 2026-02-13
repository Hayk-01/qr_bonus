import { Component, OnInit } from '@angular/core';
import { DynamicFilterItem, DFControlType } from 'app/shared/components/dynamic-filters/dynamic-filters.component';
import { DynamicTableSettings, DynamicTableItemType, ActionType } from 'app/shared/components/dynamic-table/dynamic-table.component';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { Urls } from 'app/shared/services/http.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss']
})
export class ProductsComponent implements OnInit {

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
    }
  ];

  public tableSettings: DynamicTableSettings = {
    source: Urls.Product,
    deleteUrl: Urls.Product,
    items: [
      {name: this.translationPipe.transform('admin_name'), prop: 'name', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform("admin_region"), prop: 'region.name', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform('admin_createdDate'), prop: 'createdDate', type: DynamicTableItemType.Date},
      {name: this.translationPipe.transform('admin_modifyDate'), prop: 'modifyDate', type: DynamicTableItemType.Date},
      {name: this.translationPipe.transform('admin_product_image'), prop: 'link', type: DynamicTableItemType.Image},
    ],
    actions: [ActionType.Edit, ActionType.Delete]
  }

  constructor(
    private translationPipe: TranslationPipe
  ) { }

  ngOnInit(): void {}

}
