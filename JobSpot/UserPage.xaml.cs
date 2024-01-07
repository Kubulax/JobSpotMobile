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
    public partial class UserPage : ContentPage
    {
        public UserPage()
        {
            InitializeComponent();
            lbl_Nickname.Text = (App.Current.Properties["Session"] as User).Nickname;
            Title = "Profil użytkownika " + lbl_Nickname.Text;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = await App.DataBase.ReadUserProfileByUserIdAsync((App.Current.Properties["Session"] as User).Id);
        }

        private async void EditProfile(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterOrEditUserProfilePage(BindingContext as UserProfile));
        }

        private async void ShowApplications(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchResultPage((App.Current.Properties["Session"] as User).Id));
        }
    }
}