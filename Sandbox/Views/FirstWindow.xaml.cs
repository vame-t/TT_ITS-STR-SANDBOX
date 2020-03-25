using Sandbox.ViewModels;
using System;
using System.Windows;


namespace Sandbox.Views
{
    public partial class FirstWindow : Window
    {
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

