import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { QrCodeZoomedComponent } from './qr-code-zoomed/qr-code-zoomed.component';

declare var QRCode;

@Component({
  selector: 'app-qr-code-generator',
  templateUrl: './qr-code-generator.component.html',
  styleUrls: ['./qr-code-generator.component.scss']
})
export class QrCodeGeneratorComponent implements OnInit {
  public id: string = 'initial-id';

  private qrString: string | null = null;

  @Input() set value(value: string) {
    if(!value) return;

    this.qrString = value;

    this.id = value;
    let qr = document.getElementById(this.id);

    if(qr) {
      qr.remove();
      const newQr = document.createElement("div");
      newQr.id = this.id;
      document.body.appendChild(newQr);
    }

    this.cdr.detectChanges();
    new QRCode(document.getElementById(this.id), {
      text: value,
      width: 100,
      height: 100,
      colorDark: "#000000",
      colorLight: "#ffffff",
      correctLevel: QRCode.CorrectLevel.H,
    });
  }

  constructor(
    private modalService: NgbModal,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {}

  public openQrZoomedPopup() {
    if(!this.qrString) return;
    let modalRef = this.modalService.open(QrCodeZoomedComponent);
    modalRef.componentInstance.qrString = this.qrString;
    modalRef.componentInstance.id = 'zoomed' + this.qrString;
  }

}
