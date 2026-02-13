import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalResponse } from 'app/shared/models/modal-response.model';
import { ToastrService } from 'ngx-toastr';

declare var Html5QrcodeScanner;

@Component({
  selector: 'app-scanner',
  templateUrl: './scanner.component.html',
  styleUrls: ['./scanner.component.scss']
})
export class ScannerComponent implements OnInit {

  constructor(
    public activeModal: NgbActiveModal,
    public toastr: ToastrService,
  ) { }

  ngOnInit(): void {
    this.scanner();
  }

  private scanner() {

    let self = this;

    const scanner = new Html5QrcodeScanner('reader', {
        // Scanner will be initialized in DOM inside element with id of 'reader'
        qrbox: {
            width: 180,
            height: 180,
        },  // Sets dimensions of scanning box (set relative to reader element width)
        fps: 15, // Frames per second to attempt a scan
        aspectRatio: 3,
        // videoConstraints: {
        //   facingMode: "environment",
        //   width: { ideal: 1280 },
        //   height: { ideal: 720 }
        // }
    });


    scanner.render(success, error);
    // Starts scanner

    function success(result) {

      self.goBack(true, result);
        // document.getElementById('result').innerHTML = `
        // <h2>Success!</h2>
        // <p><a href="${result}">${result}</a></p>
        // `;
        // // Prints result as a link inside result element

      scanner.clear();
      // Clears scanning instance

      document.getElementById('reader').remove();
      // Removes reader element from DOM since no longer needed

    }

    function error(err) {
        this.toastr.error("Please scan again");
        self.goBack(false);
    }
  }

  public goBack(isSuccess:boolean, data: string = '') {
    let result: ModalResponse<string> = {isSuccess, data};
    this.activeModal.close(result)
  }

}
