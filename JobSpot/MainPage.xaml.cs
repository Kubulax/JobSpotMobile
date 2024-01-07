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
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            App.Current.Properties["Session"] = null;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await RefreshUserLoggedInChanges();
        }

        private Task RefreshUserLoggedInChanges()
        {
            if (App.Current.Properties["Session"] == null)
            {
                lbl_ShowNickname.Text = String.Empty;
                btn_LogOut.IsEnabled = false;
                btn_LogOut.IsVisible = false;
                btn_LogInOrShowProfile.BorderColor = Color.Red;
                btn_LogInOrShowProfile.IsEnabled = true;
                lbl_adminModeInfo.IsVisible = false;
                btn_AddJobAdvertisement.IsVisible = false;
                btn_AddJobAdvertisement.IsEnabled = false;
            }
            else
            {
                User user = App.Current.Properties["Session"] as User;
                lbl_ShowNickname.Text = user.Nickname;
                btn_LogOut.IsEnabled = true;
                btn_LogOut.IsVisible = true;

                if (user.IsAdmin == 1)
                {
                    btn_LogInOrShowProfile.IsEnabled = false;
                    btn_LogInOrShowProfile.BorderColor = Color.Gray;
                    lbl_adminModeInfo.IsVisible = true;
                    btn_AddJobAdvertisement.IsVisible = true;
                    btn_AddJobAdvertisement.IsEnabled = true;
                }
                else
                {
                    btn_LogInOrShowProfile.IsEnabled = true;
                    btn_LogInOrShowProfile.BorderColor = Color.Lime;
                    lbl_adminModeInfo.IsVisible = false;
                    btn_AddJobAdvertisement.IsVisible = false;
                    btn_AddJobAdvertisement.IsEnabled = false;
                }
            }

            return Task.CompletedTask;
        }

        private async void LogInOrShowProfile(object sender, EventArgs e)
        {
            if (App.Current.Properties["Session"] == null)
            {
                await Navigation.PushAsync(new LogInPage());
            }
            else
            {
                UserPage userPage = new UserPage();
                userPage.BindingContext = await App.DataBase.ReadUserProfileByUserIdAsync((App.Current.Properties["Session"] as User).Id);
                await Navigation.PushAsync(userPage);
            }
        }

        private void LogOut(object sender, EventArgs e)
        {
            App.Current.Properties["Session"] = null;
            OnAppearing();
        }

        private async void AddJobAdvertisement(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditJobAdvertisement());
        }

        private async void Search(object sender, EventArgs e)
        {
            string companyName = Entry_CompanyName.Text;

            string positionName = Entry_PositionName.Text;


            string category = String.Empty;
            if (Picker_Category.SelectedItem != null)
            {
                category = Picker_Category.Items[Picker_Category.SelectedIndex];
            }


            string pay = Entry_Pay.Text;


            string localization = Entry_Localization.Text;


            string positionLevel = String.Empty;
            if (Picker_PositionLevel.SelectedItem != null)
            {
                positionLevel = Picker_PositionLevel.Items[Picker_PositionLevel.SelectedIndex];
            }


            string contractType = String.Empty;
            if (Picker_ContractType.SelectedItem != null)
            {
                contractType = Picker_ContractType.Items[Picker_ContractType.SelectedIndex];
            }


            string employmentType = String.Empty;
            if (Picker_EmploymenType.SelectedItem != null)
            {
                employmentType = Picker_EmploymenType.Items[Picker_EmploymenType.SelectedIndex];
            }


            string workType = String.Empty;
            if (Picker_WorkType.SelectedItem != null)
            {
                workType = Picker_WorkType.Items[Picker_WorkType.SelectedIndex];
            }

            List<FilterParam> filters = new List<FilterParam>();

            if (!string.IsNullOrWhiteSpace(companyName))
            {
                filters.Add(new FilterParam("CompanyName", companyName));
            }
            if (!string.IsNullOrWhiteSpace(positionName))
            {
                filters.Add(new FilterParam("PositionName", positionName));
            }
            if (!string.IsNullOrWhiteSpace(category))
            {
                filters.Add(new FilterParam("Category", category));
            }
            if (!string.IsNullOrWhiteSpace(pay))
            {
                filters.Add(new FilterParam("Pay", pay));
            }
            if (!string.IsNullOrWhiteSpace(localization))
            {
                filters.Add(new FilterParam("Localization", localization));
            }
            if (!string.IsNullOrWhiteSpace(positionLevel))
            {
                filters.Add(new FilterParam("PositionLevel", positionLevel));
            }
            if (!string.IsNullOrWhiteSpace(contractType))
            {
                filters.Add(new FilterParam("ContractType", contractType));
            }
            if (!string.IsNullOrWhiteSpace(employmentType))
            {
                filters.Add(new FilterParam("EmploymentType", employmentType));
            }
            if (!string.IsNullOrWhiteSpace(workType))
            {
                filters.Add(new FilterParam("WorkType", workType));
            }

            await Navigation.PushAsync(new SearchResultPage(filters));

            return;
        }

        private void ClearAllFieds(object sender, EventArgs e)
        {
            Entry_CompanyName.Text = String.Empty;
            Entry_PositionName.Text = String.Empty;
            Picker_Category.SelectedItem = null;
            Entry_Pay.Text = String.Empty;
            Entry_Localization.Text = String.Empty;
            Picker_PositionLevel.SelectedItem = null;
            Picker_ContractType.SelectedItem = null;
            Picker_EmploymenType.SelectedItem = null;
            Picker_WorkType.SelectedItem = null;

            return;
        }
    }
}
