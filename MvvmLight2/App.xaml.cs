using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace MvvmLight2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
