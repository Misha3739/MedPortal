export interface ICoordinates {
    latitude: number;
    longitude: number;
}

export interface ICoordinatesStorage {
  coordinates: ICoordinates;
  date: Date;
}
