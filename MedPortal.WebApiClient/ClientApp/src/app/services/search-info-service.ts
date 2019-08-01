import { Injectable } from "@angular/core";
import { ISearchCategory } from "../data/search-info";
import { SearchInfoType } from "../data/search-info-type";

@Injectable()
export class SearchInfoService {
  getSearchInfo(city: string): ISearchCategory[] {
    let searchInfo: ISearchCategory[] = [];
    searchInfo.push({
      type: SearchInfoType.clinic,
      items: [
        {
          id: 1,
          name: 'Центр Диетологии',
          alias: 'Center_Dietology'
        },
        {
          id: 2,
          alias: 'Center_Prostatology',
          name: 'Центр Простатологии'
        },
        {
          id: 3,
          alias: 'Center_FamilyMedicine',
          name: 'Центр Семейной медицины'
        }
      ]
    });
    searchInfo.push({
      type: SearchInfoType.doctor,
      items: [
        { id: 1, alias: 'Petrov_Ivan', name: 'Петров Иван Федорович' },
        { id: 2, alias: 'Grivtsova_Olga', name: 'Гривцова Ольга Александровна' },
      { id: 3, alias: 'Salikhov_Robert', name: 'Салихов Роберт Иосифович' }
      ]
    });
    searchInfo.push({
      type: SearchInfoType.speciality,
      items: [
        { id: 1, alias:'urologist', name: 'Уролог' },
        { id: 2, alias: 'surgeon', name: 'Хирург' },
        { id: 3, alias: 'ginecologist', name: 'Гинеколог' }]
    });

    return searchInfo;
  }
}
