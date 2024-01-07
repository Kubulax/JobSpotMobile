using JobSpot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JobSpot
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterUserPage : ContentPage
    {
        public RegisterUserPage()
        {
            InitializeComponent();
        }

        private async void Register(object sender, EventArgs e)
        {
            string nickname = Entry_Login.Text;
            string email = Entry_Email.Text;
            string password = Entry_Password.Text;
            string repeatPassword = Entry_RepeatPassword.Text;

            if
            (
                string.IsNullOrWhiteSpace(nickname) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(repeatPassword)
            )
            {
                await DisplayAlert("Błąd", "Proszę wprowadzić dane do rejestracji.", "OK");
                return;
            }

            User user = null;
            user = await App.DataBase.ReadUserByNameAsync(nickname);
            if (user != null)
            {
                await DisplayAlert("Nazwa użytkownika", "Podana nazwa użytkownika jest już zajęta.", "OK");
                return;
            }

            if (!ValidateEmailAddress(email))
            {
                await DisplayAlert("Adres email", "Podany adres email jest nieprwaidłowy.", "OK");
                return;
            }

            if (!VerifyPasswordsStrenght(password))
            {
                await DisplayAlert("Hasło", "Hasło powinno składać się z minimum 8 znaków. Dodatkowo musi ono zawierać cyfry, małe litery, duże litery oraz znaki specjalne (!, @, #, $, %, ^, &, *, ?, _, ~, -, £, (, )), w ilości dowolnej, większej od zera.", "OK");
                return;
            }

            if (password != repeatPassword)
            {
                await DisplayAlert("Błąd", "Hasła nie są zgodne.", "OK");
                return;
            }

            if (!ChkBox_Agreement.IsChecked)
            {
                await DisplayAlert("Umowa użytkownika", "Aby się zarejestrować należy zaakceptować warunki umowy użytkownika.", "OK");
                return;
            }

            await Navigation.PushAsync(new RegisterOrEditUserProfilePage(new User(nickname, email, password, 0)));
        }

        public bool ValidateEmailAddress(string emailAddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public bool VerifyPasswordsStrenght(string password)
        {
            if(password.Length < 8)
            {
                return false;
            }

            bool passwordIsStrong = true;

            string numbers = "1234567890";
            string lowerKeyLetters = "abcdefghijklmnopqrstuvwxyz";
            string upperHeyLetters = lowerKeyLetters.ToUpper();
            string specialCharacters = "!@#$%^&*?_~-£()";
            
            foreach(char c in numbers)
            {
                if(!password.Contains(c))
                {
                    passwordIsStrong = false;
                }
                else
                {
                    passwordIsStrong = true;
                    break;
                }
            }

            if(!passwordIsStrong) return passwordIsStrong;

            foreach (char c in lowerKeyLetters)
            {
                if (!password.Contains(c))
                {
                    passwordIsStrong = false;
                }
                else
                {
                    passwordIsStrong = true;
                    break;
                }
            }

            if (!passwordIsStrong) return passwordIsStrong;

            foreach (char c in upperHeyLetters)
            {
                if (!password.Contains(c))
                {
                    passwordIsStrong = false;
                }
                else
                {
                    passwordIsStrong = true;
                    break;
                }
            }

            if (!passwordIsStrong) return passwordIsStrong;

            foreach (char c in specialCharacters)
            {
                if (!password.Contains(c))
                {
                    passwordIsStrong = false;
                }
                else
                {
                    passwordIsStrong = true;
                    break;
                }
            }

            return passwordIsStrong;
        }
    }
}