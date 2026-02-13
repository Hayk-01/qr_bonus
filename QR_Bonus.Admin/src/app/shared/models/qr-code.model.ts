import { AddQRCountModel } from "./addQrCount.model";
import { BaseModel } from "./base.model";
import { CampaignModel } from "./campaign.model";
import { CustomerModel } from "./customer.model";
import { PrizeModel } from "./prize.model";
import { RegionModel } from "./region.model";
import { UserModel } from "./user.model";

export interface QrCodeModel extends BaseModel {
  value?: string;
  productCampaignId: number;
  qrCodeCounts?: Array<AddQRCountModel>;
  prizeId?: number;
  prize?: PrizeModel;
  customerId?: number;
  customer?: CustomerModel
  isExported?: boolean;
  isWinReceived?: boolean;
  hasCustomerConfirmed?: boolean;
  prizeDeliveryDate?: string;
  prizeReceiveDate?: string;
  userId?: number;
  user?: UserModel;
  regionId?: number;
  region?: RegionModel
}
