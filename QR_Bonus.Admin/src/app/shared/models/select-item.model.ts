export interface SelectItem {
  id: number | boolean | null;
  name: string;
  title?: string;
  isDeleted?: boolean;
  // [key: string]: any
}
