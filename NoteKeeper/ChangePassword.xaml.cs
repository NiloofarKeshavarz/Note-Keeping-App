using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            this.TbxUsername.Text = Globals.activeUser?.UserName;

            createBinding(TbxPassword);
            createBinding(TbxRetypePassword);
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            if (!validate())
            {
                return;
            }

            string username = TbxUsername.Text;
            string password = TbxPassword.Password;

            using (var context = new NoteDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.UserName == username);
                if (user != null)
                {
                    user.Password = password;
                    context.SaveChanges();
                    MessageBox.Show("Password updated successfully.", "Change Password", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }

            TbxPassword.Password = "";
            TbxRetypePassword.Password = "";
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

        private bool validate()
        {
            // all validations has to be called to update validation state of all the controls before checking the result
            bool isPasswordValid = validatePassword();
            bool isRetypePasswordValid = validateRetypePassword();

            return isPasswordValid && isRetypePasswordValid;
        }

        private bool validatePassword()
        {
            string error = null;

            User user = new User();
            user.Password = TbxPassword.Password;

            List<string> errors = validateUserObject(user, "Password");
            if (errors.Count > 0)
            {
                error = errors[0];
            }

            return setBindingValidation(TbxPassword, error);
        }

        private bool validateRetypePassword()
        {
            string error = null;

            string password = TbxPassword.Password;
            string retypePassowrd = TbxRetypePassword.Password;
            if (retypePassowrd != password)
            {
                error = "Passwords don't match!";
            }

            return setBindingValidation(TbxRetypePassword, error);
        }

        private List<string> validateUserObject(User user, string property)
        {
            ValidationContext validationContext = new ValidationContext(user);
            validationContext.MemberName = property;
            List<System.ComponentModel.DataAnnotations.ValidationResult> results = new List<System.ComponentModel.DataAnnotations.ValidationResult>();
            bool isValid = Validator.TryValidateObject(user, validationContext, results, false);
            if (!isValid)
            {
                return results.Where(r => r.MemberNames.Contains(property)).Select(r => r.ErrorMessage).ToList();
            }
            return new List<string>();
        }

        private void createBinding(Control control)
        {
            control.SetBinding(TagProperty, new Binding());
        }

        private bool setBindingValidation(Control control, string error)
        {
            var bindingExpression = control.GetBindingExpression(TagProperty);
            if (error != null)
            {
                var validationError = new ValidationError(new ExceptionValidationRule(), bindingExpression);
                validationError.ErrorContent = error;
                Validation.MarkInvalid(bindingExpression, validationError);
                return false;
            }
            else
            {
                Validation.ClearInvalid(bindingExpression);
                return true;
            }
        }

        private void TbxPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            validatePassword();
        }

        private void TbxRetypePassword_LostFocus(object sender, RoutedEventArgs e)
        {
            validateRetypePassword();
        }
    }
}
