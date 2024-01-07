using JobSpot.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace JobSpot
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterOrEditUserProfilePage : ContentPage
    {
        private bool editMode = false;
        User user = new User();
        int userProfileId;
        public RegisterOrEditUserProfilePage(User user)
        {
            InitializeComponent();
            this.user = user;

            Title = "Zarejestruj profil użytkownika";
            btn_UniversalBtn.Text = "DODAJ";
        }
        public RegisterOrEditUserProfilePage(UserProfile userProfile)
        {
            InitializeComponent();
            editMode = true;
            this.user = App.DataBase.ReadUserByNameAsync((App.Current.Properties["Session"] as User).Nickname).Result;
            userProfileId = userProfile.Id;

            TxtBox_Name.Text = userProfile.Name;
            TxtBox_Surname.Text = userProfile.Surname;
            DatePicker_DateOfBirth.Date = userProfile.DateOfBirth;
            TxtBox_PhoneNumber.Text = userProfile.PhoneNumber;
            TxtBox_PlaceOfResidence.Text = userProfile.PlaceOfResidence;
            TxtBox_WorkExperience.Text = userProfile.WorkExperience;
            TxtBox_Education.Text = userProfile.Education;
            TxtBox_Languages.Text = userProfile.Languages;
            TxtBox_Skills.Text = userProfile.Skills;
            TxtBox_Courses.Text = userProfile.Courses;

            Title = "Edytuj profil użytkownika";
            btn_UniversalBtn.Text = "EDYTUJ";
        }

        private async void RegisterUserProfile(object sender, EventArgs e)
        {
            string name = TxtBox_Name.Text;
            string surname = TxtBox_Surname.Text;
            DateTime dateOfBirth = DatePicker_DateOfBirth.Date;
            string phoneNumber = TxtBox_PhoneNumber.Text;
            string placeOfResidence = TxtBox_PlaceOfResidence.Text;
            string workExperience = TxtBox_WorkExperience.Text;
            string education = TxtBox_Education.Text;
            string languages = TxtBox_Languages.Text; ;
            string skills = TxtBox_Skills.Text;
            string courses = TxtBox_Courses.Text;

            if
            (
                string.IsNullOrWhiteSpace(name) || name.Length > 40 ||
                string.IsNullOrWhiteSpace(surname) || surname.Length > 40 ||
                dateOfBirth == DateTime.MinValue ||
                //dateOfBirth >= DateTime.Now ||
                string.IsNullOrWhiteSpace(phoneNumber) ||
                string.IsNullOrWhiteSpace(placeOfResidence)
            )
            {
                await DisplayAlert("Błęd", "Podano błędne dane.", "OK");
                return;
            }

            if(!ValidatePhoneNumber(phoneNumber))
            {
                await DisplayAlert("Numer telefonu", "Podany numer telefonu nie jest poprawnym polskim numerem telefonu.", "OK");
                return;
            }

            if (editMode)
            {
                await App.DataBase.UpdateUserProfileAsync(new UserProfile(userProfileId, name, surname, dateOfBirth, phoneNumber, placeOfResidence, workExperience, education, languages, skills, courses));
                await DisplayAlert("Edycja profilu użytkownika", "Edycja powiodła się!", "OK");
                await Navigation.PopAsync();
                return;
            }

            await App.DataBase.CreateUserAsync(user);
            user = await App.DataBase.ReadUserByNameAsync(user.Nickname);
            await App.DataBase.CreateUserProfileAsync(new UserProfile(name, surname, user.Id, dateOfBirth, phoneNumber, placeOfResidence, workExperience, education, languages, skills, courses));

            await DisplayAlert("Rejestracja profilu użytkownika", "Rejestracja powiodła się!", "OK");

            await Navigation.PopToRootAsync();
            return;
        }

        public bool ValidatePhoneNumber(string phoneNumber)
        {
            string phoneNumberWithoutSpaces = Regex.Replace(phoneNumber, @"\s+", "");

            if ((phoneNumberWithoutSpaces.Length == 9 && Regex.IsMatch(phoneNumberWithoutSpaces, @"\d{9}")) ||
                (phoneNumberWithoutSpaces.Length == 12 && Regex.IsMatch(phoneNumberWithoutSpaces, @"\+48\d{9}")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}