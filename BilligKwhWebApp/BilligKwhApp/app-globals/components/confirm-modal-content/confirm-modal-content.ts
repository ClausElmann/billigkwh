import { Component, OnInit } from "@angular/core";
import { DynamicDialogConfig, DynamicDialogRef } from "primeng/dynamicdialog";

@Component({
  selector: "confirm-modal-content",
  templateUrl: "./confirm-modal-content.html",
  styleUrls: ["./confirm-modal-content.scss"]
})
export class ConfirmModalComponent implements OnInit {

  public buttonOkText: string;
  public buttonCancelText: string;
  public headerText: string;
  public confirmLine1Text: string;
  public confirmLine2Text: string;
  public confirmLine3Text: string;

  constructor(public ref: DynamicDialogRef, public config: DynamicDialogConfig) { }

  ngOnInit() {
    this.headerText = this.config.data.headerText;
    this.buttonOkText = this.config.data.buttonOkText;
    this.buttonCancelText = this.config.data.buttonCancelText;
    this.confirmLine1Text = this.config.data.confirmLine1Text;
    this.confirmLine2Text = this.config.data.confirmLine2Text;
    this.confirmLine3Text = this.config.data.confirmLine3Text;
  }

}
