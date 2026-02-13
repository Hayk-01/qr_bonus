import { ProductModel } from "./product.model";

export interface ProductPointModel {
  productId: number;
  point: number;
  product?: ProductModel;
  productCampaignId?: number
}
