import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ScannerComponent } from '../scanner/scanner.component';
import { ModalResponse } from 'app/shared/models/modal-response.model';
import { Router } from '@angular/router';
import { QrScannerDevicePopupComponent } from '../qr-scanner-device-popup/qr-scanner-device-popup.component';

@Component({
  selector: 'app-qr-scan',
  templateUrl: './qr-scan.component.html',
  styleUrls: ['./qr-scan.component.scss']
})
export class QrScanComponent implements OnInit {
  public noData:boolean = false;

  constructor(
    private modalService: NgbModal,
    private router: Router
  ) { }

  ngOnInit(): void {}

  public openScanner() {
    let modalRef = this.modalService.open(ScannerComponent);
    modalRef.result.then((result: ModalResponse<string>) => {
      if(result.isSuccess && result.data) {
        this.setQrCodeValueAndNavigate(result.data);
      } else {
        this.noData = true;
      }
    },
    (reason: any) => {})
    .catch(error => {
      console.error('Modal error:', error);
    })
  }

  public setQrCodeValueAndNavigate(qrCode: string) {
    this.router.navigate(['scan', 'result'], {
      queryParams: {value: qrCode},
      queryParamsHandling: 'merge',
    });
  }

  public openQrSCanDevice() {
    let modalRef = this.modalService.open(QrScannerDevicePopupComponent);
    modalRef.result.then((result:ModalResponse<string>) => {
      if(result.isSuccess) {
        this.setQrCodeValueAndNavigate(result.data);
      }
    })
  }

}
