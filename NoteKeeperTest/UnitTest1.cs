using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoteKeeper;
using System.Windows.Controls;
using System.Windows.Documents;

namespace NoteKeeper.Tests
{
	[TestClass()]
	public class UnitTest1
	{
		[TestMethod()]
		public void ResetFieldTest()
		{
			var mockTxbTitle = new TextBox();
			var mockRtxbNewNote = new RichTextBox();
			var mockCmbTag = new ComboBox();
			mockTxbTitle.Text = "Some Text";
			mockCmbTag.SelectedIndex = 0;

			Assert.Fail();
		}

		[TestMethod()]
		public void BtnNewNote_ClickTest()
		{
			// Arrange
			var window = new MainWindow();
			//window.RtxbNewNote.Document = new FlowDocument(new Paragraph(new Run("Test")));
			//window.TxbTitle.Text = "Test";

			Assert.Fail();
		}
	}
}

