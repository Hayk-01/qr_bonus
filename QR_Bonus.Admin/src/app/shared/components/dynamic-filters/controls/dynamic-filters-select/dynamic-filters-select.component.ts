import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControlName, FormGroup } from '@angular/forms';
import { DropDownFilter } from 'app/shared/components/pagination/pagination.config';
import { SelectItem } from 'app/shared/models/select-item.model';
import { HttpService } from 'app/shared/services/http.service';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, tap } from 'rxjs/operators';

@Component({
  selector: 'app-dynamic-filters-select',
  templateUrl: './dynamic-filters-select.component.html',
  styleUrls: ['./dynamic-filters-select.component.scss']
})
export class DynamicFiltersSelectComponent {
  @ViewChild('searchinput') input: any;

  source: string = "";

  searchProp: string = "name";

  filter: any = {};

  items: Array<SelectItem | any> = [];

  itemsOriginal: Array<SelectItem | any> = [];

  label: string = "_";

  multiple: boolean = false;

  placeholder: string = "";

  form: FormGroup;

  formControlName: string = "";

  displayName: Array<string> = [];

  displayId:boolean = false;

  valueProp: string = "id"

  initialValueId: any = null;

  listenTo: {formControlName: string; filterProperty: string};

  constructor(
    private httpService: HttpService
    ) { }

  ngOnInit(): void {
    this.filter = {...this.filter, ...{ skip: DropDownFilter.skip, take: DropDownFilter.take }};
    if(this.source !== "" && this.items.length === 0) {
      this.getItems();
    }

    ///// dynamic filter improvements
    if(!this.source) {
      this.itemsOriginal = this.items.map(x => x);
    }
    if(this.listenTo !== undefined) {
      this.form.controls[this.listenTo.formControlName as any].valueChanges.subscribe(value => {
        let newFilters = {
          [this.listenTo.filterProperty]: value
        }
        this.filter.skip = 1;
        this.filter = {...this.filter, ...newFilters};

        if(this.source !== "") {
          this.items = [];
          this.getItems();
        } else {
          if(value !== null) {
            this.items = this.itemsOriginal.filter(x => x[this.listenTo.filterProperty] == value);
          } else {
            this.items = this.itemsOriginal;
          }
        }
      })
    }
    /////////////////////////////////

  }

  set initialValue(value: any) {
    this.initialValueId = value;
  }

  public scrollEnd() {
    this.filter.skip++;
    this.getItems();
  }

  public search(event: any) {
    // console.log(event);
  }

  public onClearAll() {
    if(!this.source) {
      return;
    }
    this.filter.skip = 1;
    this.items = [];
    this.getItems();
  }

  public async getItems() {

    if(!this.source) {
      return;
    }
    let res = await this.httpService.get<any[]>(this.source, this.filter, null, true).toPromise();
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
        return {
          id: el[this.valueProp],
          name,
          title: name,
          isDeleted: el.isDeleted
        }
      });
      this.items = [...this.items, ...newItems];

      if(this.initialValueId !== null && Array.isArray(this.initialValueId) && this.filter.skip === 1) {
        let isFound: boolean = true;
        this.initialValueId.forEach(x => {
          if(this.items.find(a => a.id === x) === undefined) {
            isFound = false;
          }
        })
        if(!isFound) {
          this.getMultiple();
        }
      }

      //// !isNaN(+this.initialValueId) ավելացրել եմ, getById ի ժամանակ id string էր, դրա համար
      if(this.initialValueId !== null && !isNaN(+this.initialValueId) && !Array.isArray(this.initialValueId) && this.filter.skip === 1) {
        //// allowing to call getById only if it's value is not in items, for remove dublicates reason
        let isExist = this.items.find(x => x.id == this.initialValueId) !== undefined;
        if(!isExist) {
          this.getById();
        }
      }

      //// ավելացրել եմ, getById ի ժամանակ id string էր, դրա համար
      if(this.initialValueId !== null && isNaN(+this.initialValueId)) {
        this.getSearchData(this.initialValueId);
      }

    }
  }

  public async getById() {
    if(!this.initialValueId) {
      return;
    }
    let res = await this.httpService.get<any>(this.source, {}, this.initialValueId, true).toPromise();
    if(res && res.isSuccess) {
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
      this.items.push({
        id: res.value[0][this.valueProp],
        name,
        title: name,
        isDeleted: res.value[0].isDeleted
      })
      this.items = [...this.items];
    }
  }

  public getTitle(id: number) {
    if(id && this.items.find(x => x.id === id) && this.items.find(x => x.id === id).name) {
      return this.items.find(x => x.id === id).name
    } else {
      return "";
    }
  }

  ngAfterViewInit() {
    fromEvent(this.input.element, 'input')
        .pipe(
            debounceTime(1000),
            distinctUntilChanged(),
            tap((text) => {
              if(this.input.searchTerm !== null) {
                let res = this.items.find(x => (x.name.toString().toLowerCase()).trim() === (this.input.searchTerm.toString().toLowerCase()).trim());
                if(this.input.searchTerm !== "" && res == undefined) {
                  this.getSearchData(this.input.searchTerm);
                }
              }
              else {
                if(!this.multiple) {
                  this.filter.skip = DropDownFilter.skip;
                  this.filter.take = DropDownFilter.take;
                  this.items = [];
                  this.getItems();
                }
              }
            })
        )
        .subscribe();
  }

  async getSearchData(value: any) {
    if(!this.source) {
      return;
    }
    let prop = this.searchProp;
    let searchFilters;

    searchFilters = {...this.filter, ...{[prop]: value}};

    let res = await this.httpService.get<any>(this.source, searchFilters, null, true).toPromise();
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
        return {
          id: el.id,
          name,
          title: name,
          isDeleted: el.isDeleted
        }
      });

      //// քոմենթել եմ, getById ի ժամանակ id string էր, դրա համար
      // if(this.multiple) {
      //   this.items = [...this.items, ...newItems]
      // } else {
      //   this.items = [...newItems]
      // }
      //// ավելացրել եմ, getById ի ժամանակ id string էր, դրա համար

      let removeedDublicated = [];
      newItems.forEach(x => {
        if(this.items.find(y => y.id === x.id) === undefined) {
          removeedDublicated.push(x)
        }
      })
      this.items = [...this.items, ...removeedDublicated];
    }
  }

  async getMultiple() {
    if(!this.source) {
      return;
    }
    let res = await this.httpService.get<any>(this.source, {ids: this.initialValueId}, null, true).toPromise();
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
        return {
          id: el.id,
          name,
          title: name,
          isDeleted: el.isDeleted
        }
      });
      this.items = [...this.items, ...newItems]
    }
  }

  public isSelectedItemDeleted(id: number) {
    let item = this.items.find(x => x.id == id);
    if(item && item.isDeleted) {
      return true;
    } else {
      return false;
    }
  }

  public removeItem(id: number) {
    if(this.multiple) {
      let filteredValue = this.form.value[this.formControlName].filter(x => x !== id);
      this.form.get(this.formControlName).patchValue(filteredValue);
    }
  }

}
