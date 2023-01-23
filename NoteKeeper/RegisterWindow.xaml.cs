using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NoteKeeper
{

    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();

            createBinding(TbxUsername);
            createBinding(TbxPassword);
            createBinding(TbxRetypePassword);
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            OpenLoginWindow();
        }

        private void OpenLoginWindow()
        {
            MainWindow MainWindow = new MainWindow();
            MainWindow.Show();
            this.Close();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (!validate())
            {
                return;
            }

            User newUser = new User();
            newUser.UserName = TbxUsername.Text;
            newUser.Password = TbxPassword.Password;

            try
            {
                using (var context = new NoteDbContext())
                {
                    context.Users.Add(newUser);
                    context.SaveChanges();
                    MessageBox.Show(this, "New user successfully registered!\nYou can login now!", "Register User", MessageBoxButton.OK, MessageBoxImage.Information);
                    OpenLoginWindow();
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error storing data in database\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private bool validate()
        {
            // all validations has to be called to update validation state of all the controls before checking the result
            bool isUserNameValid = validateUserName();
            bool isPasswordValid = validatePassword();
            bool isRetypePasswordValid = validateRetypePassword();

            return isUserNameValid && isPasswordValid && isRetypePasswordValid;
        }

        private bool validateUserName()
        {
            string error = null;

            User user = new User();
            user.UserName = TbxUsername.Text;

            List<string> errors = user.validate("UserName");
            if (errors.Count > 0)
            {
                error = errors[0];
            }

            if (string.IsNullOrEmpty(error))
            {
                try
                {
                    using (var context = new NoteDbContext())
                    {
                        var oldUser = context.Users.FirstOrDefault(u => u.UserName == user.UserName);
                        if (oldUser != null)
                        {
                            error = "UserName is already taken!";
                        }
                    }
                }
                catch (SystemException ex)
                {
                    MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(1);
                }
            }

            return setBindingValidation(TbxUsername, error);
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

        private void TbxUsername_LostFocus(object sender, RoutedEventArgs e)
        {
            validateUserName();
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
