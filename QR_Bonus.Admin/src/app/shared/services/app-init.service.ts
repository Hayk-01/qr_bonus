import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { StorageService, StorageValue } from './storage.service';
import { Language, LanguageService } from './language.service';
import { LanguagesEnum } from '../enums/languages.enum';
import { StaticTextsService } from './static-texts.service';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class AppInitService {

  constructor(
    private http: HttpClient,
    private staticTextsService: StaticTextsService,
    public toastr: ToastrService,
  ) { }

  async load() {
    LanguageService.SetDefaultLanguage();

    let staticTextsUrl = `${environment.cdn}/${LanguageService.GetLanguage().value}.json`;

    try {
      const headers = new HttpHeaders()
      let res: any = await this.http.get(staticTextsUrl, {headers}).toPromise();
      this.staticTextsService.staticTexts = res;
    } catch (err) {
      this.toastr.warning('Static text load error');
    }

    return;
  }

}
