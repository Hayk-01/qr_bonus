import { BaseModel } from "./base.model";

export interface UserModel extends BaseModel {
  firstName: string;
  lastName: string;
  userName: string;
  password?: string;
  role?: number
}
