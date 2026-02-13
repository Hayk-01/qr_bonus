import { AddQRCountModel } from "./addQrCount.model";

export interface AddQrCodeModel {
    productCampaignId: number;
    qrCodeCounts: Array<AddQRCountModel>;
    activationDate: string
}
