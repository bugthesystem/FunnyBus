namespace Sample.MvcApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
           Bootstrapper.Start();
        }
    }
}