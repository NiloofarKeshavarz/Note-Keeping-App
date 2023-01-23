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
                    MessageBox.Show("Invalid username or password");
                }
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

            List<string> errors = validateUserObject(user, "UserName");
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

            List<string> errors = validateUserObject(user, "Password");
            if (errors.Count > 0)
            {
                error = errors[0];
            }

            return setBindingValidation(TbxPassword, error);
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
