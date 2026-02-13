import { Directive, HostListener } from '@angular/core';

@Directive({
  selector: '[appNoStartOnWhiteSpace]'
})
export class NoStartOnWhiteSpaceDirective {

  constructor() { }

  @HostListener('keydown', ['$event'])
  public onEvent(event: any) {
    if(event.target.value.length ===0 && event.keyCode === 32) { // whitespace's keyCode is 32
      event.preventDefault();
    }
  }

}
