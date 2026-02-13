import { Injectable, Pipe, PipeTransform } from '@angular/core';
import { StaticTextsService } from '../services/static-texts.service';

@Injectable({
  providedIn: 'root'
})
@Pipe({
  name: 'translation'
})
export class TranslationPipe implements PipeTransform {

  constructor(private staticTextsService: StaticTextsService) {}

  transform(key: string): any {
    if(this.staticTextsService.staticTexts) {
      return this.staticTextsService.staticTexts[key] ?? key;
    } else {
      return key;
    }
  }

}
