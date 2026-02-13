import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { FormGroup, FormControl, Validators, FormArray } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PageType } from 'app/shared/enums/page-type.enum';
import { FormErrorMessageProviderService } from 'app/shared/services/form-error-message-provider.service';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { ToastrService } from 'ngx-toastr';
import { RegionModel } from 'app/shared/models/region.model';
import { LanguagesEnum } from 'app/shared/enums/languages.enum';
import { StorageService, StorageValue } from 'app/shared/services/storage.service';

@Component({
  selector: 'app-region-details',
  templateUrl: './region-details.component.html',
  styleUrls: ['./region-details.component.scss']
})
export class RegionDetailsComponent implements OnInit {
  public id = this.activatedRoute.snapshot.paramMap.get("id");

  public type = this.activatedRoute.snapshot.paramMap.get("type");

  public PageType = PageType;

  public touched:boolean = false;

  public data: RegionModel = {} as RegionModel;

  public form: FormGroup = new FormGroup({
    id: new FormControl(null),
    areaCode: new FormControl(null, [Validators.required]),
    translations: new FormArray([
      new FormGroup({
        languageId: new FormControl(LanguagesEnum.English),
        name: new FormControl(null, [Validators.required])
      }),
      new FormGroup({
        languageId: new FormControl(LanguagesEnum.Georgian),
        name: new FormControl(null, [Validators.required])
      }),
    ]),
  })

  public get translations() {
    return this.form.get("translations") as FormArray;
  }

  constructor(
    public location: Location,
    public toastr: ToastrService,
    private activatedRoute: ActivatedRoute,
    private http: HttpService,
    public formErrorMessageProviderService: FormErrorMessageProviderService
  ) { }

  ngOnInit(): void {
    if(this.type === PageType.edit) {
      this.getData();
    }
  }

  private async getData() {
    let res = await this.http.get<RegionModel>(Urls.Region, {}, parseInt(this.id)).toPromise();
    if(res && res.isSuccess) {
      this.data = res.value;
      this.form.patchValue(res.value);
    }
  }

  public get name() {
    if(this.data && this.data.translations && this.data.translations.length > 0) {
      return this.data.translations.find(x => x.languageId === StorageService.get(StorageValue.LanguageId)).name;
    }
  }

  public async onSubmit() {
    this.touched = true;
    if(this.form.invalid) {
      return;
    }

    let reqBody = this.form.value as RegionModel

    let res;
    if(this.type === PageType.new) {
      res = await this.http.post(Urls.Region, reqBody).toPromise();
    } else {
      res = await this.http.put(Urls.Region, reqBody, reqBody.id).toPromise();
    }

    if(res && res.isSuccess) {
      this.toastr.success("Success");
      this.location.back();
    }
  }

}
