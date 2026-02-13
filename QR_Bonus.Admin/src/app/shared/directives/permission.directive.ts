import { Directive, ElementRef, Input, OnInit } from '@angular/core';
import { StorageService, StorageValue } from '../services/storage.service';
import { UserRole } from '../enums/user-role.enum';

@Directive({
  selector: '[appPermission]'
})
export class PermissionDirective implements OnInit {
  @Input() appPermission: {isCourierPage: boolean};

  constructor(private el: ElementRef) {}

  ngOnInit(): void {

    let userSession = StorageService.get(StorageValue.UserSession);

    // if(userSession.user.role === UserRole.admin) {
    //   return;
    // }

    if(userSession.user.role === UserRole.courier && !this.appPermission.isCourierPage) {
      this.el.nativeElement.style.display = 'none';
    }

  }

}
