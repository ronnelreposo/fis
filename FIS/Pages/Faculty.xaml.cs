using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FIS.Lib;
using static System.Tuple;
using static System.DateTime;
using static System.String;
using static System.Reactive.Linq.Observable;
using FIS.Extensions;
using static FIS.Lib.DatabaseQuery;
using static FIS.Lib.Option<System.DateTime?>;
using FirstFloor.ModernUI.Windows.Controls;
using static FirstFloor.ModernUI.Windows.Controls.ModernDialog;

namespace FIS.Pages
{
    /// <summary>
    /// Interaction logic for Faculty.xaml
    /// </summary>
    public partial class Faculty : UserControl
    {

        /// <summary>
        /// Tries to parse the given string.
        /// If an exception occcured during parsing, it returns 0.
        /// </summary>
        /// <param name="age">given age.</param>
        /// <returns>parsed age or 0.</returns>
        int intTryParse(string age)
        {
            try { return Math.Abs(int.Parse(age)); }
            catch ( Exception ) { return 0; }
        }

        public Faculty ()
        {
            InitializeComponent();

            var sFieldsOnReg = from evt in facultyregisterButton.StreamClickEvent()
                               let dateOfBirth = dateofbirthDatePickerTextBox.SelectedDate
                               let dateOfBirthOptional = dateOfBirth == null ? None : Some(dateOfBirth.Value)
                               let dateHired = dateHiredDatePickerTextBox.SelectedDate
                               let dateHiredOptional = dateHired == null ? None : Some(dateHired.Value)
                               select new
                               {
                                   BasicInfo = new
                                   {
                                       ID = idTextBox.Text,
                                       FirstName = firstnameTextBox.Text,
                                       MiddleName = middlenameTextBox.Text,
                                       LastName = lastnameTextBox.Text,
                                       Age = intTryParse(ageTextBox.Text),
                                       DateOfBirth = dateOfBirthOptional,
                                       PlaceOfBirth = placeofbirthTextBox.Text
                                   },
                                   ContactInfo = new
                                   {
                                       Address = addressTextBox.Text,
                                       MobilePhone = mobilePhoneTextBox.Text
                                   },
                                   EmploymentInfo = new
                                   {
                                       DateHired = dateHiredOptional,
                                       Status = statusTextBox.Text
                                   }
                               }; /*end sFieldsOnReg. */

            /* find errors */
            var sFieldsAndErrorOnReg = from fields in sFieldsOnReg
                                       let basicInfo = fields.BasicInfo
                                       let isIdEmpty = IsNullOrEmpty(basicInfo.ID)
                                       let isFirstNameEmpty = IsNullOrEmpty(basicInfo.FirstName)
                                       let isMiddleNameEmpty = IsNullOrEmpty(basicInfo.MiddleName)
                                       let isLastNameEmpty = IsNullOrEmpty(basicInfo.LastName)
                                       let isInvalidAge = basicInfo.Age.Equals(0)
                                       let isDateOfBirthNotSelected = basicInfo.DateOfBirth.IsNone
                                       let isPlaceOfBirthEmpty = IsNullOrEmpty(basicInfo.PlaceOfBirth)
                                       let contactInfo = fields.ContactInfo
                                       let isAddressEmpty = IsNullOrEmpty(contactInfo.Address)
                                       let isMobilePhoneEmpty = IsNullOrEmpty(contactInfo.MobilePhone)
                                       let employeeInfo = fields.EmploymentInfo
                                       let isDateHiredNotSelected = employeeInfo.DateHired.IsNone
                                       let isStatusEmpty = IsNullOrEmpty(employeeInfo.Status)
                                       select new
                                       {
                                           Fields = fields,
                                           Error = isIdEmpty ? Option<string>.Some(attachRequired("ID")) :
                                               isFirstNameEmpty ? Option<string>.Some(attachRequired("First Name")) :
                                               isMiddleNameEmpty ? Option<string>.Some(attachRequired("Middle Name")) :
                                               isLastNameEmpty ? Option<string>.Some(attachRequired("Last Name")) :
                                               isInvalidAge ? Option<string>.Some("Please enter a valid Age") :
                                               isDateOfBirthNotSelected ? Option<string>.Some("Please select your Date of Birth.") :
                                               isPlaceOfBirthEmpty ? Option<string>.Some(attachRequired("Place of Birth")) :
                                               isAddressEmpty ? Option<string>.Some(attachRequired("Address")) :
                                               isMobilePhoneEmpty ? Option<string>.Some(attachRequired("Mobile Phone")) :
                                               isDateHiredNotSelected ? Option<string>.Some("Please select your hired date.") :
                                               isStatusEmpty ? Option<string>.Some(attachRequired("Status")) :
                                               Option<string>.None
                                       }; /* end sFieldsAndErrorOnReg. */

            /* has error */
            ( from x in sFieldsAndErrorOnReg
              let error = x.Error
              where error.IsSome
              select error.Value )
                .Subscribe(err => ShowMessage(err, string.Empty, MessageBoxButton.OK));

            /* has *no error */
            ( from x in sFieldsAndErrorOnReg
              where x.Error.IsNone
              select x.Fields )
                .Subscribe(/*async*/ async fields =>
                {
                    var basicInfo = fields.BasicInfo;
                    var contactInfo = fields.ContactInfo;
                    var employmentInfo = fields.EmploymentInfo;
                    var param_value_xs = Create<string[], object[]>(
                        new[] { "id", "firstname", "middlename",
                            "lastname", "age",
                            "address", "cellphone",
                            "dateOfBirth", "placeOfBirth",
                            "dateHired", "status" },
                        new[] {
                            basicInfo.ID, basicInfo.FirstName, basicInfo.MiddleName,
                            basicInfo.LastName, basicInfo.Age.ToString(),
                            contactInfo.Address, contactInfo.MobilePhone,
                            (basicInfo.DateOfBirth.Value).Value.ToShortDateString(), basicInfo.PlaceOfBirth,
                            (employmentInfo.DateHired.Value).Value.ToShortDateString(), employmentInfo.Status
                        });

                    await QueryAsync("add_faculty", param_value_xs,
                        ex => MessageBox.Show("Faculty Member unable to save."),
                        _ => MessageBox.Show("Faculty Member Saved."));
                });
        }

        string attachRequired(string field) => field + " is Required.";

        private void facultyupdateButton_Click (object sender, RoutedEventArgs e)
        {
            facultyupdateButton.Visibility = Visibility.Collapsed;
            facultyregisterButton.Visibility = Visibility.Visible;
        }

        private void facultydeleteButton_Click (object sender, RoutedEventArgs e)
        {
            facultydeleteButton.Visibility = Visibility.Collapsed;
            facultyclearButton.Visibility = Visibility.Visible;
        }

        private void facultyclearButton_Click (object sender, RoutedEventArgs e)
        {
            facultydeleteButton.Visibility = Visibility.Visible;
            facultyclearButton.Visibility = Visibility.Collapsed;
        }

        private void personalprofile_MouseEnter (object sender, MouseEventArgs e)
        {
            idTextBox.Text = "HELLO WORLD";
        }
    }
}