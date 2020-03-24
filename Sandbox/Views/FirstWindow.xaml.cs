using Sandbox.ViewModels;
using System;
using System.Windows;


namespace Sandbox.Views
{
    /// <summary>
    /// Interaction logic for FirstWindow.xaml
    /// </summary>
    public partial class FirstWindow : Window
    {
        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);

        //    Application.Current.Shutdown();
        //}
        public FirstWindow()
        {
            InitializeComponent();
            MainViewModel_FirstWindow vm = new MainViewModel_FirstWindow();
            this.DataContext = vm;
            if (vm.CloseAction == null)
            {
                vm.CloseAction = new Action(this.Close); 
            }
        }

      
    }
}

