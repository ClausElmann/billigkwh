export interface UserRefreshToken {
  userId: number;
  token: string;
  dateCreatedUtc: string;
  dateExpiresUtc: string;
}
