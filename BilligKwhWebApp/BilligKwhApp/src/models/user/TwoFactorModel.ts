export interface TwoFactorModel {
  email: string;
  pinCode: number;
  smsGroupId?: number;
}
