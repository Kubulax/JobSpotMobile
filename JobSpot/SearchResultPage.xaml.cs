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
    public partial class SearchResultPage : ContentPage
    {
        private static List<FilterParam> _filterParams;
        private static int _userId;
        public SearchResultPage(List<FilterParam> filterParams)
        {
            InitializeComponent();
            _filterParams = filterParams;
            Title = "Wyniki wyszukiwania";
        }
        public SearchResultPage(int userId)
        {
            InitializeComponent();
            _userId = userId;

            Title = "Aplikacje użytkownika " + (App.Current.Properties["Session"] as User).Nickname;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if(_filterParams != null)
            {
                ListView_JobAdvertisements.ItemsSource = await App.DataBase.ReadJobAdvertisementsAsync(_filterParams);
                return;
            }
            
            if(_userId != 0)
            {
                ListView_JobAdvertisements.ItemsSource = await App.DataBase.ReadApplicationsByUserIdAsync(_userId);
                return;
            }
        }

        private async void GoToTheJobAdWindow(object sender, ItemTappedEventArgs e)
        {
            JobAdvertisement jobAdvertisement = e.Item as JobAdvertisement;

            if (jobAdvertisement != null)
            {
                JobAdPage jobAdPage = new JobAdPage(jobAdvertisement);
                jobAdPage.BindingContext = jobAdvertisement;
                await Navigation.PushAsync(jobAdPage);
            }

            return;
        }
    }
}