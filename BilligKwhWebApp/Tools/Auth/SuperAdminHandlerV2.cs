using System.Threading.Tasks;
using BilligKwhWebApp.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BilligKwhWebApp.Tools.Auth
{
	public class SuperAdminHandlerV2 : AuthorizationHandler<SuperAdminRequirementV2>
	{
        private readonly IWorkContext workContext;

        public SuperAdminHandlerV2(IWorkContext workContext)
        {
            this.workContext = workContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SuperAdminRequirementV2 requirement)
		{
            if (context is null)
                throw new System.ArgumentNullException(nameof(context));
            if (requirement is null)
                throw new System.ArgumentNullException(nameof(requirement));
            
            var user = workContext.CurrentUser;

			if (user != null)
			{
				if (workContext.IsUserSuperAdmin())
				{
					context.Succeed(requirement);
				}
				else
				{
					context.Fail();
				}
			}
			else
			{
				context.Fail();
			}

			return Task.CompletedTask;
		}
	}
}
