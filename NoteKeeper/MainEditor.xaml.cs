using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
using MahApps.Metro.Controls;

namespace NoteKeeper
{
    /// <summary>
    /// Interaction logic for MainEditor.xaml
    /// </summary>
    public partial class MainEditor : MetroWindow
    {

        List<Note> notes = new List<Note>();
        public MainEditor()
        {
            InitializeComponent();
            try
            {
                Globals.dbContext = new NoteDbContext();
                LvNote.ItemsSource = Globals.dbContext.Notes.Include(x => x.Tags).ToList();
                //LvNote.ItemsSource = (from Note n in Globals.dbContext.Notes where n.UserId == Globals.activeUser.Id select n).ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Connection failed" + ex.Message, "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);

            }
            cmbFontSize.ItemsSource = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            List<string> comboSource = new List<string>();
            foreach (Tag tag in Globals.dbContext.Tags)
            {
                comboSource.Add(tag.Name);
            }
            cmbTag.ItemsSource = comboSource;

        }

        // Will clean up the richTextBox
        private void ResetField()
        {
            TxbTitle.Text = string.Empty;
            RtxbNewNote.Document.Blocks.Clear();
            cmbTag.SelectedIndex = -1;

        }


        private void BtnNewNote_Click(object sender, RoutedEventArgs e)
        {
            FlowDocument flowDoc = new FlowDocument(new Paragraph(new Run(""))); // After this constructor is called, the new RichTextBox rtb will contain flowDoc. RichTextBox rtb = new RichTextBox(flowDoc);
            RtxbNewNote.Document = flowDoc;
            FlowDocument rtbContents = RtxbNewNote.Document;
            TxbTitle.Text = "";


        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextRange tr = new TextRange(RtxbNewNote.Document.ContentStart, RtxbNewNote.Document.ContentEnd);
                MemoryStream ms = new MemoryStream();
                tr.Save(ms, DataFormats.Rtf);
                string NoteBodyData = ASCIIEncoding.Default.GetString(ms.ToArray());
                Tag tag = (from t in Globals.dbContext.Tags where cmbTag.SelectedItem.ToString() == t.Name select t).FirstOrDefault<Tag>();

                Note selectNote = LvNote.SelectedItem as Note;

                if (selectNote == null)
                {
                    Note note = new Note(TxbTitle.Text, NoteBodyData, DateTime.Now, DateTime.Now, Globals.activeUser.Id);
                    note.Tags = new List<Tag>();
                    note.Tags.Add(tag);
                    Globals.dbContext.Notes.Add(note);
                    Globals.dbContext.SaveChanges();
                }

                else
                {

                    Note noteToUpdate = (from n in Globals.dbContext.Notes where selectNote.Id == n.Id select n).FirstOrDefault();

                    if (noteToUpdate != null)
                    {
                        noteToUpdate.Title = TxbTitle.Text;
                        noteToUpdate.Body = NoteBodyData;
                        noteToUpdate.LastModificationDate = DateTime.Now;

                        noteToUpdate.Tags = new List<Tag>();
                        noteToUpdate.Tags.Add(tag);
                        Globals.dbContext.Notes.AddOrUpdate(noteToUpdate);
                        Globals.dbContext.SaveChanges();
                    }

                }
                LvNote.ItemsSource = Globals.dbContext.Notes.Include(x => x.Tags).ToList();
                LvNote.Items.Refresh();
                ResetField();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error Saving in database. Check your inputs!\n" + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //fetch data from SQL database
            List<Note> noteList = (from n in Globals.dbContext.Notes where n.UserId == Globals.activeUser.Id select n).ToList<Note>();
            List<Note> ListViewNote = new List<Note>();
            //search by title
            if (sender == btnSearchByTitle)
            {
                foreach (Note note in noteList)
                {
                    if (note.Title.Contains(TxbSearch.Text))
                    {
                        ListViewNote.Add(note);
                    }
                }
            }

            //search by tag
            if (sender == btnSearchByTag)
            {
                foreach (Tag tag in Globals.dbContext.Tags)
                {
                    if (tag.Name.Contains(TxbSearch.Text))
                    {
                        var NoteList = from note in Globals.dbContext.Notes where note.Tags.Any(t => t.Id == tag.Id) && note.UserId == Globals.activeUser.Id select note;
                        foreach (Note n in NoteList)
                        {
                            ListViewNote.Add(n);
                        }
                    }
                }
            }


            //search by body
            if (sender == btnSearchByTag)
            {
                foreach (Note note in noteList)
                {
                    if (note.Body.Contains(TxbSearch.Text) && note.UserId == Globals.activeUser.Id)
                    {
                        ListViewNote.Add(note);
                    };
                }

            }



            LvNote.ItemsSource = ListViewNote;

        }

        private void LvNote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var note = LvNote.SelectedItem as Note;
            if (note != null)
            {
                TxbTitle.Text = (note).Title; //ex : nullRefrenceException
                TextRange tr = new TextRange(RtxbNewNote.Document.ContentStart, RtxbNewNote.Document.ContentEnd);
                string selectedNote = (note).Body;

                //convert string to MemoryStream 
                MemoryStream ms = GetMemoryStreamFromString(selectedNote);
                tr.Load(ms, DataFormats.Rtf);


                // TODO: load Tag ???
                //note.Tags.FirstOrDefault().Name;
                try
                {

                    Tag t = (from tag in Globals.dbContext.Tags where tag.Notes.Any(y => y.Id == note.Id) select tag).First<Tag>();
                    cmbTag.SelectedIndex = t.Id - 1;

                }
                catch (SystemException ex)
                {
                    Console.WriteLine(ex);
                    cmbTag.SelectedIndex = -1;
                }
                //catch (NullReferenceException ex)
                //{
                //    //MessageBox.Show(this, "Error deleting from database\n" ,"Tag Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //    Console.WriteLine(ex);
                //}
            }

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
            try {

                Note selectedNote = LvNote.SelectedItem as Note;

                if (selectedNote != null)
                {
                    Globals.dbContext.Notes.Remove(selectedNote);
                    Globals.dbContext.SaveChanges();
                }
                notes.Remove(selectedNote);
                LvNote.ItemsSource = Globals.dbContext.Notes.ToList();
                LvNote.Items.Refresh();
                ResetField();
            }
                catch(SystemException ex)
                {
                MessageBox.Show(this, "Error deleting from database\n" + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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

        private void cmbTag_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if ((pd.ShowDialog() == true))

            {

                //use either one of them    

                pd.PrintVisual(RtxbNewNote as Visual, "Print Visual");

                pd.PrintDocument((((IDocumentPaginatorSource)RtxbNewNote.Document).DocumentPaginator),

                    "Print RichtextBox Content");

            }
        }
        //FIXME:
        private void InsertImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.InitialDirectory = @"C:\\";
            fd.Filter = "PNG (*.png) |*.png | JPEG(*.jpg;*jpeg)";

            if(fd.ShowDialog() == true)
            {
                var clipBoardData = Clipboard.GetDataObject();
                BitmapImage bitmapImage= new BitmapImage(new Uri(fd.FileName, UriKind.Absolute));
                Clipboard.SetImage(bitmapImage);
                // TextBox.Paste(); //
                Clipboard.SetDataObject(clipBoardData);

            }
        }

        private void HamburgerMenuControl_ItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            if (e.InvokedItem is HamburgerMenuItem menuItem)
            {
                switch (menuItem.Label)
                {
                    case "About":
                        break;
                    case "Profile":
                        Profile profile = new Profile();
                        profile.Owner = this;
                        profile.ShowDialog();
                        break;
                }
            }
            if (HamburgerMenuControl.IsPaneOpen)
            {
                HamburgerMenuControl.IsPaneOpen = false;
            }
            e.Handled = true;
        }
    }



}

