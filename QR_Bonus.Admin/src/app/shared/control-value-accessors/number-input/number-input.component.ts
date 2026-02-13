import { AfterViewInit, Component, ElementRef, forwardRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { Subscription, fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-number-input',
  templateUrl: './number-input.component.html',
  styleUrls: ['./number-input.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NumberInputComponent),
      multi: true,
    },
  ]
})
export class NumberInputComponent implements OnInit, AfterViewInit, OnDestroy, ControlValueAccessor {
  @ViewChild("input") input: ElementRef;
  @Input() label: string = "";
  @Input() placeholder: string = "";
  @Input() disabled: boolean = false;
  @Input() invalid:boolean = false;
  @Input() errorMessage: string = "";
  @Input() touched: boolean = false;
  @Input() requierd:boolean = false;
  @Input() doNotallowFirstZero:boolean = false;
  @Input() smallVersion:boolean = false;
  @Input() isFloat:boolean = false;
  @Input() digitsLenghtAfterPoint: number = 2;
  value: string = "";
  onChange: any = () => {};
  onTouch: any = () => {};
  private inputSubscription: Subscription;

  constructor() { }

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    this.inputSubscription =  fromEvent(this.input.nativeElement, 'input')
    .pipe(debounceTime(300))
    .subscribe((res: any) => {
      this.value = res.target.value;
      this.onChange(this.value)
      this.onTouch()
    });
  }

  writeValue(obj: any): void {
    this.value = obj
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

  ngOnDestroy(): void {
    this.inputSubscription.unsubscribe();
  }

}
