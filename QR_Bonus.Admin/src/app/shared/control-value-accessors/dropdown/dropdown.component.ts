import { AfterViewInit, ChangeDetectorRef, Component, forwardRef, Input, OnInit, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { PaginationConfig } from 'app/shared/components/pagination/pagination.config';
import { FilterModel } from 'app/shared/models/filter.model';
import { SelectItem } from 'app/shared/models/select-item.model';
import { HttpService } from 'app/shared/services/http.service';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';

@Component({
  selector: 'app-dropdown',
  templateUrl: './dropdown.component.html',
  styleUrls: ['./dropdown.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DropdownComponent),
      multi: true,
    },
  ]
})
export class DropdownComponent implements OnInit, ControlValueAccessor, AfterViewInit {
  @ViewChild('searchinput') input: any;

  @Input() items: SelectItem[] = [];
  @Input() label: string = "";
  @Input() multiple: boolean = false;
  @Input() placeholder: string = "";
  @Input() disabled: boolean = false;
  @Input() searchable: boolean = false;
  @Input() invalid: boolean = false;
  @Input() errorMessage: string = "";
  @Input() touched: boolean = false;
  @Input() requierd: boolean = false;
  @Input() clearable:boolean = true;

  @Input() searchProp: string = "name";

  //// DROPDOWN CHANGE
  @Input() returnObject:boolean = false;

  private sourceUrl: string;
  // public filters: Filter = { skip: PaginationConfig.skip, take: PaginationConfig.take };
  public filters: FilterModel = { skip: PaginationConfig.skip, take: PaginationConfig.take };
  private displayName: Array<string> = [];
  @Input() set dropdownData(value: DropDownDataModel) {
    this.items = [];
    this.filters = {...this.filters, ...value.additionalFilters};
    this.sourceUrl = value.source;
    if(value.displayName) {
      this.displayName = value.displayName;
    }
    setTimeout(() => {
      this.getData();
    },500)
  }

  value: number | number[] = null;
  onChange: any = () => {};
  onTouch: any = () => {};

  constructor(
    private httpService: HttpService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void { }

  ngAfterViewInit() {
    fromEvent(this.input.element, 'input')
        .pipe(
            debounceTime(500),
            distinctUntilChanged(),
            tap((text) => {
              if(this.input.searchTerm !== null) {
                // let res = this.items.find(x => x.name.toString().toLowerCase().includes(this.input.searchTerm.toString().toLowerCase()));
                let res = this.items.find(x => (x.name.toString().toLowerCase()).trim() === (this.input.searchTerm.toString().toLowerCase()).trim());
                if(this.input.searchTerm !== "" && res == undefined) {
                  this.getSearchData(this.input.searchTerm);
                }
              }
              else {
                if(!this.multiple) {
                  this.filters.skip = PaginationConfig.skip;
                  this.filters.take = PaginationConfig.take;
                  this.items = [];
                  this.getData();
                }
              }
            })
        )
        .subscribe();
  }

  async getSearchData(value: any) {
    if(!this.sourceUrl) {
      return;
    }
    let prop = this.searchProp;
    let filter;

    // if(value) {
    //   filter = {...this.filters, ...{[prop]: value}}
    // } else {
    //   this.filters.skip = 1;
    //   filter = this.filters;
    // }

    filter = {...this.filters, ...{[prop]: value}};

    filter.skip = PaginationConfig.skip;
    filter.take = PaginationConfig.take;

    let res = await this.httpService.get<any>(this.sourceUrl, filter, null, true).toPromise();
    if(res && res.isSuccess) {
      let newItems: Array<SelectItem> = [];
      newItems = res.value.map(el => {
        let name: string = "";
        if(this.displayName.length > 0) {
          this.displayName.forEach(prop => {
            let value = {...el};
            let keys = prop.split('.');
            // keys.forEach(e => {
            //   if(value[e]) {
            //     value = value[e];
            //   }
            // })
            for(let i=0; i<keys.length; i++) {
              if(value[keys[i]] !== undefined) {
                value = value[keys[i]]
              } else {
                value = "";
                break;
              }
            }
            name = name + " " + value;
          })
        } else {
          name = el.name;
        }
        return {
          id: el.id,
          name,
        }
      });
      // if(this.multiple) {
      //   this.items = [...this.items, ...newItems]
      // } else {
      //   this.items = [...newItems]
      // }

      let removeedDublicated = [];
      newItems.forEach(x => {
        if(this.items.find(y => y.id === x.id) === undefined) {
          removeedDublicated.push(x)
        }
      })
      // this.items = [...this.items, ...newItems];
      this.items = [...this.items, ...removeedDublicated];
      this.cdr.detectChanges();
    }
  }

  writeValue(obj: any): void {
    //// DROPDOWN CHANGE
    if(this.returnObject) {
      this.value = obj? obj.id : null;
    } else {
      this.value = obj;
    }

    if(Array.isArray(this.value)) {
      let isFound: boolean = true;
      this.value.forEach(x => {
        if(this.items.find(a => a.id === x) === undefined) {
          isFound = false;
        }
      })
      if(!isFound) {
        this.getMultiple();
      }
    } else {
      if(this.items.find(x => x.id === this.value) === undefined) {
        this.getDataById();
      }
    }

  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouch = fn;
  }
  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled
  }
  setValue() {
    //// DROPDOWN CHANGE
    let res: any;
    if(this.returnObject) {
      res = this.items.find(x => x.id === this.value);
    }else {
      res = this.value;
    }
    this.onChange(res);
    this.onTouch();
  }

  public async getData() {
    if(!this.sourceUrl) {
      return;
    }
    let res = await this.httpService.get<any>(this.sourceUrl, this.filters, null, true).toPromise();
    if(res && res.isSuccess) {
      let newItems: Array<SelectItem> = [];

      newItems = res.value.map(el => {
        let name: string = "";
        if(this.displayName.length > 0) {
          this.displayName.forEach(prop => {
            let value = {...el};
            let keys = prop.split('.');
            for(let i=0; i<keys.length; i++) {
              if(value[keys[i]] !== undefined) {
                value = value[keys[i]]
              } else {
                value = "";
                break;
              }
            }
            name = name + " " + value;
          })
        } else {
          name = el.name;
        }

        //// DROPDOWN CHANGE
        let dropdownItem: any = {...el};
        dropdownItem.name = name;
        return dropdownItem;

        // return {
        //   id: el.id,
        //   name,
        // }

      });

      this.items = [...this.items, ...newItems];

      //// removing dublicates from items, ի հայտ էր գալիս ջավա սկրիպտի ասինխրոնությյան պատճառող,
      //// սկզբում ուզում ա գտնի բոլորի մեջ բայց քանի որ չկա գեթ ա անում բայ այդի,
      //// հետո հիմնական դատան գալիս ա ու դուբլիկատ ա լինում
      // let dublicates = this.items.filter(x => x.id === this.value);
      // if(dublicates.length >= 2) {
      //   console.log(dublicates[0]);
      //   console.log(this.items.indexOf(dublicates[0]));
      // }
      let uniques = [];
      this.items.forEach(x => {
        if(uniques.find(a => a.id === x.id) === undefined) {
          uniques.push(x)
        }
      });

      this.items = [...uniques];

      if(this.filters.skip === 1 && this.value !== null) {
        if(!this.multiple) {
          if(this.items.find(x => x.id === this.value) === undefined) {
            this.value = null;
          }
        } else {
          (this.value as Array<number>).forEach((x, index) => {
            if(this.items.find(x => x.id === this.value) === undefined) {
              (this.value as Array<number>).splice(index, 1);
            }
          })
        }
      }
      this.cdr.detectChanges();
    }
  }

  private async getDataById() {
    if(this.sourceUrl === undefined || this.sourceUrl == null || this.value === null || this.value === undefined) {
      return;
    }
    let id: number;
    if(!Array.isArray(this.value)) {
      id =this.value
    } else {
      return;
    }
    let res = await this.httpService.get<any>(this.sourceUrl, {}, id, true).toPromise();
    if(res && res.isSuccess && res.value.length > 0) {
      let name: string = "";
      if(this.displayName.length > 0) {
        this.displayName.forEach(prop => {
          let value = {...res.value[0]};
          let keys = prop.split('.');
          for(let i=0; i<keys.length; i++) {
            if(value[keys[i]] !== undefined) {
              value = value[keys[i]]
            } else {
              value = "";
              break;
            }
          }
          name = name + " " + value;
        })
      } else {
        name = res.value[0].name;
      }

      //// DROPDOWN CHANGE
      let dropdownItem: any = {...res.value[0]};
      dropdownItem.name = name;
      this.items.push(dropdownItem)

      // this.items.push({
      //   id: res.value[0].id,
      //   name
      // });

      this.items = [...this.items];
      this.cdr.detectChanges();
    }
  }

  public async getMultiple() {
    if(!this.sourceUrl) {
      return;
    }
    let res = await this.httpService.get<any>(this.sourceUrl, {ids: this.value}, null, true).toPromise();
    if(res && res.isSuccess) {
      let newItems: Array<SelectItem> = [];
      newItems = res.value.map(el => {
        let name: string = "";
        if(this.displayName.length > 0) {
          this.displayName.forEach(prop => {
            let value = {...el};
            let keys = prop.split('.');
            // keys.forEach(e => {
            //   if(value[e]) {
            //     value = value[e];
            //   }
            // })
            for(let i=0; i<keys.length; i++) {
              if(value[keys[i]] !== undefined) {
                value = value[keys[i]]
              } else {
                value = "";
                break;
              }
            }
            name = name + " " + value;
          })
        } else {
          name = el.name;
        }

        //// DROPDOWN CHANGE
        let dropdownItem: any = {...el};
        dropdownItem.name = name;
        return dropdownItem;

        // return {
        //   id: el.id,
        //   name,
        // }

      });
      this.items = [...this.items, ...newItems];
      this.cdr.detectChanges();
    }
  }

  public scrollEnd() {
    if(this.sourceUrl === undefined || this.sourceUrl === null) {
      return;
    }
    this.filters.skip++;
    this.getData();
  }

  public search(event: any) {
    // if(event.term === "") {
    //   this.filters.skip = 1;
    //   this.items = [];
    //   this.getData();
    // }
  }

  public onClearAll() {
    if(!this.sourceUrl) {
      return;
    }
    this.filters.skip = 1;
    this.items = [];
    this.getData();
  }

}

export interface DropDownDataModel {
  source: string;
  additionalFilters?: any
  displayName?: Array<string>,
}
