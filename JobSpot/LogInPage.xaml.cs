using JobSpot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JobSpot
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogInPage : ContentPage
    {
        public LogInPage()
        {
            InitializeComponent();
        }

        public async void LogIn (object sender, EventArgs e)
        {
            string nickName = Entry_Login.Text;
            string password = Entry_Password.Text;

            if (await App.DataBase.VerifyUserPasswordAsync(nickName, password))
            {
                App.Current.Properties["Session"] = await App.DataBase.ReadUserByNameAsync(nickName);
                await Navigation.PopToRootAsync();
            }
            else
            {
               await DisplayAlert("Błąd", "Błędne dane logowania.", "OK");
            }
        }

        public void GoToRegisterPage (object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterUserPage());
        }
    }
}