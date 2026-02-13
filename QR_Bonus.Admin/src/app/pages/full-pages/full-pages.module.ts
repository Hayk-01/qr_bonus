import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FullPagesRoutingModule } from './full-pages-routing.module';
import { UsersComponent } from './users/users.component';
import { PipeModule } from 'app/shared/pipes/pipe.module';
import { SharedModule } from 'app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserDetailsComponent } from './users/user-details/user-details.component';
import { CampaignsComponent } from './campaigns/campaigns.component';
import { ProductsComponent } from './products/products.component';
import { QrCodesComponent } from './qr-codes/qr-codes.component';
import { GiftsComponent } from './gifts/gifts.component';
import { BannersComponent } from './banners/banners.component';
import { CampaingDetailsComponent } from './campaigns/campaing-details/campaing-details.component';
import { ProductDetailsComponent } from './products/product-details/product-details.component';
import { GiftDetailsComponent } from './gifts/gift-details/gift-details.component';
import { QrCodeDetailsComponent } from './qr-codes/qr-code-details/qr-code-details.component';
import { BannerDetailsComponent } from './banners/banner-details/banner-details.component';
import { AddProductPopupComponent } from './campaigns/campaing-details/add-product-popup/add-product-popup.component';
import { AddQrComponent } from './campaigns/campaing-details/add-qr/add-qr.component';
import { LeaderBoardEditComponent } from './campaigns/campaing-details/leader-board-edit/leader-board-edit.component';
import { CustomersComponent } from './customers/customers.component';
import { CustomerDetailsComponent } from './customers/customer-details/customer-details.component';
import { LeaderBoardComponent } from './campaigns/campaing-details/leader-board/leader-board.component';
import { ScannerComponent } from './scanner/scanner.component';
import { ReportsComponent } from './reports/reports.component';
import { RegionsComponent } from './regions/regions.component';
import { RegionDetailsComponent } from './regions/region-details/region-details.component';
import { QrScanComponent } from './qr-scan/qr-scan.component';
import { ScanResultComponent } from './qr-scan/scan-result/scan-result.component';
import { QrScannerDeviceComponent } from './qr-scanner-device/qr-scanner-device.component';
import { QrScannerDevicePopupComponent } from './qr-scanner-device-popup/qr-scanner-device-popup.component';
import { PushNotificationsComponent } from './push-notifications/push-notifications.component';

@NgModule({
  declarations: [
    UsersComponent,
    UserDetailsComponent,
    CampaignsComponent,
    ProductsComponent,
    QrCodesComponent,
    GiftsComponent,
    BannersComponent,
    CampaingDetailsComponent,
    ProductDetailsComponent,
    GiftDetailsComponent,
    QrCodeDetailsComponent,
    BannerDetailsComponent,
    AddProductPopupComponent,
    AddQrComponent,
    LeaderBoardEditComponent,
    CustomersComponent,
    CustomerDetailsComponent,
    LeaderBoardComponent,
    ScannerComponent,
    ReportsComponent,
    RegionsComponent,
    RegionDetailsComponent,
    QrScanComponent,
    ScanResultComponent,
    QrScannerDeviceComponent,
    QrScannerDevicePopupComponent,
    PushNotificationsComponent
  ],
  imports: [
    CommonModule,
    FullPagesRoutingModule,
    SharedModule,
    PipeModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class FullPagesModule { }
