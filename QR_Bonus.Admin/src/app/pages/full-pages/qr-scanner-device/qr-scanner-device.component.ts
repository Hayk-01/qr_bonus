import { AfterViewInit, Component, ElementRef, EventEmitter, HostListener, OnDestroy, OnInit, Output, Renderer2, ViewChild } from '@angular/core';

@Component({
  selector: 'app-qr-scanner-device',
  templateUrl: './qr-scanner-device.component.html',
  styleUrls: ['./qr-scanner-device.component.scss']
})
export class QrScannerDeviceComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('emptyElement') emptyElement: ElementRef;

  private scannedCode: string = "";

  private typingTimer: any = null;

  @Output() result: EventEmitter<string> = new EventEmitter<string>();

  @HostListener('window:keypress', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if(event.key === "Enter") {
      this.emitOutput();
      return;
    }

    if(event.key !== "Shift" && event.key !== "Control" && event.key !== "Alt" && event.key !== "Meta") {
        this.scannedCode += event.key;
    }

    clearTimeout(this.typingTimer);

    this.typingTimer = setTimeout(() => {
      this.emitOutput();
    }, 500);
  }
  
  constructor(
    private renderer: Renderer2
  ) { }

  ngOnInit(): void { }

  ngAfterViewInit(): void {
    this.renderer.selectRootElement(this.emptyElement.nativeElement).click();
  }

  ngOnDestroy(): void {
    clearTimeout(this.typingTimer);
  }

  private emitOutput() {
    if(this.scannedCode.trim() === '' || this.scannedCode === null || this.scannedCode === undefined) return;
    if(this.scannedCode.startsWith('\\000026')) {
      this.scannedCode = this.scannedCode.slice(7);
    }
    this.result.emit(this.scannedCode);
    this.scannedCode = "";
  }

}
