import { Component, OnInit } from '@angular/core';
import { DropDownDataModel } from 'app/shared/control-value-accessors/dropdown/dropdown.component';
import { PushNotificationModel } from 'app/shared/models/push-notification.model';
import { TranslationPipe } from 'app/shared/pipes/translation.pipe';
import { Urls } from 'app/shared/services/http.service';
import { HttpService } from '../../../shared/services/http.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-push-notifications',
  templateUrl: './push-notifications.component.html',
  styleUrls: ['./push-notifications.component.scss']
})
export class PushNotificationsComponent implements OnInit {

  public pushNotification: PushNotificationModel = {
    regionId: null,
    title: null,
    message: null
  }

  public regionDowpDown: DropDownDataModel = {
    source: Urls.Region
  }

  constructor(
    private httpService: HttpService,
    public toastr: ToastrService,
    private translationPipe: TranslationPipe,
  ) { }

  ngOnInit(): void {}

  public async sendNofication() {
    if (this.formInvalid()) return;
    const res = await this.httpService.post<PushNotificationModel>(Urls.PushNotification, this.pushNotification).toPromise()
    if (res && res.isSuccess) {
      this.toastr.success(this.translationPipe.transform("admin_success"));
      this.pushNotification = {
        regionId: null,
        title: null,
        message: null
      }
    }
  }

  public formInvalid() {
    return !this.pushNotification.regionId || !this.pushNotification.title || this.pushNotification.title.trim() === '' || !this.pushNotification.message || this.pushNotification.message.trim() === ''
  }

}
