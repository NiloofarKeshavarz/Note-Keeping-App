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
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }
        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            string username = TbxUsername.Text;
            string password = TbxPassword.Password;
            Register(username, password);

        }
        private void Register(string username, string password)
        {
            User newUser = new User();
            newUser.UserName = username;
            newUser.Password = password;
            

            using (var context = new NoteDbContext())
            {
                context.Users.Add(newUser);
                context.SaveChanges();
            }
        }
    }
}
