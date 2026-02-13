import { CustomerModel } from "./customer.model";
import { PrizeModel } from "./prize.model";

export interface LeaderBoardModel {
    position: number;
    points: number;
    customer: CustomerModel;
    prize?: PrizeModel
}