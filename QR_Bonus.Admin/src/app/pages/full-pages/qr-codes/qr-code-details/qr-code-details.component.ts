import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { PageType } from 'app/shared/enums/page-type.enum';
import { QrCodeModel } from 'app/shared/models/qr-code.model';

@Component({
  selector: 'app-qr-code-details',
  templateUrl: './qr-code-details.component.html',
  styleUrls: ['./qr-code-details.component.scss']
})
export class QrCodeDetailsComponent implements OnInit {

  public id = this.activatedRoute.snapshot.paramMap.get("id");

  public type = this.activatedRoute.snapshot.paramMap.get("type");

  public data: QrCodeModel = {} as QrCodeModel;

  constructor(
    public location: Location,
    private activatedRoute: ActivatedRoute,
    private http: HttpService
  ) { }

  ngOnInit(): void {
    if(this.type === PageType.edit) {
      this.getData();
    }
  }

    private async getData() {
      let res = await this.http.get<QrCodeModel>(Urls.QrCode, {}, parseInt(this.id)).toPromise();
      if(res && res.isSuccess) {
        this.data = res.value;
      }
    }

}
