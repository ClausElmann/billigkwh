import { UserRefreshToken } from "./UserRefreshToken";

export class TokenModel {
  stateCode: number;
  requestedAt: string;
  expiresAt: string;
  accessToken: string;
  userId: number;
  customerId: number;
  profileId: number;
  impersonateFromUserId: number;
  refreshTokenModel: UserRefreshToken;
  specificProfile: boolean;
}
