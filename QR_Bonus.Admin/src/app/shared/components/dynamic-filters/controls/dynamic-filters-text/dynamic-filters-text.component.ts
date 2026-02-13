import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-dynamic-filters-text',
  templateUrl: './dynamic-filters-text.component.html',
  styleUrls: ['./dynamic-filters-text.component.scss']
})
export class DynamicFiltersTextComponent {
  label: string = "_";

  placeholder: string = " ";

  form: FormGroup;

  formControlName: string = "";

}
