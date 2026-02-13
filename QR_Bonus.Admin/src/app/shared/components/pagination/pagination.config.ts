import { FilterModel } from "app/shared/models/filter.model";

export const PaginationConfig: FilterModel = {
  skip: 1,
  take: 20,
  isDeleted: false
}

export const DropDownFilter: FilterModel = {
  skip: 1,
  take: 50,
  isDeleted: false
}
