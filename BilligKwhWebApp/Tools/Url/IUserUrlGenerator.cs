﻿using BilligKwhWebApp.Core.Domain;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BilligKwhWebApp.Tools.Url
{
    public interface IUserUrlGenerator
    {
        Uri GetNewPasswordUrl(Bruger user, IUrlHelper urlHelper);
    }
}