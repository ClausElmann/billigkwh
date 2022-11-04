import { ChangeDetectorRef, Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { SmartDeviceDto } from "@apiModels/smartDeviceDto";
import { SmartDeviceService } from "@core/services/smartdevice.service";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { MenuItem, MessageService } from "primeng/api";
import { switchMap, take } from "rxjs";

@UntilDestroy()
@Component({
  selector: "app-customer-edit",
  templateUrl: "./device-detalje.component.html",
  styles: [".red { color: red; }"],
  providers: [MessageService]
})
export class DeviceDetaljeComponent implements OnInit {
  items: MenuItem[] = [];

  activeItem: MenuItem;
  public print?: SmartDeviceDto;

  constructor(private activeRoute: ActivatedRoute, private deviceService: SmartDeviceService, private cd: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.items = [
      { label: "Rediger", icon: "pi pi-fw pi-pencil", routerLink: "edit" },
      { label: "Stykliste", icon: "pi pi-fw pi-bars", routerLink: "parts" },
      // { label: "Komponenter", icon: "pi pi-fw pi-qrcode", routerLink: "components" },
      // { label: "Billeder", icon: "pi pi-fw pi-images", routerLink: "images" },
      { label: "E-mails", icon: "pi pi-fw pi-envelope", routerLink: "emails" }
    ];
    this.activeItem = this.items[0];

    this.activeRoute.params
      .pipe(
        untilDestroyed(this),
        take(1),
        switchMap(params => {
          return this.deviceService.getSmartDevice(+params["id"]);
        })
      )
      .subscribe(data => {
        if (data) {
          this.print = data;

          this.items = [
            { label: "Rediger", icon: "pi pi-fw pi-pencil", routerLink: "edit" },
            { label: "E-mails", icon: "pi pi-fw pi-envelope", routerLink: "emails" }
          ];
        }
        this.cd.detectChanges();
      });
  }
}
