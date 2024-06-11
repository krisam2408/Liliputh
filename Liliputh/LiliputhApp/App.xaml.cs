using LiliputhApp.Model;

namespace LiliputhApp
{
    public partial class App : Application
    {
        public App()
        {
            Terminal.Instance.Main = new();

            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
