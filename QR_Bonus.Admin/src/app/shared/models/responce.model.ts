import { PageInfoModel } from "./page-info.model";

export interface ResponseModel<T> {
    pagedInfo: PageInfoModel;
    value?: T | null;
    status: number;
    isSuccess: boolean;
    successMessage: string;
    correlationId: string;
    errors: Array<any>;
    validationErrors: Array<any>;
}