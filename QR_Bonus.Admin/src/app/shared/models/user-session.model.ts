import { UserModel } from "./user.model";

export interface UserSessionModel {
  userId: number;
  token: string;
  isExpired: boolean
  user?: UserModel
}
