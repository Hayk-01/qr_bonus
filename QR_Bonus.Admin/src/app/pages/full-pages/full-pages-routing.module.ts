import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './users/users.component';
import { UserDetailsComponent } from './users/user-details/user-details.component';
import { CampaignsComponent } from './campaigns/campaigns.component';
import { ProductsComponent } from './products/products.component';
import { GiftsComponent } from './gifts/gifts.component';
import { QrCodesComponent } from './qr-codes/qr-codes.component';
import { BannersComponent } from './banners/banners.component';
import { CampaingDetailsComponent } from './campaigns/campaing-details/campaing-details.component';
import { ProductDetailsComponent } from './products/product-details/product-details.component';
import { GiftDetailsComponent } from './gifts/gift-details/gift-details.component';
import { QrCodeDetailsComponent } from './qr-codes/qr-code-details/qr-code-details.component';
import { BannerDetailsComponent } from './banners/banner-details/banner-details.component';
import { CustomersComponent } from './customers/customers.component';
import { CustomerDetailsComponent } from './customers/customer-details/customer-details.component';
import { ReportsComponent } from './reports/reports.component';
import { RegionsComponent } from './regions/regions.component';
import { RegionDetailsComponent } from './regions/region-details/region-details.component';
import { QrScanComponent } from './qr-scan/qr-scan.component';
import { PermissionGuard } from 'app/shared/guards/permission.guard';
import { ScanResultComponent } from './qr-scan/scan-result/scan-result.component';
import { PushNotificationsComponent } from './push-notifications/push-notifications.component';

const routes: Routes = [

  //////////// campaigns ///////////
  {
    path: 'campaigns',
    component: CampaignsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false, redirectPath: 'products'}
  },
  {
    path: 'campaigns/:type',
    component: CampaingDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },
  {
    path: 'campaigns/:type/:id',
    component: CampaingDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },

  //////////// products ///////////
  {
    path: 'products',
    component: ProductsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false, redirectPath: 'prizes'}
  },
  {
    path: 'products/:type',
    component: ProductDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },
  {
    path: 'products/:type/:id',
    component: ProductDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },

  //////////// prizes ///////////
  {
    path: 'prizes',
    component: GiftsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false, redirectPath: 'qr-codes'}
  },
  {
    path: 'prizes/:type',
    component: GiftDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },
  {
    path: 'prizes/:type/:id',
    component: GiftDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },

  //////////// qr-codes ///////////
  {
    path: 'qr-codes',
    component: QrCodesComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false, redirectPath: 'users'}
  },
  {
    path: 'qr-codes/:type',
    component: QrCodeDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },
  {
    path: 'qr-codes/:type/:id',
    component: QrCodeDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },

  //////////// users ///////////
  {
    path: 'users',
    component: UsersComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false, redirectPath: 'customers'}
  },
  {
    path: 'users/:type',
    component: UserDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },
  {
    path: 'users/:type/:id',
    component: UserDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },

  //////////// customers ///////////
  {
    path: 'customers',
    component: CustomersComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false, redirectPath: 'regions'}
  },
  {
    path: 'customers/:type/:id',
    component: CustomerDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },

  //////////// regions ///////////
  {
    path: 'regions',
    component: RegionsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false, redirectPath: 'banners'}
  },
  {
    path: 'regions/:type',
    component: RegionDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },
  {
    path: 'regions/:type/:id',
    component: RegionDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },

  //////////// banners ///////////
  {
    path: 'banners',
    component: BannersComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false, redirectPath: 'reports'}
  },
  {
    path: 'banners/:type',
    component: BannerDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },
  {
    path: 'banners/:type/:id',
    component: BannerDetailsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false}
  },

  ////////// reports ///////////
  {
    path: 'reports',
    component: ReportsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false, redirectPath: 'scan'}
  },

  ////////// qr-scan ///////////
  {
    path: 'scan',
    component: QrScanComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: true, redirectPath: 'push-notifications'}
  },
  {
    path: 'scan/result',
    component: ScanResultComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: true}
  },

  //////////// push notifications ///////////
  {
    path: 'push-notifications',
    component: PushNotificationsComponent,
    canActivate: [PermissionGuard],
    data: {isCourierPage: false, redirectPath: '/'}
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FullPagesRoutingModule { }
