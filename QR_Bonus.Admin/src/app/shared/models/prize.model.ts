import { BaseModel } from "./base.model";
import { FileModel } from "./file.model";
import { RegionModel } from "./region.model";
import { TranslationModel } from "./translation.model";

export interface PrizeModel extends BaseModel {
  name?: string;
  isLeaderboard?: boolean;
  addPhoto?: FileModel;
  link?: string;
  translations?: Array<TranslationModel>;
  regionId?: number;
  region?: RegionModel
}
