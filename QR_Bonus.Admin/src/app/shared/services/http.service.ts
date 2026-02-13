import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { ResponseModel } from '../models/responce.model';
import { DeleteNullsUtility } from '../utility/delete-null.utility';
import { environment } from 'environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  constructor(
    private http: HttpClient,
    private spinner: NgxSpinnerService) { }

  get<T>(url: string, filter?: any, id?: number, hideSpinner?:boolean): Observable<ResponseModel<T>> {
    if(!hideSpinner) this.openSpinner();
    if(filter) DeleteNullsUtility.deteteNulls(filter);
    url = id ? url + '/' + id : url;
    return this.http.get<ResponseModel<T>>(environment.url + url, {params: filter});
  }

  post<T>(url:string, body?: any, hideSpinner?:boolean): Observable<ResponseModel<T>> {
    if(!hideSpinner) this.openSpinner();
    if(body) DeleteNullsUtility.deteteNulls(body);
    return this.http.post<ResponseModel<T>>(environment.url + url, body);
  }

  put<T>(url:string, body?: any, id? :number, hideSpinner?:boolean): Observable<ResponseModel<T>> {
    if(!hideSpinner) this.openSpinner();
    if(body) DeleteNullsUtility.deteteNulls(body);
    url = id ? url + '/' + id : url;
    return this.http.put<ResponseModel<T>>(environment.url + url , body);
  }

  delete<T>(url:string, id? :number, hideSpinner?:boolean): Observable<ResponseModel<T>> {
    if(!hideSpinner) this.openSpinner();
    url = id ? url + '/' + id : url;
    return this.http.delete<ResponseModel<T>>(environment.url + url);
  }

  public downloadGetRequest(url: string, filter?: any): any {
    if(filter) DeleteNullsUtility.deteteNulls(filter);
    return this.http.get(environment.url + url, { responseType: 'blob', params: filter});
  }

  private openSpinner() {
    this.spinner.show(undefined, {
        type: 'ball-triangle-path',
        size: 'medium',
        bdColor: 'rgba(0, 0, 0, 0.8)',
        color: '#fff',
        fullScreen: true
    });
  }

}

export enum Urls {
  UsersessionLogin = "usersession/login",
  UsersessionLogout = "usersession/logout",
  User = "user",
  Product = "product",
  Campaign = "campaign",
  CampaignLeaderBoardById = "campaign/campaignId/leaderboard",
  CampaignLeaderBoardPrizes = "campaign/leaderboard/prizes",
  CampaignLeaderBoardPrizesById = "campaign/campaignId/leaderboard/prizes",
  CampaignStart = "campaign/start",
  CampaignEnd = "campaign/end",
  Prize = "prize",
  QrCode = 'qrcode',
  QrCodeExportCsv = 'qrcode/export-csv',
  QrCodeWinRecieve = 'qrcode/id/deliver',
  QrcodeWinExportExcel = 'qrcode/win-export-excel',
  Banner = 'banner',
  Customer = 'customer',
  CustomerPoints = 'customer/customerId/points',
  ReportPrize = 'report/prize',
  ReportPrizeExport = 'report/prize/export',
  Region = 'region',
  PushNotification = 'pushnotification'
}
