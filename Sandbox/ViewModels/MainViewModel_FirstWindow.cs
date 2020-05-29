using System;
using System.ComponentModel;
using System.Windows.Input;
using Sandbox.Views;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace Sandbox.ViewModels
{
    public class MainViewModel_FirstWindow : INotifyPropertyChanged
    {
        //Implementierung des Interfaces: 
        public event PropertyChangedEventHandler PropertyChanged;

        //Fields: 
        Formular formular = new Formular();
        DeleteStudent deleteStudentWindow = new DeleteStudent(); 
        Window2 notenFormular = new Window2();
        private bool ButtonNewStudentIsChecked;
        private bool ButtonBestehenderSchueler;
        private bool ButtonDeleteSchueler;
        private ICommand nextCommand;
        //private bool isVisible;

        //Properties: 
        public Action CloseAction { get; set;}
        //public bool IsVisible
        //{
        //    get { return this.isVisible; }
        //    set 
        //    {   this.isVisible = true;
        //        NotifyOfPropertyChange("IsVisible");  
        //    }
        //}
        public bool ButtonAIsChecked
        {
            get { return ButtonNewStudentIsChecked; }
            set
            {
                ButtonNewStudentIsChecked = value;
            }
        }
        public bool ButtonBIsChecked
        {
            get { return ButtonBestehenderSchueler; }
            set
            {
                ButtonBestehenderSchueler = value;
            }
        }
        public bool ButtonCIsChecked
        {
            get { return ButtonDeleteSchueler;}
            set { ButtonDeleteSchueler = value;}
        }
        public ICommand NextCommand
        {
            get
            {
                if (nextCommand == null)
                {
                    nextCommand = new RelayCommand(
                        () => clickNext());
                }
                return nextCommand;
            }
        }

        //Methods: 

        /// <summary>
            /// Diese Methode ermöglicht es nach Auswahl eines Radiobuttons und nach dem Klick des verbundenen Buttons(siehe view: FirstWindow.xaml)
            /// ein Folgefenster zu öffnen. 
            /// Welches Fenster geöffnet wird hängt je nach Auswahl des Radiobuttons ab. 
            /// </summary>
        public void clickNext()
        {
            try
            {
                if (ButtonAIsChecked == true)
                {
                    formular.Show();
                    CloseAction();
                   
                }
                else if (ButtonBIsChecked == true)
                {
                    notenFormular.Show();
                    CloseAction(); 
                }
                else if (ButtonCIsChecked == true)
                {
                    deleteStudentWindow.Show();
                    CloseAction(); 
                }
                else
                {
                    MessageBox.Show("Bitte wählen Sie eines der wählbaren Funktionen aus.","Bitte wählen sie ein RadioButton aus" ,MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString()); 
            }
            
            
        }

    }
    
}
