import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PageType } from 'app/shared/enums/page-type.enum';
import { CustomerModel } from 'app/shared/models/customer.model';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { Location } from '@angular/common';
import { CustomerPrizePointModel } from 'app/shared/models/customer-prize-point.model';

@Component({
  selector: 'app-customer-details',
  templateUrl: './customer-details.component.html',
  styleUrls: ['./customer-details.component.scss']
})
export class CustomerDetailsComponent implements OnInit {

  public id = this.activatedRoute.snapshot.paramMap.get("id");

  public type = this.activatedRoute.snapshot.paramMap.get("type");

  public PageType = PageType;

  public data: CustomerModel = {} as CustomerModel;

  public active: number = 1;

  public customerPrizePoints: Array<CustomerPrizePointModel> = [];

  public slectedIndex:number | null = null;

  constructor(
    public location: Location,
    private activatedRoute: ActivatedRoute,
    private http: HttpService
  ) { }

  ngOnInit(): void {
    if(this.type === PageType.edit) {
      this.getData();
      this.getCustomerHistory();
    }
  }

  private async getData() {
    let res = await this.http.get<CustomerModel>(Urls.Customer, {}, parseInt(this.id)).toPromise();
    if(res && res.isSuccess) {
      this.data = res.value;
    }
  }

  private async getCustomerHistory() {
    let res = await this.http.get<Array<CustomerPrizePointModel>>(Urls.CustomerPoints.replace('customerId', this.id.toString())).toPromise();
    if(res && res.isSuccess) {
      this.customerPrizePoints = res.value;
    }
  }

  public openPrizesBlock(i: number) {
    if(this.slectedIndex == i) {
      this.slectedIndex = null;
    } else {
      this.slectedIndex = i;
    }
  }

}
