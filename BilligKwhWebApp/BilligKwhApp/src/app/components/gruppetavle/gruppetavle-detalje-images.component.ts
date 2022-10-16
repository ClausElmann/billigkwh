import { Component, OnInit } from "@angular/core";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService } from "primeng/api";
import { finalize, Observable } from "rxjs";
import { ActivatedRoute } from "@angular/router";
import { EltavleService } from "@core/services/eltavle.service";
import { DokumentDto } from "@apiModels/dokumentDto";
import { DomSanitizer, SafeUrl } from "@angular/platform-browser";

@UntilDestroy()
@Component({
  selector: "app-gruppetavle-images",
  templateUrl: "./gruppetavle-detalje-images.component.html",
  providers: [MessageService, ConfirmationService]
})
export class GruppetavleImagesComponent implements OnInit {
  public loading = true;

  public imageSpinner = false;
  public displayImageDialog: boolean;

  //customer: CustomerModel;
  public images: Array<DokumentDto> = [];
  public images$: Observable<Array<DokumentDto>>;

  selectedImage: DokumentDto;

  cols: any[];

  showDeleted: boolean;

  constructor(private eltavleService: EltavleService, private domSanitizer: DomSanitizer, private activeRoute: ActivatedRoute) {}

  ngOnInit() {
    // this.route.parent.data.subscribe(data => {
    //   this.customer = data["resolvedData"].customer;
    // });
    this.showDeleted = false;

    this.initializeUsers();

    this.cols = [
      { field: "oprettet", header: "Oprettet" },
      { field: "fuldtNavn", header: "Bruger" },
      { field: "brugernavn", header: "Brugernavn" }
      // { field: "timeZoneId", header: "timeZoneId" },
      // { field: "languageId", header: "languageId" }
    ];
  }

  onRowSelect(event) {
    this.displayImageDialog = true;
    //this.router.navigate(["users", event.data.id, "edit"]);
  }

  public onShowActiveChange(checked: boolean) {
    if (checked) {
      this.showDeleted = true;
      this.initializeUsers();
    } else {
      this.showDeleted = false;
      this.initializeUsers();
    }
  }

  private initializeUsers() {
    this.images$ = this.eltavleService.getEltavleDokumenter(this.activeRoute.parent.snapshot.params.id).pipe(
      untilDestroyed(this),
      finalize(() => (this.loading = false))
    );
  }

  public get imageUrl(): SafeUrl {
    const image = this.selectedImage?.base64Data;
    if (image) return this.domSanitizer.bypassSecurityTrustUrl("data:image/jpg;base64, " + image);
  }

  showPdfDialog() {
    this.displayImageDialog = true;
  }
}
