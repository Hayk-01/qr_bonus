import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-area-code-selector',
  templateUrl: './area-code-selector.component.html',
  styleUrls: ['./area-code-selector.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => AreaCodeSelectorComponent),
      multi: true,
    },
  ]
})
export class AreaCodeSelectorComponent implements OnInit, ControlValueAccessor {
  @Input() label: string = "";
  @Input() disabled: boolean = false;
  @Input() requierd:boolean = false;
  @Input() invalid:boolean = false;
  @Input() errorMessage: string = "";
  @Input() touched: boolean = false;
  value: boolean = false;
  onChange: any = () => {};
  onTouch: any = () => {};

  public areacodes: any = [
    {
      name: '+374',
      value: '+374',
      flag: './assets/regions-flags/arm.png'
    },
    {
      name: '+995',
      value: '+995',
      flag: './assets/regions-flags/geo.png'
    }
  ]

  constructor() { }

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
  setValue() {
    this.onChange(this.value)
    this.onTouch()
  }

  onClear() {
    this.value = null;
    this.onChange(null)
    this.onTouch()
  }

  ngOnInit(): void {}

}
