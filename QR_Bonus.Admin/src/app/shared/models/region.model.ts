import { BaseModel } from "./base.model";
import { TranslationModel } from "./translation.model";

export interface RegionModel extends BaseModel {
  areaCode: string,
  translations?: Array<TranslationModel>;
  name?: string
}
