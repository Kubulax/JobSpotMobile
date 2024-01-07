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
    public partial class AddOrEditJobAdvertisement : ContentPage
    {
        private bool editMode = false;
        int jobAdvertisementId;
        public AddOrEditJobAdvertisement()
        {
            InitializeComponent();

            btn_AddOrEditJobAdvert.Text = "DODAJ";
        }
        public AddOrEditJobAdvertisement(JobAdvertisement jobAdvertisement)
        {
            InitializeComponent();

            editMode = true;
            jobAdvertisementId = jobAdvertisement.Id;

            Entry_CompanyName.Text = jobAdvertisement.CompanyName;
            Entry_PositionName.Text = jobAdvertisement.PositionName;
            Picker_Category.SelectedItem = jobAdvertisement.Category;
            Entry_Pay.Text = jobAdvertisement.Pay;
            Entry_Localization.Text = jobAdvertisement.Localization;
            Picker_PositionLevel.SelectedItem = jobAdvertisement.PositionLevel;
            Picker_ContractType.SelectedItem = jobAdvertisement.ContractType;
            Picker_EmploymenType.SelectedItem = jobAdvertisement.EmploymentType;
            Picker_WorkType.SelectedItem = jobAdvertisement.WorkType;
            Editor_Duties.Text = jobAdvertisement.Duties;
            Editor_Requirements.Text = jobAdvertisement.Requirements;
            Editor_Benefits.Text = jobAdvertisement.Benefits;
            DatePicker_RecrutationEnd.Date = jobAdvertisement.RecrutationEnd;

            btn_AddOrEditJobAdvert.Text = "EDYTUJ";
        }

        private async void AddOrEditJobAdvert(object sender, EventArgs e)
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

            string duties = Editor_Duties.Text;
            string requirements = Editor_Requirements.Text;
            string benefits = Editor_Benefits.Text;
            DateTime recrutationEnd = DatePicker_RecrutationEnd.Date;

            if
            (
                string.IsNullOrWhiteSpace(companyName) ||
                string.IsNullOrWhiteSpace(positionName) ||
                string.IsNullOrWhiteSpace(category) ||
                string.IsNullOrWhiteSpace(pay) ||
                string.IsNullOrWhiteSpace(contractType) ||
                string.IsNullOrWhiteSpace(employmentType) ||
                string.IsNullOrWhiteSpace(workType)
            )
            {
                await DisplayAlert("Błąd", "Proszę wypełnić wymagane pola.", "OK");
                return;
            }
            if (editMode)
            {
                JobAdvertisement jobAdvertisement = new JobAdvertisement(jobAdvertisementId, companyName, positionName, category, pay, localization, positionLevel, contractType, employmentType, workType, duties, requirements, benefits, recrutationEnd);
                await App.DataBase.UpdateJobAdvertisementAsync(jobAdvertisement);
                await DisplayAlert("Edytowanie ogłoszenia", "Edycja powiodła się!", "OK");
                await Navigation.PopAsync();
                return;
            }

            await App.DataBase.CreateJobAdvertisementAsync(new JobAdvertisement(companyName, positionName, category, pay, localization, positionLevel, contractType, employmentType, workType, duties, requirements, benefits, recrutationEnd));
            await DisplayAlert("Dodawanie ogłoszenia", "Pomyślnie dodano ogłoszenie!", "OK");
            await Navigation.PushAsync(new AddOrEditJobAdvertisement());
            Navigation.RemovePage(this);
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
            Editor_Duties.Text = String.Empty;
            Editor_Requirements.Text = String.Empty;
            Editor_Benefits.Text = String.Empty;
            DatePicker_RecrutationEnd.Date = DateTime.Now;

            return;
        }
    }
}