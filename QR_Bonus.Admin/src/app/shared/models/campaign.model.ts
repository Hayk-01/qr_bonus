import { BaseModel } from "./base.model";
import { ProductPointModel } from "./product-point.model";
import { RegionModel } from "./region.model";
import { TranslationModel } from "./translation.model";

export interface CampaignModel extends BaseModel {
  name?: string;
  startDate?: string;
  endDate?: string;
  status?: number;
  productPoints: Array<ProductPointModel>;
  translations?: Array<TranslationModel>;
  regionId?: number;
  region?: RegionModel
}
