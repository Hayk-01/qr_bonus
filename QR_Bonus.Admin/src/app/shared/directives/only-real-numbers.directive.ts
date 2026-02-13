import { Directive, HostListener, Input } from '@angular/core';

@Directive({
  selector: '[appOnlyRealNumbers]'
})
export class OnlyRealNumbersDirective {
  @Input() doNotallowFirstZero: boolean = false;

  @Input() isFloat: boolean = false;

  @Input() digitsLenghtAfterPoint: number = 2;

  constructor() { }

  @HostListener('keydown', ['$event'])
  public onEvent(event: any) {

    if(!this.isFloat) {

      if((event.target.value.length === 0 && event.key === '0') && this.doNotallowFirstZero) event.preventDefault(); 
      if(event.key === ' ') event.preventDefault(); 
      if(isNaN(+event.key) && event.key !== 'Backspace' && event.key !== 'Delete' && event.key !== 'ArrowLeft' && event.key !== 'ArrowRight' && !event.ctrlKey) event.preventDefault();

    } else {

      if(event.target.value.includes('.')) {
        let x = event.target.value.split('.')[1];
        let length = x.length
        if(length > this.digitsLenghtAfterPoint) {
          if(event.key !== 'Backspace') event.preventDefault();
          if(event.key === 'Backspace') length = length - 1;
        }
      }

      if(event.target.value.length === 0 && event.key === '.') event.preventDefault(); 
      if(event.key === ' ') event.preventDefault(); 
      if(event.target.value === '0' && event.key !== '.' && event.key !== 'Backspace' && event.key !== 'Delete' && event.key !== 'ArrowLeft' && event.key !== 'ArrowRight' && !event.ctrlKey) event.preventDefault(); 
      if((event.target.value.length > 1 && event.target.value.includes('.')) && event.key === '.') event.preventDefault(); 
      if(isNaN(+event.key) && event.key !== 'Backspace' && event.key !== 'Delete' && event.key !== 'ArrowLeft' && event.key !== 'ArrowRight' && !event.ctrlKey && event.key !== '.') event.preventDefault();

    }
  }

}
