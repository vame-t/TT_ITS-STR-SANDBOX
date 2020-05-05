using Sandbox.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sandbox.Views
{
    /// <summary>
    /// Interaktionslogik für Formular.xaml
    /// </summary>
    public partial class Formular : Window
    {
        public Formular()
        {
            InitializeComponent();
            MainViewModel_Formular vm = new MainViewModel_Formular();
            this.DataContext = vm;
            if (vm.CloseAction == null)
            {
                vm.CloseAction = new Action(this.Close);
            }
        }


        //protected override void OnClosed(EventArgs e)
        //{
        //    base.OnClosed(e);

        //    Application.Current.Shutdown();
        //}

    }
}
