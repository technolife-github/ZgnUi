using System.Security.Claims;

namespace ZgnWebApi.Core.Utilities.IoC
{
    public static class ServiceTool
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IServiceCollection Create(IServiceCollection services)
        {
            ServiceProvider = services.BuildServiceProvider();
            return services;
        }
        public static int GetUserId()
        {
            var context = ServiceProvider.GetService<IHttpContextAccessor>();
            if (context?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value != null)
                return int.Parse(context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return 0;
        }
    }
}
