import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'convertToZulu'
})
export class ConvertToZuluPipe implements PipeTransform {

  transform(value: string): string {
    if(value === "---") return;
    if(value !== null && value != undefined && !value.includes('Z')) {
      return value + 'Z';
    } else {
      return value;
    }
    ///// without UTC
    // if(value !== null && value != undefined && value.includes('Z')) {
    //   return value.replace("Z", '');
    // } else {
    //   return value;
    // }
  }

}
