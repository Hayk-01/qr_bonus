import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { StorageService, StorageValue } from '../services/storage.service';
import { UserRole } from '../enums/user-role.enum';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard implements CanActivate {

  constructor(private router: Router) {

  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    let isCourierPage:boolean = route.data.isCourierPage ?? false;

    let userSession = StorageService.get(StorageValue.UserSession);

    // if(userSession.user.role === UserRole.admin) {
    //   return true;
    // }

    if(userSession.user.role === UserRole.courier && !isCourierPage) {
      this.router.navigate([route.data.redirectPath]);
      return false;
    }

    return true;
  }

}
