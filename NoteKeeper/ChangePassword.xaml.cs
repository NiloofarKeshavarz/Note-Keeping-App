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
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : UserControl
    {
        public ChangePassword()
        {
            InitializeComponent();
            this.TblUserName.Text = Globals.activeUser?.UserName;
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.GetHasError(TbxPassword) || Validation.GetHasError(TbxRetypePassword))
            {
                MessageBox.Show("Please correct the errors.");
                return;
            }
            String username = TblUserName.Text;
            String password = TbxPassword.Password;
            String retypePassword = TbxRetypePassword.Password;
            if (password != retypePassword)
            {
                MessageBox.Show("Password and Repeat Password doesn't match!");
                return;
            }
            
            UpdatePassword(username, password);
        }

        private void UpdatePassword(string username, string newPassword)
        {
            using (var context = new NoteDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserName == username);
                if (user != null)
                {
                    user.Password = newPassword;
                    context.SaveChanges();
                    MessageBox.Show("Password updated successfully.");
                }
            }
        }
    }
}
