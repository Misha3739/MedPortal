import { Injectable } from "@angular/core";
import { ISearchCategory } from "../data/search-info";
import { SearchInfoType } from "../data/search-info-type";

@Injectable()
export class SearchInfoService {
  getSearchInfo(city: string): ISearchCategory[] {
    let searchInfo: ISearchCategory[] = [];
    searchInfo.push({
      type: SearchInfoType.clinic,
      items: [{ id: 1, name: 'Центр Диетологии' }, { id: 2, name: 'Центр Простатологии' }, { id: 3, name: 'Центр Семейной медицины' }]
    });
    searchInfo.push({
      type: SearchInfoType.doctor,
      items: [{ id: 1, name: 'Петров Иван Федорович' }, { id: 2, name: 'Гривцова Ольга Александровна' }, { id: 3, name: 'Салихов Роберт Иосифович' }]
    });
    searchInfo.push({
      type: SearchInfoType.speciality,
      items: [{ id: 1, name: 'Уролог' }, { id: 2, name: 'Хирург' }, { id: 3, name: 'Гинеколог' }]
    });

    return searchInfo;
  }
}
