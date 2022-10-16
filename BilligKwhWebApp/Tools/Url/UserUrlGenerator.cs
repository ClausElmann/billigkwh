using BilligKwhWebApp.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BilligKwhWebApp.Tools.Url
{
    public class UserUrlGenerator : IUserUrlGenerator
    {
        public Uri GetNewPasswordUrl(Bruger user, IUrlHelper urlHelper)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));
            if (urlHelper is null)
                throw new ArgumentNullException(nameof(urlHelper));

            return new Uri($"{urlHelper.ActionContext.HttpContext.Request.Scheme}://{urlHelper.ActionContext.HttpContext.Request.Host}/new-password?token={user.PasswordResetToken}&email={user.Brugernavn}");
        }
    }
}
