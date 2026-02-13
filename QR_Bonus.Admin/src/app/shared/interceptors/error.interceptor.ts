import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HttpResponse
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { StorageService, StorageValue } from '../services/storage.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(
    private router: Router,
    public toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      map((response: any) => {

        if(response instanceof HttpResponse) {
          this.spinner.hide();
        }

        if(response.body && !response.body.isSuccess && response.body.errors) {
          let message = response.body.errors.join(' ') ?? '';
          this.toastr.error('Error' + (message && message.length > 0 ? (', ' + message) : ''));
        }

        return response as any;
      }),
      catchError((err: HttpErrorResponse) => {

        if(err instanceof HttpErrorResponse) {
          this.spinner.hide();
        }

        if(err) {
          this.toastr.error(err.message)
          switch (err.status) {
            case 400:
              this.toastr.error("status code 400" + err.error)
              break;
            case 401:
              this.toastr.error("status code 401" + err.error)
              StorageService.delete(StorageValue.UserSession);
              this.router.navigate(['/']);
              break;
            default:
              this.toastr.error('Error' + (err.error ? (', ' + err.error) : ''));
              break;
          }
        }
        throw err;
      })
    )
  }

}
