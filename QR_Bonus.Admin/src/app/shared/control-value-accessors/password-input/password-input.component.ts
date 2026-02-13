import { AfterViewInit, Component, ElementRef, forwardRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Subscription, fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-password-input',
  templateUrl: './password-input.component.html',
  styleUrls: ['./password-input.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => PasswordInputComponent),
      multi: true,
    },
  ]
})
export class PasswordInputComponent implements OnInit, AfterViewInit, OnDestroy, ControlValueAccessor {
  @ViewChild("input") input: ElementRef;
  @Input() label: string = "";
  @Input() placeholder: string = "";
  @Input() disabled: boolean = false;
  @Input() invalid: boolean = false;
  @Input() errorMessage: string = "";
  @Input() touched: boolean = false;
  @Input() requierd: boolean = false;
  value: string = "";
  onChange: any = () => { };
  onTouch: any = () => { };
  private inputSubscription: Subscription;

  public isPassVisible: boolean = false;

  constructor() { }

  ngOnInit(): void { }

  ngAfterViewInit(): void {
    this.inputSubscription = fromEvent(this.input.nativeElement, 'input')
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
