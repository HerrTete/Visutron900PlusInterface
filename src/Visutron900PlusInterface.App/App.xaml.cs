using System.Windows;

namespace Visutron900PlusInterface.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var integration = new Integration();
            integration.Run();
            base.OnStartup(e);
        }
    }
}
