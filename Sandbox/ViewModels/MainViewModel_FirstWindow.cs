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


        //TODO: 
        /*
         * Mit dem Button Weiter soll bestimmt werden, welches Fenster sich öffnen soll.
         * Dies hängt davon ab, welcher RadioButton ausgewählt wurde. 
         * Anhand des ausgewähltem RadioButtons soll sich entweder Fenster nr.2 also Das Fenster mit den Noten
         * oder das Formular-Fenster öffnen, indem man einen neuen Schüler eintragen kann. 
         * Dies soll durch Command Bindings realisiert werden. 
         * Dadurch ist es sehr wichtig keine Button_Click events in der .cs-Datei der View zu generieren. 
         * Dafür haben wir jetzt unser ViewModel, denn hier soll die Magie stattfinden
         * 
         */

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
        //protected void NotifyOfPropertyChange(string name)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(name));
        //    }
        //}
    }






    
}
