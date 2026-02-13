import { Component, OnInit } from '@angular/core';
import { DynamicFilterItem, DFControlType } from 'app/shared/components/dynamic-filters/dynamic-filters.component';
import { DynamicTableSettings, DynamicTableItemType, ActionType } from 'app/shared/components/dynamic-table/dynamic-table.component';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { Urls } from 'app/shared/services/http.service';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.scss']
})
export class CustomersComponent implements OnInit {

  public filters: Array<DynamicFilterItem> = [
    {
      propName: 'id',
      type: DFControlType.Number,
      label: 'Id',
      placeholder: 'Id'
    },
    {
      propName: 'firstName',
      type: DFControlType.Text,
      label: this.translationPipe.transform('admin_first_name'),
      placeholder: this.translationPipe.transform('admin_first_name')
    },
    {
      propName: 'lastName',
      type: DFControlType.Text,
      label: this.translationPipe.transform('admin_last_name'),
      placeholder: this.translationPipe.transform('admin_last_name')
    },
    {
      propName: 'phoneNumber',
      type: DFControlType.Text,
      label: this.translationPipe.transform('admin_customer_phone_number'),
      placeholder: this.translationPipe.transform('admin_customer_phone_number')
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
    source: Urls.Customer,
    deleteUrl: Urls.Customer,
    items: [
      {name: this.translationPipe.transform('admin_first_name'), prop: 'firstName', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform('admin_last_name'), prop: 'lastName', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform('admin_customer_phone_number'), prop: 'phoneNumber', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform('admin_customer_email'), prop: 'email', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform("admin_region"), prop: 'region.name', type: DynamicTableItemType.Text},
    ],
    actions: [ActionType.Edit, ActionType.Delete]
  }

  constructor(private translationPipe: TranslationPipe) { }

  ngOnInit(): void {}

}
