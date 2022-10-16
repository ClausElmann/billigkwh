import { Language } from "../common/Language";
import { SelectListItem } from "../common/SelectListItem";

export interface UserInfo {
  name: string;
  email?: string;
  mobileNumber?: string;
  //newsletterActive: boolean;
  newPassword?: string;
  /**
   *  1 = DK, 2 = SV, 3 = EN
   */
  languageId: number;
  languages?: SelectListItem<Language>[];
}
