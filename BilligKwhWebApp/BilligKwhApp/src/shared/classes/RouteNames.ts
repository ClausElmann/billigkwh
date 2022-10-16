/**
 * Class for all possible web app routes.
 * We try to follow Google's guideline for url naming conventions: https://support.google.com/webmasters/answer/76329?hl=en
 */
export class RouteNames {
  public static readonly frontPage = "frontpage";
  public static readonly login = "login";
  public static readonly resetPassword = "reset-password";
  public static readonly newPassword = "new-password";
  public static readonly forgotPassword = "forgot-password";
  public static readonly landing = "pages/landing";
  public static readonly dragdrop = "dragdrop";
  //public static readonly protected = "protected";

  public static readonly mainRoutes = {
    main: {
      main: "customer",
      settings: "settings",
      users: {
        main: "users",
        createUser: "create-user",
        editUser: {
          main: "edit-user", // an "id" parameter should be added in the route config
          children: {
            userRoles: "user-roles"
          }
        }
      },
      superAdmin: {
        translations: "super/translations",
        monitoring: {
          main: "super/monitoring",
          childRoutes: {
            dashboard: "dashboard",
            map: "map"
          }
        },
        customerAdmin: {
          main: "super/customer",
          children: {
            customers: {
              main: "customers",
              edit: "edit",
              create: "create"
            },
            users: {
              main: "users"
            }
          }
        },

        users: {
          main: "super/users",
          children: {
            usersIndex: "users-index",
            detail: {
              main: ":id/detail"
            }
          }
        },
        settings: {
          main: "super/settings",
          childRoutes: {
            packages: "packages-setup",
            functions: "functions",
            salesInfo: "sales-info"
          }
        }
      }
    }
  };


  public static noAuthRoutes = [
    RouteNames.login,
    RouteNames.resetPassword,
    RouteNames.newPassword,
    RouteNames.frontPage
    //RouteNames.protected
  ];

  public static readonly myUser = {
    main: "my-user",
    childRoutes: {
      edit: "edit"
    }
  };

}
