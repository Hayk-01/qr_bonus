import { Component, OnInit } from '@angular/core';
import { DFControlType, DynamicFilterItem } from 'app/shared/components/dynamic-filters/dynamic-filters.component';
import { ActionType, DynamicTableItemType, DynamicTableSettings } from 'app/shared/components/dynamic-table/dynamic-table.component';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { Urls } from 'app/shared/services/http.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

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
      propName: 'userName',
      type: DFControlType.Text,
      label: this.translationPipe.transform('admin_user_name'),
      placeholder: this.translationPipe.transform('admin_user_name')
    }
  ];

  public tableSettings: DynamicTableSettings = {
    source: Urls.User,
    deleteUrl: Urls.User,
    items: [
      {name: this.translationPipe.transform('admin_first_name'), prop: 'firstName', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform('admin_last_name'), prop: 'lastName', type: DynamicTableItemType.Text},
      {name: this.translationPipe.transform('admin_user_name'), prop: 'userName', type: DynamicTableItemType.Text},
    ],
    actions: [ActionType.Edit, ActionType.Delete]
  }

  constructor(private translationPipe: TranslationPipe) { }

  ngOnInit(): void {}

}
