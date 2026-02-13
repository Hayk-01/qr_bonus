import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { DateTimeConverter } from 'app/shared/utility/datetime.converter';

@Component({
  selector: 'app-datetime-picker',
  templateUrl: './datetime-picker.component.html',
  styleUrls: ['./datetime-picker.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DatetimePickerComponent),
      multi: true,
    },
  ]
})
export class DatetimePickerComponent implements OnInit, ControlValueAccessor  {
  @Input() label: string = "";
  @Input() disabled: boolean = false;
  @Input() invalid:boolean = false;
  @Input() touched: boolean = false;
  @Input() errorMessage: string = "";
  @Input() isDateFrom:boolean = false;
  @Input() isDateTo:boolean = false;
  @Input() showTime:boolean = false;
  @Input() set max(value: string) {
    this.maxDate = DateTimeConverter.GetDateObjectForDataPicker(value);
  }
  @Input() set min(value: string) {
    this.minDate = DateTimeConverter.GetDateObjectForDataPicker(value);
  }

  public date: DateObject | null = null;
  public time: TimeObject = {hour: 0, minute: 0, second: 0};

  onChange: any = () => {};
  onTouch: any = () => {};

  public minDate: {year: number; month: number; day: number};
  public maxDate: {year: number; month: number; day: number};

  constructor() { }

  ngOnInit(): void {
    this.setTimeDefaultValue();
  }

  private setTimeDefaultValue() {
    if(this.isDateFrom) {
      this.time = {hour: 0, minute: 0, second: 0};
    }
    if(this.isDateTo) {
      this.time = {hour: 23, minute: 59, second: 59};
    }
  }

  writeValue(obj: any): void {
    if(obj == null || obj == "") {
      this.date = null;
      this.setTimeDefaultValue();
      return
    }

    let x = DateTimeConverter.GetDateObjectForDataPicker(obj);
    this.date = {year: x.year, month: x.month, day: x.day};
    this.time = {hour: x.hour, minute: x.minute, second: x.second};
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouch = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  public setDateTime() {
    if(this.date === null) return;

    if(this.time == null || !this.showTime) {
      this.setTimeDefaultValue();
    }

    let dateTime = DateTimeConverter.ConvertToUTC(this.date.year, this.date.month, this.date.day, this.time.hour, this.time.minute, this.time.second);
    this.onChange(dateTime);
    this.onTouch();
  }

}

interface DateObject {
  year: number,
  month: number,
  day: number
}

interface TimeObject {
  hour: number,
  minute: number,
  second: number
}
