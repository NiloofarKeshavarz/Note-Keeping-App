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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoteKeeper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = TbxUsername.Text;
            string password = TbxPassword.Password;
            Login(username, password);

        }
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow RegisterWindow = new RegisterWindow();
            RegisterWindow.Show();
            this.Close();
        }


        private void Login(string username, string password)
        {
            if (Validation.GetHasError(TbxUsername) || Validation.GetHasError(TbxPassword))
            {
                MessageBox.Show("Please correct the errors.");
            }
            else
            {
                using (var context = new NoteDbContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
                    if (user != null)
                    {
                        Globals.activeUser = user;
                        Home homeWindow = new Home();
                        homeWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }
            }

        }
    }
}
