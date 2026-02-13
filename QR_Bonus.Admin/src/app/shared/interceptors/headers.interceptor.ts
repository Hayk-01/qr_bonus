import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { StorageService, StorageValue } from '../services/storage.service';
import { LanguageService } from '../services/language.service';

@Injectable()
export class HeadersInterceptor implements HttpInterceptor {

  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let token = StorageService.get(StorageValue.UserSession) ? StorageService.get(StorageValue.UserSession).token : '';
    const authRequest = request.clone({
      headers: request.headers
      .set('Authorization', token)
      .set('lang-code', (LanguageService.GetLanguage().id).toString())
    });
    return next.handle(authRequest);
  }

}
