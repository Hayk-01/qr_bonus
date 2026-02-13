import { Component, ViewChild } from '@angular/core';
import { NgForm, UntypedFormGroup, UntypedFormControl, Validators } from '@angular/forms';
// import { Router, ActivatedRoute } from "@angular/router";
import { AuthService } from 'app/shared/auth/auth.service';
import { LanguageService } from 'app/shared/services/language.service';
// import { NgxSpinnerService } from "ngx-spinner";


@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})

export class LoginPageComponent {

  loginFormSubmitted = false;

  // isLoginFailed = false;

  loginForm = new UntypedFormGroup({
    userName: new UntypedFormControl('', [Validators.required]),
    password: new UntypedFormControl('', [Validators.required]),
    // rememberMe: new UntypedFormControl(true)
  });

  public languages = LanguageService.languages;
  public selectedLanguageId = LanguageService.GetLanguage().id;
  public selectedLanguage = this.languages.find(x => x.id === this.selectedLanguageId);

  constructor(
    private authService: AuthService,
    // private router: Router,
    // private spinner: NgxSpinnerService,
    // private route: ActivatedRoute
  ) {
  }

  get lf() {
    return this.loginForm.controls;
  }

  // On submit button click
  onSubmit() {
    this.loginFormSubmitted = true;
    if (this.loginForm.invalid) {
      return;
    }
    // this.authService.signinUser(this.loginForm.value);
    this.authService.signinUser({userName: this.loginForm.value.userName, password: this.loginForm.value.password});
  }

  public selectLanguage(language: any) {
    this.selectedLanguage = language;
    LanguageService.SetLanguage(language.id);
  }

}
