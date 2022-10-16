// import { SelectListItem } from "../common/SelectListItem";
// import { CustomerBase } from "./CustomerBase";

// /**
//  * Like a Customer but only intended to be used when returning data for an edit page (e.g. customer edit page)
//  */
// export interface CustomerEdit extends CustomerBase {
//   /**
//    * The name of the header to use for areas where internal messages are shown. As an example, it's being used on the "Status" page.
//    */
//   sms2InternalDisplayName: string;

//   benchmarkDisplayName: string;

//   /**
//    * Flag telling if the customer has any profiles setup for Voice messages
//    */
//   hasProfilesWithVoice?: boolean;

//   /**
//    * Flag telling whether the customer has any profiles with the role "CanReceiveSmsReplies"
//    */
//   hasProfilesThatCanReceiveSmsReply?: boolean;

//   /**
//    * Tells whether the VoiceSendAs numbers should be 1 of the available InfoBip numbers. If false, any number specified by user can be used.
//    */
//   useInfoBipVoiceNumbers: boolean;

//   /**
//    * List of .NET Time Zones (our model object is simplified, though).
//    */
//   timeZones: Array<SelectListItem<string>>;

//   senderGuid: string;
// }
