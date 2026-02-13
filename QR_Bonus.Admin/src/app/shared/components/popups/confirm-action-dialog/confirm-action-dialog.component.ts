import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslationPipe } from '../../../pipes/translation.pipe';

@Component({
  selector: 'app-confirm-action-dialog',
  templateUrl: './confirm-action-dialog.component.html',
  styleUrls: ['./confirm-action-dialog.component.scss']
})
export class ConfirmActionDialogComponent implements OnInit {
  public message: string = this.TranslationPipe.transform("admin_do_you_want_to_perform_this_action");

  constructor(
    public activeModal: NgbActiveModal,
    private TranslationPipe: TranslationPipe
  ) { }

  ngOnInit(): void {}

}
