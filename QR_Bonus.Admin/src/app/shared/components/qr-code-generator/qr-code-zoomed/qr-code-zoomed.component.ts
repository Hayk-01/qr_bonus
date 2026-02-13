import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

declare var QRCode;

@Component({
  selector: 'app-qr-code-zoomed',
  templateUrl: './qr-code-zoomed.component.html',
  styleUrls: ['./qr-code-zoomed.component.scss']
})
export class QrCodeZoomedComponent implements OnInit, AfterViewInit {

  public qrString: string| null = null;

  public id: string = 'initial-zoom-id';

  constructor(
    public activeModal: NgbActiveModal,
  ) { }

  ngAfterViewInit(): void {
    if(this.qrString) {
      this.generateQr();
    }
  }

  ngOnInit(): void {}

  private generateQr() {
    new QRCode(document.getElementById(this.id), {
      text: this.qrString,
      colorDark: "#000000",
      colorLight: "#ffffff",
      correctLevel: QRCode.CorrectLevel.H,
    });
  }

}
