import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalResponse } from 'app/shared/models/modal-response.model';

@Component({
  selector: 'app-qr-scanner-device-popup',
  templateUrl: './qr-scanner-device-popup.component.html',
  styleUrls: ['./qr-scanner-device-popup.component.scss']
})
export class QrScannerDevicePopupComponent implements OnInit {

  constructor(
    public activeModal: NgbActiveModal
  ) { }

  ngOnInit(): void {
  }

  onResult(value: string) {
    let result: ModalResponse<string> = {
      isSuccess: true, 
      data: value
    };
    this.activeModal.close(result);
  }

}
