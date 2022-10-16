import { Component, OnInit, ChangeDetectorRef, HostBinding, ChangeDetectionStrategy } from "@angular/core";
import { FormGroup, FormControl, ValidationErrors, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { switchMap, take } from "rxjs/operators";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService, SelectItem } from "primeng/api";
import { ElTavleDto } from "@apiModels/elTavleDto";
import { EltavleService } from "@core/services/eltavle.service";
import { CustomValidators } from "@globals/classes/custom-validators";
import { BiCustomAnimations } from "@shared/classes/BICustomAnimations";
import { CustomerService } from "@core/services/customer.service";
import { CustomerModel } from "@apiModels/customerModel";
import { UserService } from "@core/services/user.service";
import { UserModel } from "@apiModels/UserModel";

@UntilDestroy()
@Component({
  selector: "app-komponent-edit",
  templateUrl: "./komponent-detalje-edit.component.html",
  providers: [MessageService, ConfirmationService],
  changeDetection: ChangeDetectionStrategy.OnPush,
  animations: [BiCustomAnimations.fadeInDown, BiCustomAnimations.fadeIn]
})
export class KomponentDetaljeEditComponent implements OnInit {
  public eltavle?: ElTavleDto;
  public mainForm: FormGroup;
  public showFormErrorMessage: boolean;
  public pageTitle = "Eltavle redigering";
  public loading = false;
  public errorMessage = "";
  public customers: Array<CustomerModel>;
  public users: Array<UserModel>;
  public oprettetAfBrugere: Array<SelectItem>;

  public valgtKunde: CustomerModel;
  public valgtBruger: UserModel;

  public deleteOrRecreate = "Slet";
  public createOrSave = "Opret";
  public updateEconomic = "Gem i e-conomic";
  deleteDialog = false;
  takForBestillingDialog = false;
  viHarNuSendtDinTavleDialog = false;
  sendFakturaMailDialog = false;

  flytKundeDialog = false;

  public src: Blob;
  public pdfViewerVisible = false;
  public pdfSpinner = false;
  public displayPdfDialog: boolean;

  @HostBinding("@fadeInDown") get fadeInHost() {
    return true;
  }

  constructor(
    private activeRoute: ActivatedRoute,
    private router: Router,
    private eltavleService: EltavleService,
    private cd: ChangeDetectorRef,
    private messageService: MessageService,
    private userService: UserService,
    private customerService: CustomerService
  ) {}

  ngOnInit() {
    this.activeRoute.parent.params
      .pipe(
        untilDestroyed(this),
        take(1),
        switchMap(params => {
          return this.eltavleService.getEltavle(+params["id"]);
        })
      )
      .subscribe(data => {
        if (data) {
          this.eltavle = data;
          if (this.eltavle.slettet == true) this.deleteOrRecreate = "Genskab";
          this.createOrSave = "Gem";
          this.userService
            .getUsersByCustomer(this.eltavle.kundeID, false)
            .pipe(take(1))
            .subscribe(c => {
              this.oprettetAfBrugere = c.map(c => <SelectItem>{ value: c.id, label: c.email });
              this.initFormGroup();
              this.cd.detectChanges();
            });
        }
      });
  }

  private initFormGroup() {
    this.mainForm = new FormGroup({
      udbetaltBonus: new FormControl(this.eltavle.udbetaltBonus, [CustomValidators.digitsOnly()]),
      fragt: new FormControl(this.eltavle.fragt, [Validators.required, CustomValidators.digitsOnly()]),
      antal: new FormControl(this.eltavle.antal, [Validators.required, CustomValidators.digitsOnly()]),
      nettoPris: new FormControl(this.eltavle.nettoPris, [Validators.required, CustomValidators.digitsOnly()]),
      initialRabat: new FormControl(this.eltavle.initialRabat, [Validators.required]),
      bonusGivende: new FormControl(this.eltavle.bonusGivende, [Validators.required]),
      oprettetAfBrugerID: new FormControl(this.eltavle.oprettetAfBrugerID, [Validators.required]),
      rekvisition: new FormControl(this.eltavle.rekvisition),
      adresse: new FormControl(this.eltavle.adresse),
      kommentar: new FormControl(this.eltavle.kommentar)
    });

    if (this.eltavle.economicBookedInvoiceNumber !== null) this.mainForm.disable();
  }

  public get udbetaltBonus() {
    return this.mainForm.get("udbetaltBonus");
  }

  public get fragt() {
    return this.mainForm.get("fragt");
  }

  public get antal() {
    return this.mainForm.get("antal");
  }

  public get nettoPris() {
    return this.mainForm.get("nettoPris");
  }

  public get initialRabat() {
    return this.mainForm.get("initialRabat");
  }

  public get bonusGivende() {
    return this.mainForm.get("bonusGivende");
  }

  public get oprettetAfBrugerID() {
    return this.mainForm.get("oprettetAfBrugerID");
  }

  public get rekvisition() {
    return this.mainForm.get("rekvisition");
  }

  public get adresse() {
    return this.mainForm.get("adresse");
  }

  public get kommentar() {
    return this.mainForm.get("kommentar");
  }

  private checkAndValidateForm() {
    if (this.mainForm.invalid) {
      Object.keys(this.mainForm.controls).forEach(cName => this.mainForm.controls[cName].markAsTouched());
      this.showFormErrorMessage = true;
      return false;
    }

    this.showFormErrorMessage = false;
    return true;
  }

  getFormValidationErrors() {
    console.log("%c ==>> Validation Errors: ", "color: red; font-weight: bold; font-size:25px;");
    let totalErrors = 0;
    Object.keys(this.mainForm.controls).forEach(key => {
      const controlErrors: ValidationErrors = this.mainForm.get(key).errors;
      if (controlErrors != null) {
        totalErrors++;
        Object.keys(controlErrors).forEach(keyError => {
          console.log("Key control: " + key + ", keyError: " + keyError + ", err value: ", controlErrors[keyError]);
        });
      }
    });
    console.log("Number of errors: ", totalErrors);
  }

  deleteItem() {
    this.deleteDialog = true;
  }

  deleteRecreate(): void {
    if (this.eltavle.slettet) {
      this.recreateItem();
    } else {
      this.deleteDialog = true;
    }
  }

  recreateItem() {
    this.eltavle.slettet = false;

    this.eltavleService.updateEltavle(this.eltavle).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev genskabt"
        });

        this.mainForm.reset();

        this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/eltavler", this.eltavle.id, "edit"]));
      },
      error: err => (this.errorMessage = err)
    });
  }

  confirmDelete() {
    this.eltavle.slettet = true;
    this.eltavleService.updateEltavle(this.eltavle).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev slettet"
        });

        this.mainForm.reset();

        this.router.navigate(["/eltavler"]);
      },
      error: err => (this.errorMessage = err)
    });
  }

  saveItem() {
    if (!this.checkAndValidateForm()) return;

    this.eltavle.udbetaltBonus = this.udbetaltBonus.value;
    this.eltavle.fragt = this.fragt.value;
    this.eltavle.antal = this.antal.value;
    this.eltavle.nettoPris = this.nettoPris.value;
    this.eltavle.initialRabat = this.initialRabat.value;
    this.eltavle.bonusGivende = this.bonusGivende.value;
    this.eltavle.oprettetAfBrugerID = this.oprettetAfBrugerID.value;
    this.eltavle.rekvisition = this.rekvisition.value;
    this.eltavle.adresse = this.adresse.value;
    this.eltavle.kommentar = this.kommentar.value;

    this.eltavleService.updateEltavle(this.eltavle).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev gemt"
        });

        this.mainForm.reset();
        this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/eltavler", this.eltavle.id, "edit"]));
      },
      error: err => (this.errorMessage = err)
    });
  }

  public sendTakForBestillingMail() {
    this.takForBestillingDialog = true;
  }

  public sendViHarNuSendtDinTavleMail() {
    this.viHarNuSendtDinTavleDialog = true;
  }

  public sendFakturaMail() {
    this.sendFakturaMailDialog = true;
  }

  public flytKunde() {
    this.customerService
      .getCustomers(1, false)
      .pipe(take(1))
      .subscribe(c => {
        this.customers = c;
        this.flytKundeDialog = true;
        this.cd.detectChanges();
      });
  }

  onKundeChange(event: CustomerModel) {
    this.userService
      .getUsersByCustomer(this.valgtKunde.id, false)
      .pipe(take(1))
      .subscribe(c => {
        console.log(c);
        this.users = c;
        this.cd.detectChanges();
      });
  }

  flytTilKunde() {
    if (this.valgtBruger === null) return;
    this.eltavleService.flytKunde(this.eltavle.id, this.valgtKunde.id, this.valgtBruger.id).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Data blev flyttet"
        });

        this.mainForm.reset();
        this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/eltavler", this.eltavle.id, "edit"]));
      },
      error: err => (this.errorMessage = err)
    });
  }

  public confirmTakForBestillingMail() {
    this.eltavleService.sendTavleMail(this.eltavle.id, "TakForDinBestilling").subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Email blev sendt",
          life: 3000
        });
        this.takForBestillingDialog = false;
        this.cd.detectChanges();
      },
      error: err => (this.errorMessage = err)
    });
  }

  public confirmViHarNuSendtDinTavleMail() {
    this.eltavleService.sendTavleMail(this.eltavle.id, "ViHarNuSendtDinTavle").subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Email blev sendt",
          life: 3000
        });
        this.viHarNuSendtDinTavleDialog = false;
        this.cd.detectChanges();
      },
      error: err => (this.errorMessage = err)
    });
  }

  public confirmSendFakturaMail() {
    this.eltavleService.sendFakturaMail(this.eltavle.id).subscribe({
      next: () => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Email blev sendt",
          life: 3000
        });
        this.sendFakturaMailDialog = false;
        this.cd.detectChanges();
      },
      error: err => (this.errorMessage = err)
    });
  }

  createOrUpdateOrder(): void {
    this.eltavleService.createOrUpdateOrder(this.eltavle.id).subscribe({
      next: x => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Ordren blev oprettet"
        });

        this.mainForm.reset();
        this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/eltavler", this.eltavle.id, "edit"]));
      },
      error: err => (this.errorMessage = err)
    });
  }

  createInvoiceDraft(): void {
    this.eltavleService.createInvoiceDraft(this.eltavle.id).subscribe({
      next: x => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Ordren blev oprettet"
        });

        this.mainForm.reset();
        this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/eltavler", this.eltavle.id, "edit"]));
      },
      error: err => (this.errorMessage = err)
    });
  }

  bookInvoice(): void {
    this.eltavleService.bookInvoice(this.eltavle.id).subscribe({
      next: x => {
        this.messageService.add({
          severity: "success",
          summary: "Success",
          detail: "Ordren blev oprettet"
        });

        this.mainForm.reset();
        this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/eltavler", this.eltavle.id, "edit"]));
      },
      error: err => (this.errorMessage = err)
    });
  }

  // openInvoiceDraft(): void {
  //   this.eltavleService.createInvoiceDraft(this.eltavle.id).subscribe({
  //     next: x => {
  //       this.messageService.add({
  //         severity: "success",
  //         summary: "Success",
  //         detail: "Ordren blev oprettet"
  //       });

  //       this.mainForm.reset();
  //       this.router.navigateByUrl("/", { skipLocationChange: true }).then(() => this.router.navigate(["/eltavler", this.eltavle.id, "edit"]));
  //     },
  //     error: err => (this.errorMessage = err)
  //   });
  // }

  showFoelgeseddel() {
    this.pdfSpinner = true;
    this.eltavleService.getSentOrderPdf(this.eltavle.economicOrderNumber).subscribe(res => {
      this.src = res;
      this.pdfSpinner = false;
      this.showPdfDialog();
    });
  }

  showFakturaKladde() {
    this.pdfSpinner = true;
    this.eltavleService.getDraftInvoicePdf(this.eltavle.id).subscribe(res => {
      this.src = res;
      this.pdfSpinner = false;
      this.showPdfDialog();
    });
  }

  showFaktura() {
    this.pdfSpinner = true;
    this.eltavleService.getBookedInvoicePdf(this.eltavle.id).subscribe(res => {
      this.src = res;
      this.pdfSpinner = false;
      this.showPdfDialog();
    });
  }

  showPdfDialog() {
    this.displayPdfDialog = true;
    setTimeout(() => {
      this.pdfViewerVisible = true;
      this.cd.detectChanges();
    }, 300);
  }
}
