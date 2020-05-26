using Sandbox.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox.ViewModels; 
using Xceed.Document.NET;

namespace Sandbox.Models
{
    public class DocGenerieren
    {

        private static readonly System.Drawing.Color COLOR_GRAU = System.Drawing.ColorTranslator.FromHtml("#595959");
        private static readonly System.Drawing.Color COLOR_SCHWARZ = System.Drawing.ColorTranslator.FromHtml("#000000");
        private static readonly System.Drawing.Color COLOR_SHADING = System.Drawing.ColorTranslator.FromHtml("#E6E6E6");
        private static readonly string fontart = "Liberation Serif";
        private string filename = @System.IO.Directory.GetCurrentDirectory() + "\\Student_certificate.docx";

        private Document document;
        //private Schueler schueler = new Schueler();
        //private MainViewModel_Window2 mainViewModel_Window2 = new MainViewModel_Window2(); 
        


        public void docGenerieren()
        {

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

            paragraph.Append("\n\n\nKlassenstufe:" /*hier kommt noch die klassenstufe nummer*/ ).FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("\t\tSchuljahr:" /*hier kommt noch die Schuljahr*/ ).FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);

            paragraph.Append("\n\n\nVor-und Zuname\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("...." + "\t\t\t\t\t\t\t\t").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);
            paragraph.Append("\n\ngeboren am\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("geboren_Test" + "\t\t\t\t\t\t\t\t").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);
            paragraph.Append("\n\nin\t\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("In_Test" + "\t\t\t\t\t\t\t\t\t").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);
            paragraph.Append("\n\nAusbildungsberuf\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("Ausbildungsberuf_Test" + "\t\t\t\t\t\t\t").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("\n\n\n\nLeistungen in den einzelnen Fächern:\n\n").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);

            //Fächer Noten...
            paragraph.Append("1. Pflichtfächer:\n\n").FontSize(10D).Color(COLOR_SCHWARZ).Bold().Font(fontart);
            paragraph.Append("Religionslehre\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("befriedigend").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("\t\tBetriebswirtschaftslehre\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("befriedigend" + "\n\n").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("Deutsch\t\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("befriedigend").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("\t\tSoftwareanwendung und\n\t\t\t\t\t\t -entwicklung\t\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("befriedigend" + "\n\n").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("Gemeinschaftskunde\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("befriedigend" + "\n\n").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("Wirtschaftskunde\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("befriedigend").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("\t\tIT-Systemtechnik\t\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("befriedigend" + "\n\n\n\n").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);

            paragraph.Append("1. Wahlpflichtfächer:\n\n").FontSize(10D).Color(COLOR_SCHWARZ).Bold().Font(fontart);
            paragraph.Append("Englisch\t\t\t").FontSize(10D).Color(COLOR_SCHWARZ).Font(fontart);
            paragraph.Append("befriedigend" + "\n\n\n\n\n").FontSize(10D).Font(fontart).Bold().Shading(COLOR_SHADING);
            paragraph.Append("Datum:" + "\n\n\n").FontSize(10D).Font(fontart);
            paragraph.Append("\t\tSchulleiter" + "\t\t\t\t\t" + "Klassenlehrerin/er").FontSize(10D).Font(fontart);


            // Fusszeile Test
            document.AddFooters();
            document.DifferentFirstPage = true;
            document.DifferentOddAndEvenPages = true;

            Table fuss = document.Footers.First.InsertTable(1, 1);
            fuss.Design = TableDesign.None;
            fuss.Alignment = Alignment.center;

            //fuss.Rows[1].Cells[0].Paragraphs[0].Append("Notenstufen:   sehr gut(1), gut(2), befriedigend(3), ausreichend(4), mangelhaft(5), ungenügend(6)").FontSize(9D).Color(COLOR_SCHWARZ);

            fuss.Rows[0].Cells[0].Paragraphs[0].Append("Seite ").FontSize(9D).Color(COLOR_GRAU);
            fuss.Rows[0].Cells[0].Paragraphs[0].AppendPageNumber(PageNumberFormat.normal);

            document.Save();

        }
    }
}
