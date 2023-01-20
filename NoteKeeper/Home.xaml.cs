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

namespace NoteKeeper
{
	/// <summary>
	/// Interaction logic for Home.xaml
	/// </summary>
	public partial class Home : Window
	{
		List<Note> notes = new List<Note>();
		public Home()
		{
			InitializeComponent();
			try
			{
				Globals.dbContext = new NoteDbContext();
				LvNotes.ItemsSource = Globals.dbContext.Notes.ToList();
			}
			catch(SystemException ex){
				MessageBox.Show(this, "Connection failed" + ex.Message, "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
				Environment.Exit(1);

			}
			}
		
	

		private void BtnCreate_Click_1(object sender, RoutedEventArgs e)
		{
			

		}
	}
}
