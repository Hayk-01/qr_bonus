import { EventEmitter, Injectable } from '@angular/core';
import { PaginationModel } from '../models/pagination.model';

@Injectable({
  providedIn: 'root'
})
export class EventsEmitterService {

  public paginationDataEmiter: EventEmitter<PaginationModel> = new EventEmitter<PaginationModel>();

  constructor() { }
}
