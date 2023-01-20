using NoteKeeper;
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
    public partial class ChangePasswordWindow : UserControl
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = TbxNewPassword.Password;
            ChangePassword(newPassword);

        }
        private void ChangePassword(string newPassword)
        {
            using (var context = new NoteDbContext())
            {
                var user = context.Users.Where(u => u.username == Globals.activeUser.username).SingleOrDefault();
                user.password = newPassword;
                context.SaveChanges();
            }
            Globals.activeUser.password = newPassword;
        }
    }
}
