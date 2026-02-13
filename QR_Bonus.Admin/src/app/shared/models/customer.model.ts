import { BaseModel } from "./base.model";
import { RegionModel } from "./region.model";

export interface CustomerModel extends BaseModel {
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  email?: string;
  regionId?: number;
  region?: RegionModel
}
