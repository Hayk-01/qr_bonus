import { Injectable } from '@angular/core';
import { TranslationPipe } from '../pipes/translation.pipe';

@Injectable({
  providedIn: 'root'
})
export class FormErrorMessageProviderService {

  constructor(
    private translationPipe: TranslationPipe,
  ) { }

  public getErrorMessage(errors: any) {
    if(errors && errors.wiihespaceError) {
      return this.translationPipe.transform("admin_invalid_entered_value_spaces_problem") ;
    }
    if(errors && errors.required) {
      return this.translationPipe.transform("admin_this_field_is_required");
    }
    if(errors && errors.includesWiihespaceError) {
      return this.translationPipe.transform("admin_invalid_entered_value_includes_spaces_problem");
    }

    return this.translationPipe.transform("admin_this_field_is_required");
  }

}
