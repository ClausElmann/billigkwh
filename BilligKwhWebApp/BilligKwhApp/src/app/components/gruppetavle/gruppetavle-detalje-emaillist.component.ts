import { Component, OnInit } from "@angular/core";
import { EmailModel } from "@apiModels/EmailModel";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { ConfirmationService, MessageService, SortEvent } from "primeng/api";
import { finalize, map, Observable, ReplaySubject, tap } from "rxjs";
import { ActivatedRoute, Router } from "@angular/router";
import { CustomerService } from "@core/services/customer.service";
import { BiLocalizationHelperService } from "@core/utility-services/bi-localization-helper.service";
import moment from "moment";
import { TableColumnPrimeNg } from "@shared/interfaces-and-enums/TableColumnPrimeNg";

export interface EmailModelExt extends EmailModel {
  dateSentForSort?: moment.Moment;
  dateSent?: string;
}

export interface TableColumnPrimeNgExt extends TableColumnPrimeNg {
  sortField?: string;
}

@UntilDestroy()
@Component({
  templateUrl: "gruppetavle-detalje-emaillist.component.html",
  styleUrls: ["./gruppetavle-detalje-emaillist.component.scss"],
  providers: [MessageService, ConfirmationService]
})
export class GruppetavleDetaljeEmailsComponent implements OnInit {
  public loading = true;

  public emails: Array<EmailModel> = [];
  public emails$: Observable<Array<EmailModel>>;
  private columns = new ReplaySubject<Array<TableColumnPrimeNgExt>>(1);
  public columns$ = this.columns.asObservable();
  private globalFilterFields = new ReplaySubject<Array<string>>(1);
  public globalFilterFields$ = this.globalFilterFields.asObservable();

  selectedValue: string;

  selectedEmail: EmailModel;

  cols: any[];

  showDeleted: boolean;

  displayEmailDialog: boolean;

  text: string;

  showEmail() {
    this.displayEmailDialog = true;
  }

  constructor(private customerService: CustomerService, private activeRoute: ActivatedRoute, private router: Router, private localizor: BiLocalizationHelperService) {}

  ngOnInit() {
    this.text = "<h2>Vælg en email i listen for at se indholdet</h2>";

    this.initializeEmails();

    this.initColumns();
  }

  onRowSelect(event) {
    // this.text = this.selectedEmail.body.substring(this.selectedEmail.body.lastIndexOf("<!--contentstart -->") + 20, this.selectedEmail.body.lastIndexOf("<!--contentend -->"));
    this.text = this.selectedEmail.body;
    this.showEmail();
  }

  public onCreateNewEmail() {
    this.router.navigate([0, "main"], { relativeTo: this.activeRoute });
  }

  private initColumns() {
    this.globalFilterFields.next(["subject", "body", "toName", "toEmail"]);
    this.columns.next([
      { field: "dateSent", header: "Sendt", sortField: "dateSentForSort" },
      { field: "subject", header: "Emne" },
      { field: "toName", header: "Til navn" },
      { field: "toEmail", header: "Til" },
      { field: "fromName", header: "Fra navn" },
      { field: "fromEmail", header: "Fra" }
    ]);
  }

  private initializeEmails() {
    this.emails$ = this.customerService.getTavleEmails(this.activeRoute.parent.snapshot.params.id).pipe(
      tap((data: Array<EmailModelExt>) => {
        data.forEach(element => {
          element.dateSent = this.localizor.localizeDateTime(element.dateSentUtc);
          element.dateSentForSort = moment(element.dateSentUtc);
        });
      }),
      untilDestroyed(this),
      finalize(() => {
        this.loading = false;
      })
    );
  }

  customSort(event: SortEvent) {
    this.columns$.pipe(map(columns => columns.find(f => f.field === event.field))).subscribe(col => {
      let value1: any, value2: any;
      event.data.sort((data1, data2) => {
        if (col && col.sortField) {
          value1 = +data1[col.sortField];
          value2 = +data2[col.sortField];
        } else {
          value1 = data1[event.field];
          value2 = data2[event.field];
        }

        let result = null;
        if (value1 == null && value2 != null) {
          result = -1;
        } else if (value1 != null && value2 == null) {
          result = 1;
        } else if (value1 == null && value2 == null) {
          result = 0;
        } else if (typeof value1 === "string" && typeof value2 === "string") {
          result = value1.localeCompare(value2);
        } else {
          result = value1 < value2 ? -1 : value1 > value2 ? 1 : 0;
        }
        return event.order * result;
      });
    });
  }
}
