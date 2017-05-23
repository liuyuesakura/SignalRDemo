using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SignalRDemo.Startup))]
namespace SignalRDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            var hubConfiguration = new Microsoft.AspNet.SignalR.HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            app.MapSignalR();
        }
    }
}
