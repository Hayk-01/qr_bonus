import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'tableRowValue'
})
export class TableRowValuePipe implements PipeTransform {

  transform(row, prop: string): any {
    if (prop == null) {
      return null;
    }

    let props = prop.split('.');
    let value = row;
    props.forEach(e => {
      if(value != null){
        value = value[e];
      }
    });
    if(value === undefined) {
      return "---"
    }
    return value;
  }

}
