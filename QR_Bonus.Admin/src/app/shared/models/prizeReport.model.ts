import { BaseModel } from "./base.model";

export interface PrizeReport extends BaseModel {
  prizeId?: number;
  prizeName?: string;
  qrTotalQuantity?: number;
  winTotalQuantity?: number;
  claimedPrizeQuantity?: number;
  unclaimedPrizeQuantity?: number;
}
