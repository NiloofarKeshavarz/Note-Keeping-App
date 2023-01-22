using System;
using System.Collections.Generic;
using System.Data.Entity;
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
		}

		// Will clean up the richTextBox
		private void BtnNewNote_Click(object sender, RoutedEventArgs e)
		{
			FlowDocument flowDoc = new FlowDocument(new Paragraph(new Run(""))); // After this constructor is called, the new RichTextBox rtb will contain flowDoc. RichTextBox rtb = new RichTextBox(flowDoc);
			RtxbNewNote.Document = flowDoc;
			FlowDocument rtbContents = RtxbNewNote.Document;

		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{

		}

		private void LvNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			
			Note currentNote = LvNote.SelectedItem as Note;
			if (currentNote == null)
			{
				FlowDocument flowDoc = new FlowDocument(new Paragraph(new Run("")));
				RtxbNewNote.Document = flowDoc;
				Console.WriteLine("it is  null");
			}
			else
			{
				
				Console.WriteLine(currentNote.Body);
				RtxbNewNote.Document = new FlowDocument(new Paragraph(new Run(currentNote.Body)));
			}


		}

		private void BtnDelete_Click(object sender, RoutedEventArgs e)
		{

			Note currentSelPer = LvNote.SelectedItem as Note;


		}
	}

		
	
}

