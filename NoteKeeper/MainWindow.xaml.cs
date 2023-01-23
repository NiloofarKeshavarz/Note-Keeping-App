using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NoteKeeper
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            createBinding(TbxUsername);
            createBinding(TbxPassword);
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow RegisterWindow = new RegisterWindow();
            RegisterWindow.Show();
            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
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
                    var user = context.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
                    if (user != null)
                    {
                        Globals.activeUser = user;
                        MainEditor mainEditor = new MainEditor();
                        mainEditor.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(this, "Invalid username or password", "Login error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "Error reading from database\n" + ex.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private bool validate()
        {
            // all validations has to be called to update validation state of all the controls before checking the result
            bool isUserNameValid = validateUserName();
            bool isPasswordValid = validatePassword();

            return isUserNameValid && isPasswordValid;
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
    }
}
