import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { ElTavleDto } from "@apiModels/elTavleDto";
import { EltavleService } from "@core/services/eltavle.service";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { MenuItem, MessageService } from "primeng/api";
import { switchMap, take } from "rxjs";

@UntilDestroy()
@Component({
  selector: "app-customer-edit",
  templateUrl: "./komponent-detalje.component.html",
  providers: [MessageService]
})
export class KomponentDetaljeComponent implements OnInit {
  items: MenuItem[];

  activeItem: MenuItem;
  public eltavle?: ElTavleDto;

  constructor(private activeRoute: ActivatedRoute, private eltavleService: EltavleService, private cd: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.items = [
      { label: "Rediger", icon: "pi pi-fw pi-pencil", routerLink: "edit" },
      { label: "Komponenter", icon: "pi pi-fw pi-bars", routerLink: "parts" },
      { label: "Opbygning", icon: "pi pi-fw pi-qrcode", routerLink: "components" },
      { label: "Billeder", icon: "pi pi-fw pi-images", routerLink: "images" },
      { label: "E-mails", icon: "pi pi-fw pi-envelope", routerLink: "emails" }
    ];
    this.activeItem = this.items[0];

    this.activeRoute.params
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

          if (this.eltavle.typeID !== 2) {
            this.items = [
              { label: "Rediger", icon: "pi pi-fw pi-pencil", routerLink: "edit" },
              //  { label: "Komponenter", icon: "pi pi-fw pi-bars", routerLink: "parts },
              //  { label: "Opbygning", icon: "pi pi-fw pi-qrcode", routerLink: "components" },
              //  { label: "Billeder", icon: "pi pi-fw pi-images", routerLink: "images" },
              { label: "E-mails", icon: "pi pi-fw pi-envelope", routerLink: "emails" }
            ];
          }
        }
        this.cd.detectChanges();
      });
  }
}
