using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using System.Net;
using MahApps.Metro.Controls;

namespace NoteKeeper
{
	/// <summary>
	/// Interaction logic for Profile.xaml
	/// </summary>
	public partial class Profile : MetroWindow
    {
		public Profile()
		{
			InitializeComponent();
			RandomQuoteMaker();

		}

		
		private void Button_Click(object sender, RoutedEventArgs e)
		{

        }

		private void TxbSearch_TextChanged(object sender, TextChangedEventArgs e)
		{

			
		}

		public void RandomQuoteMaker()
		{
			Console.WriteLine("get request to random quote");
			var url = "https://zenquotes.io/api/random";

			var request = WebRequest.Create(url);
			request.Method = "GET";

			var webResponse = request.GetResponse();
			var webStream = webResponse.GetResponseStream();

			var reader = new StreamReader(webStream);
			var data = reader.ReadToEnd();

			Console.WriteLine(data);
		}

		private void myTextBlock_Loaded(object sender, RoutedEventArgs e)
		{
			RandomQuoteMaker();
		}
	}
}
