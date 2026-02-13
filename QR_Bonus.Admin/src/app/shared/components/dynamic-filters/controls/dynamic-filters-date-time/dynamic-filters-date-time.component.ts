import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-dynamic-filters-date-time',
  templateUrl: './dynamic-filters-date-time.component.html',
  styleUrls: ['./dynamic-filters-date-time.component.scss']
})
export class DynamicFiltersDateTimeComponent {

  label: string = "_";

  form: FormGroup;

  formControlName: string = "";

  isDateFrom:boolean = false;

  isDateTo:boolean = false;

  showTime:boolean = false;

}
