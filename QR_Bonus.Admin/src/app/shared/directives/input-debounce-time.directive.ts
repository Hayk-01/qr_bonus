import { AfterViewInit, Directive, ElementRef, EventEmitter, Input, Output } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Directive({
  selector: '[appInputDebounceTime]'
})
export class InputDebounceTimeDirective implements AfterViewInit  {

  @Input() debounceTime = 800;
  @Output() debouncedValue = new EventEmitter<string>();

  constructor(private elementRef: ElementRef) {}

  ngAfterViewInit() {
    fromEvent(this.elementRef.nativeElement, 'keyup')
      .pipe(debounceTime(this.debounceTime))
      .subscribe(() => {
      this.debouncedValue.emit(this.elementRef.nativeElement.value);
    });
  }

}
