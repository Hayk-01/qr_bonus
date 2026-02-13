import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DropDownDataModel } from 'app/shared/control-value-accessors/dropdown/dropdown.component';
import { PageType } from 'app/shared/enums/page-type.enum';
import { ModalResponse } from 'app/shared/models/modal-response.model';
import { ProductPointModel } from 'app/shared/models/product-point.model';
import { Urls } from 'app/shared/services/http.service';

@Component({
  selector: 'app-add-product-popup',
  templateUrl: './add-product-popup.component.html',
  styleUrls: ['./add-product-popup.component.scss']
})
export class AddProductPopupComponent implements OnInit {
  public PageType = PageType;

  public pageType:string = PageType.new;

  public data: ProductPointModel = {
    productId: null,
    point: null,
    product: null
  }

  public regionId: number = null;

  public productsDropDownData: DropDownDataModel = {} as DropDownDataModel

  constructor(
    public activeModal: NgbActiveModal,
  ) { }

  ngOnInit(): void {
    this.productsDropDownData = {
      source: Urls.Product,
      additionalFilters: {regionIds: [this.regionId]}
    }
  }

  public save() {
    if(!this.data.product || !this.data.point) {
      return;
    }
    this.data.point = this.data.point*1;
    this.data.productId = this.data.product.id;
    let result: ModalResponse<any> = {isSuccess: true, data: this.data}
    this.activeModal.close(result)
  }

}
