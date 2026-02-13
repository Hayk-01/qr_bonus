import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PageType } from 'app/shared/enums/page-type.enum';
import { UserRole } from 'app/shared/enums/user-role.enum';
import { UserModel } from 'app/shared/models/user.model';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { FormErrorMessageProviderService } from 'app/shared/services/form-error-message-provider.service';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { Validator } from 'app/shared/validators/validators';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.scss']
})
export class UserDetailsComponent implements OnInit {

  public id = this.activatedRoute.snapshot.paramMap.get("id");

  public type = this.activatedRoute.snapshot.paramMap.get("type");

  public PageType = PageType;

  public touched:boolean = false;

  public data: UserModel = {} as UserModel;

  private validator = new Validator();

  public roles: Array<{id:number, name: string}> = [
    {
      id: UserRole.admin,
      name: this.tanslationPipe.transform("admin_role_name_admin")
    },
    {
      id: UserRole.courier,
      name: this.tanslationPipe.transform("admin_role_name_courier")
    }
  ]

  public form: FormGroup = new FormGroup({
    id: new FormControl(null),
    firstName: new FormControl(null, [Validators.required, this.validator.noWhiteSapaceValidator3()]),
    lastName: new FormControl(null, [Validators.required, this.validator.noWhiteSapaceValidator3()]),
    userName: new FormControl(null, [Validators.required, this.validator.whiteSpaceIncludesValidator()]),
    password: new FormControl(null, this.type === PageType.new ? Validators.required : null),
    role: new FormControl(null, [Validators.required])
  })

  constructor(
    public location: Location,
    public toastr: ToastrService,
    private activatedRoute: ActivatedRoute,
    private http: HttpService,
    public formErrorMessageProviderService: FormErrorMessageProviderService,
    private tanslationPipe: TranslationPipe
  ) { }

  ngOnInit(): void {
    if(this.type === PageType.edit) {
      this.getData();
    }
  }

  private async getData() {
    let res = await this.http.get<UserModel>(Urls.User, {}, parseInt(this.id)).toPromise();
    if(res && res.isSuccess) {
      this.data = res.value;
      this.form.patchValue(res.value);
    }
  }

  public async onSubmit() {
    this.touched = true;
    if(this.form.invalid) {
      return;
    }

    let reqBody = this.form.value as UserModel

    let res;
    if(this.type === PageType.new) {
      res = await this.http.post(Urls.User, reqBody).toPromise();
    } else {
      res = await this.http.put(Urls.User, reqBody, reqBody.id).toPromise();
    }

    if(res && res.isSuccess) {
      this.toastr.success("Success");
      this.location.back();
    }
  }

}
