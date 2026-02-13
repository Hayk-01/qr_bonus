import { AfterViewInit, ChangeDetectorRef, Component, Input, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { FormGroup, FormControl, FormControlName } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { SelectItem } from 'app/shared/models/select-item.model';
import { DynamicFiltersNumberComponent } from './controls/dynamic-filters-number/dynamic-filters-number.component';
import { DynamicFiltersSelectComponent } from './controls/dynamic-filters-select/dynamic-filters-select.component';
import { DynamicFiltersSwitchToggleComponent } from './controls/dynamic-filters-switch-toggle/dynamic-filters-switch-toggle.component';
import { DynamicFiltersTextComponent } from './controls/dynamic-filters-text/dynamic-filters-text.component';
import { DynamicFiltersDateTimeComponent } from './controls/dynamic-filters-date-time/dynamic-filters-date-time.component';

@Component({
  selector: 'app-dynamic-filters',
  templateUrl: './dynamic-filters.component.html',
  styleUrls: ['./dynamic-filters.component.scss']
})
export class DynamicFiltersComponent implements OnInit, AfterViewInit {
  @ViewChild("container", { read: ViewContainerRef }) container: ViewContainerRef;

  private filterItems: Array<DynamicFilterItem> = [];
  @Input() set filters(items: Array<DynamicFilterItem>) {
    if(items.length > 0) {
      this.filterItems = items;
      this.buildForm(this.filterItems);
    }
  }

  public showPagination: boolean = true;
  @Input() set hidePagination(value: boolean) {
    this.showPagination = !value;
    this.cdr.detectChanges();
  }

  public showFilterSettings: boolean = false;
  @Input() set setShowFilterSettings(value:boolean) {
    this.showFilterSettings = value;
  }

  public filterToggle: boolean = false;

  public ready: boolean = false;

  public form: FormGroup = null;

  private destroyed$: Subject<boolean> = new Subject<boolean>();

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private cdr: ChangeDetectorRef,
  ) { }

  ngAfterViewInit(): void {
    this.generateControls(this.filterItems);
    this.setDefaultValues();
    this.cdr.detectChanges();
  }

  ngOnInit(): void {
    //// if you don't want to filters apply automaticly, just comment this part and uncomment apply button in html
    this.listenToFormValueChanges();

    this.activatedRoute.queryParams.subscribe(queryParams => {
      if(queryParams) {
        let formData = {...queryParams};
        for(let key in formData) {
          if(Array.isArray(formData[key])) {
            formData[key] = formData[key].map(el => parseInt(el));
          }
          if(!isNaN(+formData[key]) && !formData[key].toString().includes(".")) { //// !formData[key].toString().includes(".") for not converting doub;les into integer
            formData[key] = parseInt(formData[key]);
          }
          if(!isNaN(+formData[key]) && formData[key].toString().includes(".")) { //// converting into double
            formData[key] = parseFloat(formData[key]);
          }
          if(formData[key] === 'true') {
            formData[key] = true
          }
          if(formData[key] === 'false') {
            formData[key] = false
          }
          //// այստեղ գնտում եմ բոլոր multiple դաշտերը, և եթե մեկ հատ առժեք կա ընտրված, դարձնում եմ զանգված, from ի մեջ patch անելու համար
          let res = this.filterItems.find(x => key === x.propName);
          if(res !== undefined && res.multiple) {
            if(!Array.isArray(formData[key])) {
              formData[key] = [formData[key]];
            }
          }
        }

        this.form.patchValue(formData, {onlySelf: true, emitEvent: false});

      }

      //// this code works when clicking in the same route in menu
      if(Object.keys(queryParams).length === 0) {
        for(let key in this.form.value) {
          let prop = key;
          this.form.patchValue({[prop]: null}, {onlySelf: true, emitEvent: false});
        }
      }

    })

  }

  private listenToFormValueChanges() {
    this.form.valueChanges
    .pipe(debounceTime(800))
    .subscribe(_ => {
      this.applyFilter();
    })
  }

  public applyFilter() {
    let filters = this.form.value;
    for(let key in filters) {
      let value = filters[key];
      if(value === null || value.toString().trim() === "" || value === undefined || (Array.isArray(value) && value.length === 0)) {
        filters[key] = null;
      }
    }
    this.setQueriesNavigate(filters);
  }

  generateControls(filterItems: Array<DynamicFilterItem>) {
    if(filterItems.length === 0 && this.form === null) return;
    for(let field of filterItems) {
      let control: any;
      switch (field.type) {
        case DFControlType.Text:
          control = this.container.createComponent(DynamicFiltersTextComponent);
          break;
        case DFControlType.Number:
          control = this.container.createComponent(DynamicFiltersNumberComponent);
          break;
        case DFControlType.Select:
          control = this.container.createComponent(DynamicFiltersSelectComponent);
          if(field.filter) {
            control.instance.filter = field.filter;
          }
          if(field.displayName) {
            control.instance.displayName = field.displayName;
          }
          if(field.valueProp) {
            control.instance.valueProp = field.valueProp;
          }
          if(field.searchProp) {
            control.instance.searchProp = field.searchProp;
          }
          if(field.displayId) {
            control.instance.displayId = field.displayId;
          }
          if(field.listenTo) {
            control.instance.listenTo = field.listenTo;
          }
          break;
        case DFControlType.DateTime:
          control = this.container.createComponent(DynamicFiltersDateTimeComponent);
          if(field.isDateFrom) {
            control.instance.isDateFrom = field.isDateFrom;
          }
          if(field.isDateTo) {
            control.instance.isDateTo = field.isDateTo;
          }
          if(field.showTime) {
            control.instance.showTime = field.showTime;
          }
          break;
        case DFControlType.SwitchToggle:
          control = this.container.createComponent(DynamicFiltersSwitchToggleComponent);
          if(field.returnNullWhenValueIsFalse) {
            control.instance.returnNullWhenValueIsFalse = field.returnNullWhenValueIsFalse;
          }
          break;
        default:
          console.log("what are you want from me?");
      }
      control.instance.label = field.label;
      control.instance.form = this.form;
      control.instance.formControlName = field.propName;
      control.instance.initialValue = this.form.value[field.propName];
      if(field.placeholder) {
        control.instance.placeholder = field.placeholder;
      }
      if(field.source) {
        control.instance.source = field.source;
      }
      if(field.multiple) {
        control.instance.multiple = field.multiple;
      }
      if(field.items) {
        control.instance.items = field.items;
      }
    }
    this.ready = true;
  }

  buildForm(filterItems: Array<DynamicFilterItem>) {
    const formGroupFields = this.getFormControlsFields(filterItems);
    this.form = new FormGroup(formGroupFields);
  }

  getFormControlsFields(filterItems: Array<DynamicFilterItem>) {
    const formGroupFields = {};
    for(let field of filterItems) {
      formGroupFields[field.propName] = new FormControl(null);
    }
    return formGroupFields;
  }

  private setDefaultValues() {
    let defaultValue: any = {};
    for(let key in this.filterItems) {
      if(this.filterItems[key].defaultValue !== undefined) {
        defaultValue[this.filterItems[key].propName] = this.filterItems[key].defaultValue;
      }
    }
    this.form.patchValue(defaultValue)
  }

  private setQueriesNavigate(filters: any) {
    //// ստուգում եմ, եթե ֆիլտերով որևէ դաշտ կա ընտրված․ ապա skip պետք է լինի 1
    for(let key in filters) {
      if(filters[key] !== null) {
        filters.skip = 1;
      }
    }

    this.router.navigate([], {
      relativeTo: this.activatedRoute,
      queryParams: filters,
      queryParamsHandling: 'merge',
    });
  }

  public resetFilter() {
    this.form.reset({}, { emitEvent: true });
  }

  ngOnDestroy(): void {
    this.destroyed$.next(true);
    this.destroyed$.unsubscribe();
  }

}

export interface DynamicFilterItem {
  propName: string;
  type: DFControlType;
  label: string;
  items?: Array<SelectItem>;
  source?: string;
  filter?: any
  multiple?: boolean;
  placeholder?: string;
  isEnabled?: boolean;
  displayName?: Array<string>;
  valueProp?: string; // api request property
  searchProp?: string; // filter request property
  defaultValue?: any;
  isDateFrom?: boolean;
  isDateTo?:boolean;
  showTime?:boolean;
  returnNullWhenValueIsFalse?:boolean;
  displayId?: boolean;
  listenTo?: {formControlName: string; filterProperty: string};
}

export enum DFControlType {
  Text = "text",
  Number = "number",
  Select = "select",
  DateTime = "date-time",
  SwitchToggle = "switch-toggle",
}
