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
    /// Interaction logic for FirstWindow.xaml
    /// </summary>
    public partial class FirstWindow : Window
    {
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
        public FirstWindow()
        {
            InitializeComponent();
        }

        //Diese Methode dient nur zur Logikübersicht, später soll ein eigenes Event erstellt werden 
        private void Button_Weiter_Fenster1_Click(object sender, RoutedEventArgs e)
        {
            //Instanzen zu den Windows: 
            FirstWindow erstesFenster = new FirstWindow(); 
            Formular newStudentWindow = new Formular();
            Window2 notenFormularWindow = new Window2();
            try
            {
                if (RB_NewStudent.IsChecked == true)
                {
                    newStudentWindow.Show();
                    Close();
                    //System.Windows.Application.Current.Shutdown();


                }
                else if (RB_ExistingStudent.IsChecked == true)
                {
                    notenFormularWindow.Show();
                    Close();
                    
                    //System.Windows.Application.Current.Shutdown();


                }
                else
                {
                    MessageBox.Show("Please decide and Click!");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            //finally
            //{
            //    if (newStudentWindow)
            //    {

            //    }
            //}
           

            // Bei der MVVM Architektur muss das schließen und öffnen der Fenster mit Commandbindings Funktionieren hier nur ein Beispiel: 

            /*
             *   Button Command="{Binding MainCloseButtonCommand}"
                 CommandParameter="{Binding ElementName=mainWindow}"
                 private void performMainCloseButtonCommand(object Parameter)
                {
                    Window objWindow  = Parameter as Window;
                    objWindow.Close();
             * 
             * 
             * 
             */

        }
    }
}
