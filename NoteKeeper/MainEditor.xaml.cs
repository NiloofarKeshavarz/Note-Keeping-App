using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace NoteKeeper
{
	/// <summary>
	/// Interaction logic for MainEditor.xaml
	/// </summary>
	public partial class MainEditor : Window
	{

		List<Note> notes = new List<Note>();
		public MainEditor()
		{
			InitializeComponent();
			try
			{
				Globals.dbContext = new NoteDbContext();
				LvNote.ItemsSource = Globals.dbContext.Notes.ToList();
			}
			catch (SystemException ex)
			{
				MessageBox.Show(this, "Connection failed" + ex.Message, "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
				Environment.Exit(1);

			}
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        }

        // Will clean up the richTextBox
        private void ResetField()
        {
            TxbTitle.Text = string.Empty;
            TxbTag.Text = string.Empty;
            RtxbNewNote.Document.Blocks.Clear();

        }

        private void BtnNewNote_Click(object sender, RoutedEventArgs e)
		{
			FlowDocument flowDoc = new FlowDocument(new Paragraph(new Run(""))); // After this constructor is called, the new RichTextBox rtb will contain flowDoc. RichTextBox rtb = new RichTextBox(flowDoc);
			RtxbNewNote.Document = flowDoc;
			FlowDocument rtbContents = RtxbNewNote.Document;

		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
            TextRange tr = new TextRange(RtxbNewNote.Document.ContentStart, RtxbNewNote.Document.ContentEnd);
            MemoryStream ms = new MemoryStream();
            tr.Save(ms, DataFormats.Rtf);
            string NoteBodyData = ASCIIEncoding.Default.GetString(ms.ToArray());


            Note note = new Note(TxbTitle.Text, NoteBodyData, DateTime.Now, DateTime.Now, 2);
            Globals.dbContext.Notes.Add(note);
            //Globals.dbContext.Notes.Attach(note);

            Tag tag = new Tag(TxbTag.Text);
            Globals.dbContext.Tags.Add(tag);
            //Globals.dbContext.Tags.Attach(tag);

            note.Tags = new List<Tag>();
            note.Tags.Add(tag);

            Globals.dbContext.SaveChanges();
			LvNote.ItemsSource = Globals.dbContext.Notes.ToList();
			LvNote.Items.Refresh();
            ResetField();
        }

		private void LvNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
            TxbTitle.Text = (LvNote.SelectedItem as Note).Title;
            TextRange tr = new TextRange(RtxbNewNote.Document.ContentStart, RtxbNewNote.Document.ContentEnd);
            string selectedNote = (LvNote.SelectedItem as Note).Body;

            //convert string to MemoryStream 
            MemoryStream ms = GetMemoryStreamFromString(selectedNote);
            tr.Load(ms, DataFormats.Rtf);

   //         Note currentNote = LvNote.SelectedItem as Note;
			//if (currentNote == null)
			//{
			//	FlowDocument flowDoc = new FlowDocument(new Paragraph(new Run("")));
			//	RtxbNewNote.Document = flowDoc;
			//	Console.WriteLine("it is  null");
			//}
			//else
			//{
				
			//	Console.WriteLine(currentNote.Body);
			//	RtxbNewNote.Document = new FlowDocument(new Paragraph(new Run(currentNote.Body)));
			//}


		}

        public MemoryStream GetMemoryStreamFromString(string s)
        {
            if (s == null || s.Length == 0)
                return null;
            MemoryStream m = new MemoryStream();
            StreamWriter sw = new StreamWriter(m);
            sw.Write(s);
            sw.Flush();
            return m;
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
		{

			TextRange txt = new TextRange(RtxbNewNote.Document.ContentStart, RtxbNewNote.Document.ContentEnd);
			txt.Text = "";

		}

		private void BtnDelete_Click(object sender, RoutedEventArgs e)
		{

		}


		// rich text editor
		private void richTxt_SelectionChanged(object sender, RoutedEventArgs e)
        {

            object temp = RtxbNewNote.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            temp = RtxbNewNote.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));
            temp = RtxbNewNote.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));
            temp = RtxbNewNote.Selection.GetPropertyValue(Inline.FontSizeProperty);
            cmbFontSize.Text = temp.ToString();
        }

        private void cmbFontSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            RtxbNewNote.Selection.ApplyPropertyValue(Inline.FontSizeProperty, cmbFontSize.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //fetch data from SQL database
            List<Note> noteList = (from n in Globals.dbContext.Notes where n.UserId == 2 select n).ToList<Note>(); //FIX need to get UserId
            List<Note> ListViewNote = new List<Note>();
            List<string> SQLDataList = new List<string>();

            //search by title
            foreach (Note note in noteList)
            {
                if (note.Title.Contains(TxbSearch.Text))
                {
                    ListViewNote.Add(note);
                }
            }

            //search by body
            //foreach (Note note in noteList)
            //{
            //    if (note.Body.Contains(TxbSearch.Text))
            //    {
            //        ListViewNote.Add(note);
            //    };
            //}
        }

		
	}



}

