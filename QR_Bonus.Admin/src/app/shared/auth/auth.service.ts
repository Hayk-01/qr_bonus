import { Router } from '@angular/router';
import { Injectable } from '@angular/core';
// import { AngularFireAuth } from "@angular/fire/auth";
// import firebase from 'firebase/app'
// import { Observable } from 'rxjs';
import { HttpService, Urls } from '../services/http.service';
import { StorageService, StorageValue } from '../services/storage.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmActionDialogComponent } from '../components/popups/confirm-action-dialog/confirm-action-dialog.component';
import { ModalResponse } from '../models/modal-response.model';
import { LoginModel } from '../models/login.model';
import { UserSessionModel } from '../models/user-session.model';
import { TranslationPipe } from '../pipes/translation.pipe';

@Injectable()
export class AuthService {
  // private user: Observable<firebase.User>;
  // private userDetails: firebase.User = null;

  constructor(
    // public _firebaseAuth: AngularFireAuth,
    public router: Router,
    private httpService: HttpService,
    private modalService: NgbModal,
    private translationPipe: TranslationPipe
  ) {
    // this.user = _firebaseAuth.authState;
    // this.user.subscribe(
    //   (user) => {
    //     if (user) {
    //       this.userDetails = user;
    //     }
    //     else {
    //       this.userDetails = null;
    //     }
    //   }
    // );
  }

  // signupUser(email: string, password: string) {
  //   //your code for signing up the new user
  // }

  async signinUser(login: LoginModel) { /// set model to Login
    let result = await this.httpService.post<any>(Urls.UsersessionLogin, login).toPromise();
    if(result && result.isSuccess) {
      StorageService.set(StorageValue.UserSession, result.value);
      this.router.navigate(['/campaigns']);
    }
  }

  logout() {
    // this._firebaseAuth.signOut();
    let modalRef = this.modalService.open(ConfirmActionDialogComponent);
    modalRef.componentInstance.message = this.translationPipe.transform("admin_do_you_really_want_to_log_out");
    modalRef.result.then(async (result: ModalResponse<any>) => {
      if(result.isSuccess) {
        await this.httpService.post<any>(Urls.UsersessionLogout).toPromise();
        StorageService.delete(StorageValue.UserSession);
        this.router.navigate(['/']);
      }
    },
    (reason: any) => {})
    .catch(error => {
      console.error('Modal error:', error);
    })
  }

  isAuthenticated() {
    let userSession: UserSessionModel = StorageService.get(StorageValue.UserSession);
    if(userSession !== null && userSession.token !== null && !userSession.isExpired) {
      return true;
    } else {
      return false;
    }
  }

}
