import { Injectable } from '@angular/core';
import { StaticText } from '../models/static-text.model';

@Injectable({
  providedIn: 'root'
})
export class StaticTextsService {

  public staticTexts: StaticText | null = null;

  constructor() { }
}
