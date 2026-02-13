import { Injectable } from '@angular/core';

export enum StorageValue {
  UserSession = 'UserSession',
  LanguageId = 'LanguageId',
  Permissions = 'Permissions'
}

@Injectable({
  providedIn: 'root'
})
export class StorageService {

  constructor() { }

  static get(key: StorageValue): any {
    try {
      const value = localStorage.getItem(key);
      if (value != null) {
        return JSON.parse(value);
      }
      return null;
    } catch (err) {
      return null;
    }
  }

  static set(key: StorageValue, value: any) {
    return localStorage.setItem(key, JSON.stringify(value));
  }

  static delete(key: StorageValue) {
    return localStorage.removeItem(key);
  }

}
