import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-dynamic-filters-number',
  templateUrl: './dynamic-filters-number.component.html',
  styleUrls: ['./dynamic-filters-number.component.scss']
})
export class DynamicFiltersNumberComponent {

  label: string = "_";

  placeholder: string = "";

  form: FormGroup;

  formControlName: string = "";

}
