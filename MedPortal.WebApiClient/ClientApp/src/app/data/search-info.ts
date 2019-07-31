import { SearchInfoType } from "./search-info-type";
import { ISearchItem } from "./search-item";

export interface ISearchCategory {
  type: SearchInfoType;
  items: ISearchItem[];
}
