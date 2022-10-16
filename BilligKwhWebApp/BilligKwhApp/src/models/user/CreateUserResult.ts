
export interface UserCreationWarning {
  localeStringResource: string;
  parameters: Array<string>;
}

export interface CreateUserResult {
  id: number;
  warnings: Array<UserCreationWarning>;
}
