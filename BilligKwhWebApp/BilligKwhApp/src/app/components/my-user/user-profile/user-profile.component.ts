import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { GlobalStateAndEventsService } from "@core/global-state-and-events.service";
import { UserService } from "@core/services/user.service";
import { BiLocalizationHelperService } from "@core/utility-services/bi-localization-helper.service";
import { BiNotificationConfig } from "@core/utility-services/bi-toast-notification.service";
import { UserInfo } from "@models/user/UserInfo";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { TranslateService } from "@ngx-translate/core";
import { BiCustomValidators } from "@shared/classes/BiCustomValidators";
import { BiCustomAnimations } from "@shared/classes/BICustomAnimations";
import { uniqueID } from "@shared/variables-and-functions/helper-functions";
import { SelectItem } from "primeng/api/selectitem";
import { debounceTime, filter, take } from "rxjs/operators";

@UntilDestroy()
@Component({
  selector: "app-user-profile",
  templateUrl: "./user-profile.component.html",
  styleUrls: ["./user-profile.component.scss"],
  animations: [BiCustomAnimations.fadeInDown]
})
export class UserProfileComponent implements OnInit {

  //@ViewChild("phoneControl")
  public userForm: FormGroup;
  public userLanguages: Array<SelectItem>;

  public htmlIdPostFix = uniqueID();
  constructor(
    public localizor: BiLocalizationHelperService,
    private translator: TranslateService,
    private userService: UserService,
    private eventsManager: GlobalStateAndEventsService
  ) {

  }

  public ngOnInit() {
    this.initUserInfo();
  }

  private initUserInfo() {
    // get current user info
    // this.userService
    //   .getUserInfo()
    //   .pipe(take(1))
    //   .subscribe((info) => {
    //     this.userLanguages = info.languages.map(
    //       (language) =>
    //         <SelectItem>{
    //           label: language.displayName,
    //           value: language.value.id
    //         }
    //     );

    //     this.initFormGroup(info);

    //     this.setLanguage(info.languageId);
    //   });
  }

  private initFormGroup(userInfo: UserInfo) {
    this.userForm = new FormGroup({
      name: new FormControl(userInfo.name, {
        validators: [Validators.required],
        updateOn: "blur"
      }),
      mobileNumber: new FormControl(userInfo.mobileNumber, {
        updateOn: "blur"
      }),
      email: new FormControl(userInfo.email, { updateOn: "blur" }),
      languageId: new FormControl(userInfo.languageId)
    });

    // Init Validators
    this.initEmailValidation();

    // Use timeout and clear errors as setting validators triggers a validation, which we don't want
    setTimeout(() => {
      this.email.setErrors(null);
      this.userForm.markAsPristine(); // important! For some reason, inputs would be dirty otherwise
    }, 100);
  }
  // FormGroup Initalizers
  private initEmailValidation() {
    this.email.setValidators([BiCustomValidators.email()]);

    // Make sure the email cannot contain white spaces by removing it after it changes
    this.email.valueChanges
      .pipe(
        filter((value) => value !== null && value !== undefined),
        untilDestroyed(this),
        debounceTime(2000)
      )
      .subscribe((value) => {
        this.email.setValue(value.replace(/\s/g, ""));
      });
  }

  // GETTERS FOR EASY ACCESS TO FORMCONTROLS
  public get name() {
    return this.userForm.get("name") as FormControl;
  }

  public get email() {
    return this.userForm.get("email") as FormControl;
  }
  public get newsletterActive() {
    return this.userForm.get("newsletterActive") as FormControl;
  }
  public get languageId() {
    return this.userForm.get("languageId") as FormControl;
  }

  public setLanguage(value: number) {
    const formControl = this.userForm.get("languageId") as FormControl;
    formControl.setValue(value);
  }

  // Click events
  public saveUserInfo() {
    if (this.userForm.valid && this.userForm.dirty) {
      const userInfo: UserInfo = {
        name: this.name.value,
        email: this.email.value,
        languageId: this.languageId.value
      };

      // this.userService
      //   .setUserInfo(userInfo)
      //   .pipe(take(1))
      //   .subscribe(() => {
      //     this.userForm.markAsPristine();
      //     this.eventsManager.showAppEventNotification.next(
      //       new BiNotificationConfig(undefined, this.translator.instant("settings.myUser.ProfileInfoUpdated"), undefined)
      //     );
      //   });
    }
  }
}

