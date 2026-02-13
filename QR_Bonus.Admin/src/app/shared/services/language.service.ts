import { Injectable } from '@angular/core';
import { LanguagesEnum, LanguageValueEnum } from '../enums/languages.enum';
import { StorageService, StorageValue } from './storage.service';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  public static languages: Array<Language> = [
    {
      id: LanguagesEnum.English,
      value: LanguageValueEnum.English,
      title: "admin_language_title_english",
      flag: '/assets/flags/uk.png',
    },
    {
      id: LanguagesEnum.Georgian,
      value: LanguageValueEnum.Georgian,
      title: "admin_language_title_georgian",
      flag: '/assets/flags/geo.png'
    }
  ]

  constructor() { }

  public static SetDefaultLanguage() {
    if(!StorageService.get(StorageValue.LanguageId)) {
      StorageService.set(StorageValue.LanguageId, LanguagesEnum.English);
    }
  }

  public static SetLanguage(id: number) {
    StorageService.set(StorageValue.LanguageId, id);
    location.reload();
  }

  public static GetLanguage(): Language {
    let languageId = StorageService.get(StorageValue.LanguageId) ?? LanguagesEnum.English;
    return this.languages.find(x => x.id === languageId);
  }

}

export interface Language {
  id: number,
  value: string,
  title: string,
  flag: string,
}
