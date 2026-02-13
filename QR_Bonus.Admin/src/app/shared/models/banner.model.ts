import { BaseModel } from "./base.model";
import { FileModel } from "./file.model";
import { RegionModel } from "./region.model";
import { TranslationModel } from "./translation.model";

export interface BannerModel extends BaseModel {
  expirationDate?: string;
  addPhoto?: FileModel;
  link?: string;
  translations?: Array<TranslationModel>;
  regionId?: number;
  region?: RegionModel
}
