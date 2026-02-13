import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaginationConfig } from '../pagination/pagination.config';
import { HttpService } from 'app/shared/services/http.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmActionDialogComponent } from '../popups/confirm-action-dialog/confirm-action-dialog.component';
import { ModalResponse } from 'app/shared/models/modal-response.model';
import { EventsEmitterService } from 'app/shared/services/events-emitter.service';
import { TranslationPipe } from '../../pipes/translation.pipe';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dynamic-table',
  templateUrl: './dynamic-table.component.html',
  styleUrls: ['./dynamic-table.component.scss']
})
export class DynamicTableComponent implements OnInit {
  public DynamicTableItemType = DynamicTableItemType;

  public ActionType = ActionType;

  private filter: any = {};

  public rows: Array<any> | null = null;

  public dynamicTableSettings: DynamicTableSettings = {} as DynamicTableSettings;

  @Input() set settings(value: DynamicTableSettings) {
    this.dynamicTableSettings = value;
  }

  public loading:boolean = true;

  public nodata:boolean = false;

  constructor(
    private activatedRouter: ActivatedRoute,
    private httpService: HttpService,
    private modalService: NgbModal,
    private eventsEmitterService: EventsEmitterService,
    private translationPipe: TranslationPipe,
    public toastr: ToastrService,
  ) { }

  ngOnInit(): void {
    this.activatedRouter.queryParams.subscribe(queryParams => {
      const queries = {...queryParams}

      if (queries && !queries.hasOwnProperty('skip')) {
        queries.skip = PaginationConfig.skip;
      }
      if (queries && !queries.hasOwnProperty('take')) {
        queries.take = PaginationConfig.take;
      }

      this.filter = {...queries};

      if(this.dynamicTableSettings.source) {
        this.getData();
      }

    })
  }

  public async getData() {
    this.loading = true;
    this.nodata = false;

    if(!this.filter.hasOwnProperty('skip') && !this.filter.hasOwnProperty('take')) {
      this.filter.skip = PaginationConfig.skip;
      this.filter.take = PaginationConfig.take;
    }

    if(this.dynamicTableSettings.additionalFilters) {
      let additionalFilters = this.dynamicTableSettings.additionalFilters;
      this.filter = {...this.filter, ...additionalFilters};
    }

    try {
      let res = await this.httpService.get<any>(this.dynamicTableSettings.source, this.filter, null, true).toPromise();
      if(res && res.isSuccess) {
        this.rows = res.value;
        this.eventsEmitterService.paginationDataEmiter.emit(res.pagedInfo);
        this.nodata = this.rows.length === 0;
      }
      this.loading = false;
    } catch (error) {
      this.loading = false;
      this.nodata = true;
    }

  }

  public onDelete(id: number) {
    let modalRef = this.modalService.open(ConfirmActionDialogComponent);
    modalRef.componentInstance.message = `${this.translationPipe.transform("admin_do_you_want_to_delete_element_with_id")} ${id} ?`
    modalRef.result.then(async (result: ModalResponse<any>) => {
      if(result.isSuccess) {
        let result = await this.httpService.delete<any>(this.dynamicTableSettings.deleteUrl, id).toPromise();
        if(result && result.isSuccess) {
          this.toastr.success(this.translationPipe.transform("admin_the_action_was_successful"));
          this.getData();
        }
      }
    },
    (reason: any) => {})
    .catch(error => {
      console.error('Modal error:', error);
    })
  }

}

export interface DynamicTableSettings {
  source: string;
  deleteUrl?: string;
  additionalFilters?: any
  items: Array<DynamicTableItem>;
  actions?: Array<ActionType>;
  hideId?:boolean;
}

export interface DynamicTableItem {
  name: string,
  prop?: string,
  type?: any,
  width?: number;
  customAction?: {
    action?: Function,
    title?: Function,
    isDisabled?: Function,
    backgroundColor?: Function,
    hide?: Function
  },
  status?: Function,
  delegatedValue?: Function
}

export enum ActionType {
  Edit = 'edit',
  Delete = 'delete',
}

export enum DynamicTableItemType {
  Text = 'text',
  Number = 'number',
  Boolean = 'boolean',
  Date = 'date',
  Image = 'image',
  Button = 'button',
  Status = 'status',
  QrCode = 'qrcode',
  DelegatedValue = 'delegatedValue'
}

export interface TableStatusItem {
  id: number;
  name: string;
  color?: string;
  icon?: any;
}
