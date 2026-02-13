import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'app-dynamic-filters-switch-toggle',
  templateUrl: './dynamic-filters-switch-toggle.component.html',
  styleUrls: ['./dynamic-filters-switch-toggle.component.scss']
})
export class DynamicFiltersSwitchToggleComponent {

  label: string = "_";

  form: FormGroup;

  formControlName: string = "";

  placeholder: string = "";

  returnNullWhenValueIsFalse:boolean = false

}
