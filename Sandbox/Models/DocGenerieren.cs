using Sandbox.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ViewModels; 
using Xceed.Document.NET;
using System.Dynamic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Sandbox.Models
{
    public class DocGenerieren : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        static String connectionString = "SERVER=127.0.0.1;DATABASE=studentmanagement-db;UID=root;PASSWORD=;";
        MySqlConnection mySqlConnection = new MySqlConnection(connectionString);

        private static readonly System.Drawing.Color COLOR_GRAU = System.Drawing.ColorTranslator.FromHtml("#595959");
        private static readonly System.Drawing.Color COLOR_SCHWARZ = System.Drawing.ColorTranslator.FromHtml("#000000");
        private static readonly System.Drawing.Color COLOR_SHADING = System.Drawing.ColorTranslator.FromHtml("#E6E6E6");
        private static readonly string fontart = "Liberation Serif";
        private string filename = @System.IO.Directory.GetCurrentDirectory() + "\\Student_certificate.docx";

        private Document document;
        public Schueler schueler = new Schueler();
        public Klasse klasse = new Klasse();
        public Schuljahr schuljahr = new Schuljahr();
        public Note note = new Note(); 
        private ObservableCollection<Schueler> studentsID = new ObservableCollection<Schueler>();
        private ObservableCollection<Klasse> classes = new ObservableCollection<Klasse>();
        private ObservableCollection<Schuljahr> schuljahre = new ObservableCollection<Schuljahr>();
        private ObservableCollection<Note> noten = new ObservableCollection<Note>();

        public ObservableCollection<Schueler> StudentsID
        {
            get { return studentsID; }
            set
            {
                studentsID = value;
                PropertyChanged(this, new PropertyChangedEventArgs("StudentsID"));
            }
        }
        public ObservableCollection<Schuljahr> Schuljahre
        {
            get { return schuljahre; }
            set
            {
                schuljahre = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Schuljahre"));
            }
        }
        public ObservableCollection<Klasse> Classes
        {
            get { return classes; }
            set
            {
                classes = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Classes"));
            }
        }

        public ObservableCollection<Note> Noten
        {
            get { return noten; }
            set
            {
                noten = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Note"));
            }
        }

        public void getClassName()
        {
            try
            {
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open(); 
                }
                int ID = GetIDFromForeignKeyTblCLass(); 
                string query = "SELECT CLassName FROM `studentmanagement-db`.tbl_class INNER JOIN `studentmanagement-db`.tbl_student ON tbl_class_id = @FK_Klasse ";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@FK_Klasse", ID);
                    command.ExecuteNonQuery();
                    DataTable classTable = new DataTable();
                    sqlDataAdapter.Fill(classTable);
                    if (classes != null)
                    {
                        classes.Clear();
                    }
                    foreach (DataRow dataRow in classTable.Rows)
                    {
                        klasse.KlassenNamen = Convert.ToString(dataRow["CLassName"]);
                       
                    }
                }
            }
            catch (Exception)
            {

                 throw;
            }
        }
        public void getSchoolYear()
        {
            try
            {
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }
                int ID = GetIDFromForeignKeyTblSchoolYear();
                string query = "SELECT Schuljahr FROM `studentmanagement-db`.tbl_schuljahr INNER JOIN `studentmanagement-db`.tbl_student ON tbl_Schuljahr_id = @FK_Schuljahr ";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@FK_Schuljahr", ID);
                    command.ExecuteNonQuery();
                    DataTable schoolYearTable = new DataTable();
                    sqlDataAdapter.Fill(schoolYearTable);
                    if (classes != null)
                    {
                        schuljahre.Clear();
                    }
                    foreach (DataRow dataRow in schoolYearTable.Rows)
                    {
                        schuljahr.SchulJahrProperty= Convert.ToString(dataRow["Schuljahr"]);

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetIDFromForeignKeyTblCLass()
        {
            try
            {
                int IDFromPickedStudent = 0; 
                //mySqlConnection.Open();
                string query = "SELECT FK_Klasse FROM `studentmanagement-db`.tbl_student WHERE Vorname= @Vorname;";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@Vorname", schueler.Vorname);
                    command.ExecuteNonQuery();
                    DataTable studentTable = new DataTable();
                    sqlDataAdapter.Fill(studentTable);
                    if (studentsID != null)
                    {
                        studentsID.Clear();
                    }
                    foreach (DataRow dataRow in studentTable.Rows)
                    {
                        schueler.Schueler_ID = (int)(dataRow["FK_Klasse"]);
                        IDFromPickedStudent = schueler.Schueler_ID; 
                        studentsID.Add(schueler);
                    }
                }
            return IDFromPickedStudent; 
            }
            catch (Exception e)
            {
                return -1; 
            }
            finally
            {
                //mySqlConnection.Close(); 
            }

        }
        public int GetIDFromForeignKeyTblSchoolYear()
        {
            try
            {
                int IDFromPickedStudent = 0;
                //mySqlConnection.Open();
                string query = "SELECT FK_Schuljahr FROM `studentmanagement-db`.tbl_student WHERE Vorname= @Vorname;";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@Vorname", schueler.Vorname);
                    command.ExecuteNonQuery();
                    DataTable studentTable = new DataTable();
                    sqlDataAdapter.Fill(studentTable);
                    if (studentsID != null)
                    {
                        studentsID.Clear();
                    }
                    foreach (DataRow dataRow in studentTable.Rows)
                    {
                        schueler.Schueler_ID = (int)(dataRow["FK_Schuljahr"]);
                        IDFromPickedStudent = schueler.Schueler_ID;
                        studentsID.Add(schueler);
                    }
                }
                return IDFromPickedStudent;
            }
            catch (Exception e)
            {
                return -1;
            }
            finally
            {
                //mySqlConnection.Close(); 
            }
        }
        public string getNachname()
        {
            try
            {
                string nachName = "";
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }
                string query = "SELECT Nachname FROM `studentmanagement-db`.tbl_student WHERE Vorname= @Vorname;";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@Vorname", schueler.Vorname);
                    command.ExecuteNonQuery();
                    DataTable studentTable = new DataTable();
                    sqlDataAdapter.Fill(studentTable);
                    if (studentsID != null)
                    {
                        studentsID.Clear();
                    }
                    foreach (DataRow dataRow in studentTable.Rows)
                    {
                        schueler.Nachname = Convert.ToString(dataRow["Nachname"]);
                        nachName = schueler.Nachname;
                        studentsID.Add(schueler);
                    }
                }
            return nachName;
            }
             catch (Exception e)
            {
                return "-1";
            }
            finally
            {
                //mySqlConnection.Close(); 
            }
        }
        public string getBirthday()
        {
            try
            {
                string birthDay = "";
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }
                string query = "SELECT Geburtsdatum FROM `studentmanagement-db`.tbl_student WHERE Vorname= @Vorname;";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@Vorname", schueler.Vorname);
                    command.ExecuteNonQuery();
                    DataTable studentTable = new DataTable();
                    sqlDataAdapter.Fill(studentTable);
                    if (studentsID != null)
                    {
                        studentsID.Clear();
                    }
                    foreach (DataRow dataRow in studentTable.Rows)
                    {
                        schueler.GeborenAm = Convert.ToString(dataRow["Geburtsdatum"]);
                        birthDay = schueler.GeborenAm;
                        studentsID.Add(schueler);
                    }
                }
                return birthDay;
            }
            catch (Exception e)
            {
                return "-1";
            }
            finally
            {
                //mySqlConnection.Close(); 
            }
        }
        public void getBirthPlace()
        {
            try
            {
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }
                string query = "SELECT Geburtsort FROM `studentmanagement-db`.tbl_student WHERE Vorname= @Vorname;";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@Vorname", schueler.Vorname);
                    command.ExecuteNonQuery();
                    DataTable studentTable = new DataTable();
                    sqlDataAdapter.Fill(studentTable);
                    if (studentsID != null)
                    {
                        studentsID.Clear();
                    }
                    foreach (DataRow dataRow in studentTable.Rows)
                    {
                        schueler.GeburtsOrt = Convert.ToString(dataRow["Geburtsort"]);
                        studentsID.Add(schueler);
                    }
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
            }
            finally
            {
                //mySqlConnection.Close(); 
            }
        }
        public void getNoten()
        {
            try
            {
                int currentStudentID = getStudentID(); 
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }
                string query = "SELECT Note FROM `studentmanagement-db`.tbl_note WHERE FK_Fach_id = '1' AND FK_Student_id = @StudentID;";
                string queryITS = "SELECT Note FROM `studentmanagement-db`.tbl_note WHERE FK_Fach_id = '2' AND FK_Student_id = @StudentID;";
                string queryBWL = "SELECT Note FROM `studentmanagement-db`.tbl_note WHERE FK_Fach_id = '3' AND FK_Student_id = @StudentID;";
                string queryGK = "SELECT Note FROM `studentmanagement-db`.tbl_note WHERE FK_Fach_id = '4' AND FK_Student_id = @StudentID;";
                string queryWI = "SELECT Note FROM `studentmanagement-db`.tbl_note WHERE FK_Fach_id = '5' AND FK_Student_id = @StudentID;";
                string queryD = "SELECT Note FROM `studentmanagement-db`.tbl_note WHERE FK_Fach_id = '6' AND FK_Student_id = @StudentID;";
                string queryE = "SELECT Note FROM `studentmanagement-db`.tbl_note WHERE FK_Fach_id = '7' AND FK_Student_id = @StudentID;";

                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlCommand commandITS = new MySqlCommand(queryITS, mySqlConnection);
                MySqlCommand commandBWL = new MySqlCommand(queryBWL, mySqlConnection);
                MySqlCommand commandGK = new MySqlCommand(queryGK, mySqlConnection);
                MySqlCommand commandWI = new MySqlCommand(queryWI, mySqlConnection);
                MySqlCommand commandD = new MySqlCommand(queryD, mySqlConnection);
                MySqlCommand commandE = new MySqlCommand(queryE, mySqlConnection);

                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                MySqlDataAdapter sqlDataAdapterITS = new MySqlDataAdapter(commandITS);
                MySqlDataAdapter sqlDataAdapterBWL = new MySqlDataAdapter(commandBWL);
                MySqlDataAdapter sqlDataAdapterGK = new MySqlDataAdapter(commandGK);
                MySqlDataAdapter sqlDataAdapterWI = new MySqlDataAdapter(commandWI);
                MySqlDataAdapter sqlDataAdapterD = new MySqlDataAdapter(commandD);
                MySqlDataAdapter sqlDataAdapterE = new MySqlDataAdapter(commandE);

                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@StudentID", currentStudentID);
                    command.ExecuteNonQuery();
                    DataTable SAETable = new DataTable();
                    sqlDataAdapter.Fill(SAETable);
                    if (noten != null)
                    {
                        noten.Clear();
                    }
                    foreach (DataRow dataRow in SAETable.Rows)
                    {
                        
                        note.NoteSAE = (int)(dataRow["Note"]);

                        //studentsID.Add(schueler);
                    }
                }
                using (sqlDataAdapterITS)
                {
                    commandITS.Parameters.AddWithValue("@StudentID", currentStudentID);
                    commandITS.ExecuteNonQuery();
                    DataTable notenTable = new DataTable();
                    sqlDataAdapterITS.Fill(notenTable);
                    if (noten != null)
                    {
                        noten.Clear();
                    }
                    foreach (DataRow dataRow in notenTable.Rows)
                    {

                        note.NoteITS = (int)(dataRow["Note"]);

                        //studentsID.Add(schueler);
                    }
                }
                using (sqlDataAdapterBWL)
                {
                    commandBWL.Parameters.AddWithValue("@StudentID", currentStudentID);
                    commandBWL.ExecuteNonQuery();
                    DataTable BWLTable = new DataTable();
                    sqlDataAdapterBWL.Fill(BWLTable);
                    if (noten != null)
                    {
                        noten.Clear();
                    }
                    foreach (DataRow dataRow in BWLTable.Rows)
                    {

                        note.NoteBWL = (int)(dataRow["Note"]);

                    }
                }
                using (sqlDataAdapterGK)
                {
                    commandGK.Parameters.AddWithValue("@StudentID", currentStudentID);
                    commandGK.ExecuteNonQuery();
                    DataTable GKTable = new DataTable();
                    sqlDataAdapterGK.Fill(GKTable);
                    if (noten != null)
                    {
                        noten.Clear();
                    }
                    foreach (DataRow dataRow in GKTable.Rows)
                    {

                        note.NoteGK = (int)(dataRow["Note"]);

                    }
                }
                using (sqlDataAdapterWI)
                {
                    commandWI.Parameters.AddWithValue("@StudentID", currentStudentID);
                    commandWI.ExecuteNonQuery();
                    DataTable WITable = new DataTable();
                    sqlDataAdapterWI.Fill(WITable);
                    if (noten != null)
                    {
                        noten.Clear();
                    }
                    foreach (DataRow dataRow in WITable.Rows)
                    {

                        note.NoteWI = (int)(dataRow["Note"]);

                    }
                }
                using (sqlDataAdapterD)
                {
                    commandD.Parameters.AddWithValue("@StudentID", currentStudentID);
                    commandD.ExecuteNonQuery();
                    DataTable DTable = new DataTable();
                    sqlDataAdapterD.Fill(DTable);
                    if (noten != null)
                    {
                        noten.Clear();
                    }
                    foreach (DataRow dataRow in DTable.Rows)
                    {

                        note.NoteD = (int)(dataRow["Note"]);

                    }
                }
                using (sqlDataAdapterE)
                {
                    commandE.Parameters.AddWithValue("@StudentID", currentStudentID);
                    commandE.ExecuteNonQuery();
                    DataTable ETable = new DataTable();
                    sqlDataAdapterE.Fill(ETable);
                    if (noten != null)
                    {
                        noten.Clear();
                    }
                    foreach (DataRow dataRow in ETable.Rows)
                    {

                        note.NoteE = (int)(dataRow["Note"]);

                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public int getStudentID()
        {
            try
            {
                int IDFromPickedStudent = 0;
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }
                string query = "SELECT tbl_student_id FROM `studentmanagement-db`.tbl_student WHERE Vorname= @Vorname;";
                MySqlCommand command = new MySqlCommand(query, mySqlConnection);
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command);
                using (sqlDataAdapter)
                {
                    command.Parameters.AddWithValue("@Vorname", schueler.Vorname);
                    command.ExecuteNonQuery();
                    DataTable studentTable = new DataTable();
                    sqlDataAdapter.Fill(studentTable);
                    if (studentsID != null)
                    {
                        studentsID.Clear();
                    }
                    foreach (DataRow dataRow in studentTable.Rows)
                    {
                        schueler.Schueler_ID = (int)(dataRow["tbl_student_id"]);
                        IDFromPickedStudent = schueler.Schueler_ID;
                        studentsID.Add(schueler);
                    }
                }
                return IDFromPickedStudent;
            }
            catch (Exception e)
            {
                return -1;
            }
           
        }
        public string getNotenBezeichnung(int note)
        {
            string bezeichnung = ""; 
            switch (note)
            {
                case 1:
                    bezeichnung = "Sehr gut";
                    return bezeichnung;
                case 2:
                    bezeichnung = "Gut";
                    return bezeichnung;
                case 3:
                    bezeichnung = "Befriedigend";
                    return bezeichnung;
                case 4:
                    bezeichnung = "Ausreichend";
                    return bezeichnung;
                case 5:
                    bezeichnung = "Mangelhaft";
                    return bezeichnung;
                case 6:
                    bezeichnung = "Ungenügend";
                    return bezeichnung;

                default:
                    return "-";
            }
        }

        public void docGenerieren()
        {
          
            getClassName();
            getSchoolYear();
            getNachname();
            getBirthday();
            getBirthPlace();
            getNoten(); 

            document = Xceed.Words.NET.DocX.Create(filename);


            document.MarginTop = 0;
            document.MarginBottom = 0;
            document.MarginLeft = 65;
            document.MarginRight = 71;


            // Kopfzeile Test
            document.AddHeaders();
            document.DifferentFirstPage = true;
            document.DifferentOddAndEvenPages = true;

            Paragraph paragraph = document.InsertParagraph();
            paragraph.Append("Baden-Württemberg\t").FontSize(11D).Color(COLOR_SCHWARZ).Bold().Font(fontart);
            paragraph.Append("it.schule stuttgart\n\n").FontSize(15D).Color(COLOR_SCHWARZ).Bold().Font(fontart);
            paragraph.Append("\t\t\tGewerbliche und Kaufmännische Schule\n\t\t\tfür Informationstechnik\n\n").FontSize(11D).Color(COLOR_SCHWARZ).Bold().Font(fontart);
            paragraph.Append("\t\t\tZeugnis\n\t\t\tder Gewerblichen Berufsschule").FontSize(15D).Color(COLOR_SCHWARZ).Bold().Font(fontart);

            paragraph.Append("\n\n\nKlassenstufe: " + klasse.KlassenNamen).FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("\t\tSchuljahr: " + schuljahr.SchulJahrProperty ).FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);

            paragraph.Append("\n\n\nVor -und Zuname\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append(schueler.Vorname + " " + schueler.Nachname + "\t\t\t\t\t\t\t\t").FontSize(10D).Font(fontart).Bold();
            paragraph.Append("\n\ngeboren am\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append(schueler.GeborenAm + "\t\t\t\t\t\t\t\t").FontSize(10D).Font(fontart).Bold();
            paragraph.Append("\n\nin\t\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append(schueler.GeburtsOrt).FontSize(10D).Font(fontart).Bold();
            paragraph.Append("\n\nAusbildungsberuf\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("Fachinformatiker im Bereich der Anwendungsentwicklung" + "\t\t").FontSize(10D).Font(fontart).Bold();

            paragraph.Append("\n\n\n\nLeistungen in den einzelnen Fächern:\n\n").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);

            //Fächer Noten...
            paragraph.Append("Pflichtfächer:\n\n").FontSize(10D).Color(COLOR_SCHWARZ).Bold().Font(fontart);
            paragraph.Append("Englisch:  ").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append(getNotenBezeichnung(note.NoteE)).FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("\t\t\tBetriebswirtschaftslehre: ").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append(getNotenBezeichnung(note.NoteBWL) + "\n\n").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("Deutsch:  ").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append(getNotenBezeichnung(note.NoteD)).FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("\t\t\tSAE:  ").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append(getNotenBezeichnung(note.NoteSAE) + "\n\n").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("GK:  ").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append(getNotenBezeichnung(note.NoteGK)).FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("\t\t\tWI:  ").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append(getNotenBezeichnung(note.NoteWI) + "\n\n").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("IT-Systemtechnik:  ").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append(getNotenBezeichnung(note.NoteITS) + "\n\n\n\n").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

           
            paragraph.Append("Datum:" + "\n\n\n").FontSize(10D).Font(fontart);
            paragraph.Append("\t\tSchulleiter" + "\t\t\t\t\t" + "Klassenlehrerin/er").FontSize(10D).Font(fontart);


            // Fusszeile Test
            document.AddFooters();
            document.DifferentFirstPage = true;
            document.DifferentOddAndEvenPages = true;

            Table fuss = document.Footers.First.InsertTable(1, 1);
            fuss.Design = TableDesign.None;
            fuss.Alignment = Alignment.center;

            document.Save();

            MessageBox.Show("Word-Dokument wurde erstellt und ist zu finden unter: Projektverzeichnis/bin/Debug/Student_certificate.docx, bitte überprüfen Sie, ob alle Noten eingetragen sind, wenn nicht ist der Vorgang zu wiederholen.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
           
 }

