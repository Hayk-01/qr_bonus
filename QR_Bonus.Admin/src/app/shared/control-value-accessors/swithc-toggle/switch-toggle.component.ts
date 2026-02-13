import { Component, Input, OnInit, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-switch-toggle',
  templateUrl: './switch-toggle.component.html',
  styleUrls: ['./switch-toggle.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SwitchToggleComponent),
      multi: true,
    },
  ]
})
export class SwitchToggleComponent implements OnInit, ControlValueAccessor  {
  value: boolean = false;
  @Input() label: string = "";
  @Input() disabled: boolean = false;
  @Input() invalid: boolean = false;
  @Input() errorMessage: string = "";
  @Input() touched: boolean = false;
  @Input() requierd: boolean = false;
  @Input() returnNullWhenValueIsFalse: boolean = false;
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
    if(this.returnNullWhenValueIsFalse) {
      this.value = event.target.checked ? true : null;
    } else {
      this.value = event.target.checked;
    }
    this.onChange(this.value)
    this.onTouch()
  }

}
