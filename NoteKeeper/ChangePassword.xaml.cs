using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

            try
            {
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
            }
            catch (SystemException ex)
            {
                MessageBox.Show("Error reading from database\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }

            TbxPassword.Password = "";
            TbxRetypePassword.Password = "";
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

            List<string> errors = user.validate("Password");
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

        private void createBinding(Control control)
        {
            control.SetBinding(TagProperty, new Binding());
        }

        private bool setBindingValidation(Control control, string error)
        {
            var bindingExpression = control.GetBindingExpression(TagProperty);
            Validation.ClearInvalid(bindingExpression);
            if (error != null)
            {
                var validationError = new ValidationError(new ExceptionValidationRule(), bindingExpression);
                validationError.ErrorContent = error;
                Validation.MarkInvalid(bindingExpression, validationError);
                return false;
            }
            return true;
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
