import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BannerModel } from 'app/shared/models/banner.model';
import { Location } from '@angular/common';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { LanguagesEnum } from 'app/shared/enums/languages.enum';
import { PageType } from 'app/shared/enums/page-type.enum';
import { FileModel } from 'app/shared/models/file.model';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { HttpService, Urls } from 'app/shared/services/http.service';
import { ToastrService } from 'ngx-toastr';
import { Validator } from 'app/shared/validators/validators';
import { FormErrorMessageProviderService } from 'app/shared/services/form-error-message-provider.service';
import { StorageService, StorageValue } from 'app/shared/services/storage.service';
import { DropDownDataModel } from 'app/shared/control-value-accessors/dropdown/dropdown.component';

@Component({
  selector: 'app-banner-details',
  templateUrl: './banner-details.component.html',
  styleUrls: ['./banner-details.component.scss']
})
export class BannerDetailsComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;

  public id = this.activatedRoute.snapshot.paramMap.get("id");

  public type = this.activatedRoute.snapshot.paramMap.get("type");

  public PageType = PageType;

  public touched:boolean = false;

  public data: BannerModel = {} as BannerModel;

  public addPhoto: FileModel | null = null;

  public image: string = null;

  private validator = new Validator();

  public regionDowpDown: DropDownDataModel = {
    source: Urls.Region
  }

  public form: FormGroup = new FormGroup({
    id: new FormControl(null),
    expirationDate: new FormControl(null),
    translations: new FormArray([
      new FormGroup({
        languageId: new FormControl(LanguagesEnum.English),
        name: new FormControl(null, [this.validator.noWhiteSapaceValidator3()]),
        description: new FormControl(null, [this.validator.noWhiteSapaceValidator3()])
      }),
      new FormGroup({
        languageId: new FormControl(LanguagesEnum.Georgian),
        name: new FormControl(null, [this.validator.noWhiteSapaceValidator3()]),
        description: new FormControl(null, [this.validator.noWhiteSapaceValidator3()])
      }),
    ]),
    link: new FormControl(null),
    regionId: new FormControl(null, [Validators.required])
  })

  constructor(
    public location: Location,
    public toastr: ToastrService,
    private activatedRoute: ActivatedRoute,
    private http: HttpService,
    private translationPipe: TranslationPipe,
    public formErrorMessageProviderService: FormErrorMessageProviderService
  ) { }

  ngOnInit(): void {
    if(this.type === PageType.edit) {
      this.getData();
    }
  }

  public get name() {
    if(this.data && this.data.translations && this.data.translations.length > 0) {
      return this.data.translations.find(x => x.languageId === StorageService.get(StorageValue.LanguageId)).name;
    }
  }

  private async getData() {
    let res = await this.http.get<BannerModel>(Urls.Banner, {}, parseInt(this.id)).toPromise();
    if(res && res.isSuccess) {
      this.data = res.value;
      this.form.patchValue(res.value);
      this.image = this.form.value.link;

      this.data.translations.forEach(item => {
        if(item.languageId === LanguagesEnum.English) {
          this.translations.controls[0].patchValue(item);
        }
        if(item.languageId === LanguagesEnum.Georgian) {
          this.translations.controls[1].patchValue(item);
        }
      })

    }
  }

  public get translations() {
    return this.form.get("translations") as FormArray;
  }

  public uploadImage(event) {
    let file = event.target.files[0];
    let type = event.target.files[0].type.split('/')[1];
    // let name = event.target.files[0].name.split('.')[0];
    // let size = event.target.files[0].size;

    if(type !== 'jpeg' && type !== 'jpg' && type !== 'png' && type !== 'svg') {
      this.toastr.warning("wrong format");
      this.fileInput.nativeElement.value = '';
      return;
    }
    if(file.size > 2000000) {
      this.toastr.warning("file is to big");
      this.fileInput.nativeElement.value = '';
      return;
    }

    let reader = new FileReader();
    reader.onload = (event: any) => {

      //// res is for back-end
      // let res = event.target.result.split(',')[1];
      this.addPhoto = {
        data: event.target.result.split(',')[1],
        type,
        // name
      }

      //// event.target.resultis for web preview
      this.image = event.target.result;

      this.fileInput.nativeElement.value = '';
    };
    reader.readAsDataURL(file);
  }

  public async onSubmit() {
    this.touched = true;
    if(this.form.invalid) {
      return;
    }

    let reqBody: BannerModel = this.form.value;
    if(this.addPhoto) {
      reqBody.addPhoto = this.addPhoto;
      delete reqBody.link;
    }

    reqBody.translations = reqBody.translations.filter((t: any) => t.name !== null && t.description !== null);

    let res;
    if(this.type === PageType.new) {
      res = await this.http.post(Urls.Banner, reqBody).toPromise();
    } else {
      res = await this.http.put(Urls.Banner, reqBody, reqBody.id).toPromise();
    }

    if(res && res.isSuccess) {
      this.toastr.success(this.translationPipe.transform("admin_success"));
      this.location.back();
    }
  }

}
