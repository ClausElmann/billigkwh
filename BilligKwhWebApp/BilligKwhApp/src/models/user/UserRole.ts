
export interface CustomerUserRole {
  defaultSelected: boolean;
  hasAccess: boolean;
  userRole: UserRole;
}


/**
 * Defines a role that a user can have
 */
export interface UserRole {
  id: number;

  /**
   * Name as used in database
   */
  name: string;

  /**
   * The display name.
   */
  nameLocalized: string;

  /**
   * Localized name of role category.
   */
  category: string;

  description: string;

  visible?: boolean;
}
