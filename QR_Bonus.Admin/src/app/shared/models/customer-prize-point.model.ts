import { BaseModel } from "./base.model";
import { PrizeModel } from "./prize.model";

export interface CustomerPrizePointModel extends BaseModel {
  campaignId: number;
  points: number;
  prizes: Array<PrizeModel>
}
