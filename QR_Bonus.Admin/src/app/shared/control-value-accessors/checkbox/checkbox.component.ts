import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-checkbox',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CheckboxComponent),
      multi: true,
    },
  ]
})
export class CheckboxComponent implements OnInit, ControlValueAccessor {
  @Input() label: string = "";
  @Input() disabled: boolean = false;
  @Input() requierd:boolean = false;
  @Input() invalid:boolean = false;
  @Input() errorMessage: string = "";
  @Input() touched: boolean = false;
  value: boolean = false;
  onChange: any = () => {};
  onTouch: any = () => {};

  constructor() { }

  ngOnInit(): void {}

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
  setValue(event) {
    this.value = event.target.checked;
    this.onChange(this.value)
    this.onTouch()
  }

}
