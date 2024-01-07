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
    public partial class JobAdPage : ContentPage
    {
        public JobAdPage(JobAdvertisement jobAdvertisement)
        {
            InitializeComponent();

            if (App.Current.Properties["Session"] != null)
            {
                int userId = (App.Current.Properties["Session"] as User).Id;

                if (!App.DataBase.CheckApplicationAsync(new JobApplication(jobAdvertisement.Id, userId)).Result)
                {
                    Btn_Apply.Text = "APLIKUJ";
                }
                else
                {
                    Btn_Apply.Text = "PRZESTAŃ APLIKOWAĆ";
                }

                Btn_Apply.IsVisible = true;
                Btn_Apply.IsEnabled = true;


                if ((App.Current.Properties["Session"] as User).IsAdmin == 1)
                {
                    Btn_EditJobAd.IsVisible = true;
                    Btn_EditJobAd.IsEnabled = true;

                    Btn_DeleteJobAd.IsVisible = true;
                    Btn_DeleteJobAd.IsEnabled = true;

                    Btn_Apply.IsVisible = false;
                    Btn_Apply.IsEnabled = false;
                }
                else
                {
                    Btn_EditJobAd.IsVisible = false;
                    Btn_EditJobAd.IsEnabled = false;

                    Btn_DeleteJobAd.IsVisible = false;
                    Btn_DeleteJobAd.IsEnabled = false;
                }
            }
            else
            {
                Btn_Apply.IsVisible = false;
                Btn_Apply.IsEnabled = false;
            }

            Title = "Strona oferty pracy";
            return;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = await App.DataBase.ReadJobAdvertisementByIdAsync((BindingContext as JobAdvertisement).Id);
        }
        private async void Apply(object sender, EventArgs e)
        {
            int jobAdId = (BindingContext as JobAdvertisement).Id;
            int userId = (App.Current.Properties["Session"] as User).Id;


            if (await App.DataBase.CheckApplicationAsync(new JobApplication(jobAdId, userId)))
            {
                await App.DataBase.DeleteApplicationAsync(new JobApplication(jobAdId, userId));
                Btn_Apply.Text = "APLIKUJ";
                return;
            }
            await App.DataBase.CreateApplicationAsync(new JobApplication(jobAdId, userId));
            Btn_Apply.Text = "PRZESTAŃ APLIKOWAĆ";
            return;
        }

        private async void EditJobAd(object sender, EventArgs e)
        {
            AddOrEditJobAdvertisement addOrEditJobAdvertisement = new AddOrEditJobAdvertisement(BindingContext as JobAdvertisement);
            await Navigation.PushAsync(addOrEditJobAdvertisement);
            return;
        }

        private async void DeleteJobAd(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Usuwanie", "Czy na pewno chcesz usunąć ogłoszenie od " + (BindingContext as JobAdvertisement).CompanyName + " na stanowisko " + (BindingContext as JobAdvertisement).PositionName + "?", "TAK", "NIE");

            if (confirm)
            {
                await App.DataBase.DeleteJobAdvertisementAsync(BindingContext as JobAdvertisement);
                await Navigation.PopToRootAsync();
                Navigation.RemovePage(this);
            }
            AddOrEditJobAdvertisement addOrEditJobAdvertisement = new AddOrEditJobAdvertisement(BindingContext as JobAdvertisement);
            await Navigation.PushAsync(addOrEditJobAdvertisement);
            return;
        }
    }
}