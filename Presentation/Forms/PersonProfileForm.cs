using Business;
using Presentation.Events;
using Presentation.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public enum FormMode
    {
        Add,
        Edit
    }

    public partial class PersonProfileForm : Form
    {
        private FormMode _mode;
        private int _personId;
        private string _imagePath = null;
        private bool _imageChanged = false;
        private string _originalImagePath = null;
        private string _sourceImagePath = null;
        private Person _currentPerson = null;

        public event EventHandler<PersonSavedEventArgs> PersonSaved;

        public int AddedPersonId { get; private set; } = -1;

        public PersonProfileForm()
        {
            InitializeComponent();

            // Subscribing to PersonDetailsControl events
            personDetailsControl.ImageSelected += PersonDetailsControl_ImageSelected;
            personDetailsControl.SaveButtonClicked += PersonDetailsControl_SaveButtonClicked;
            personDetailsControl.ClosebuttonClicked += PersonDetailsControl_CloseButtonClicked;
            personDetailsControl.RemoveImageClicked += PersonDetailsControl_RemoveImageClicked;
            personDetailsControl.NationalNumberValidated += PersonDetailsControl_NationalNumberValidated;

            personDetailsControl.MinimumAge = AppSettings.MinimumDrivingAge;

            _mode = FormMode.Add;
            _personId = -1;
        }

        public PersonProfileForm(int personId)
        {
            InitializeComponent();

            // Subscribing to PersonDetailsControl events
            personDetailsControl.ImageSelected += PersonDetailsControl_ImageSelected;
            personDetailsControl.SaveButtonClicked += PersonDetailsControl_SaveButtonClicked;
            personDetailsControl.ClosebuttonClicked += PersonDetailsControl_CloseButtonClicked;
            personDetailsControl.RemoveImageClicked += PersonDetailsControl_RemoveImageClicked;
            personDetailsControl.NationalNumberValidated += PersonDetailsControl_NationalNumberValidated;

            personDetailsControl.MinimumAge = AppSettings.MinimumDrivingAge;

            _mode = FormMode.Edit;
            _personId = personId;
        }

        private void PersonDetailsControl_NationalNumberValidated(object sender, EventArgs e)
        {
            string nationalNo = personDetailsControl.NationalNo.Trim();

            // Skip validation if empty
            if (string.IsNullOrWhiteSpace(nationalNo))
            {
                personDetailsControl.ErrorProvider.SetError(
                    personDetailsControl.NationalNumberTextBox,
                    "National Number is required.");
                return;
            }

            if (_mode == FormMode.Add)
            {
                // In Add mode: check if national number exists
                if (Person.IsNationalNoExists(nationalNo))
                {
                    personDetailsControl.ErrorProvider.SetError(
                        personDetailsControl.NationalNumberTextBox,
                        "National Number already exists.");
                }
                else
                {
                    personDetailsControl.ErrorProvider.SetError(
                        personDetailsControl.NationalNumberTextBox,
                        "");
                }
            }
            else if (_mode == FormMode.Edit)
            {
                // In Edit mode: reuse cached person
                if (_currentPerson == null)
                {
                    _currentPerson = Person.Find(_personId);
                }

                if (_currentPerson != null && _currentPerson.NationalNo != nationalNo)
                {
                    // National number was changed - check if new one already exists
                    if (Person.IsNationalNoExists(nationalNo))
                    {
                        personDetailsControl.ErrorProvider.SetError(
                            personDetailsControl.NationalNumberTextBox,
                            "National Number already exists for another person.");
                    }
                    else
                    {
                        personDetailsControl.ErrorProvider.SetError(
                            personDetailsControl.NationalNumberTextBox,
                            "");
                    }
                }
                else
                {
                    // National number unchanged - clear any errors
                    personDetailsControl.ErrorProvider.SetError(
                        personDetailsControl.NationalNumberTextBox,
                        "");
                }
            }
        }

        private void PersonDetailsControl_CloseButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PersonDetailsControl_RemoveImageClicked(object sender, EventArgs e)
        {
            // Clear the image paths
            _imagePath = null;
            _sourceImagePath = null;
            _imageChanged = true;

            // Reset to default gender image
            if (personDetailsControl.Gender == 0)
            {
                personDetailsControl.PersonImage.Image = personDetailsControl.DefaultMaleImage;
            }
            else
            {
                personDetailsControl.PersonImage.Image = personDetailsControl.DefaultFemaleImage;
            }

            // Hide the remove link after removing
            personDetailsControl.RemovePersonImageLinkVisible = false;
        }

        private void PersonDetailsControl_SaveButtonClicked(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                MessageBox.Show("Please correct the validation errors before saving.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            Person person;

            if (_mode == FormMode.Add)
            {
                // Create new person
                person = new Person();
            }
            else
            {
                // Load existing person for update
                person = _currentPerson ?? Person.Find(_personId);
                if (person == null)
                {
                    MessageBox.Show("Person not found.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _currentPerson = person;
            }

            // Update person properties from control
            person.FirstName = personDetailsControl.FirstName;
            person.SecondName = personDetailsControl.SecondName;
            person.ThirdName = string.IsNullOrWhiteSpace(personDetailsControl.ThirdName) ? null : personDetailsControl.ThirdName;
            person.LastName = personDetailsControl.LastName;
            person.NationalNo = personDetailsControl.NationalNo;
            person.DateOfBirth = personDetailsControl.DateOfBirth;
            person.Gender = personDetailsControl.Gender;
            person.Address = personDetailsControl.Address;
            person.Phone = personDetailsControl.Phone;
            person.Email = string.IsNullOrWhiteSpace(personDetailsControl.Email) ? null : personDetailsControl.Email;
            person.NationalityCountryID = personDetailsControl.SelectedCountryId;

            // Handle ImagePath based on mode
            if (_mode == FormMode.Add)
            {
                // New person: save image if one was selected
                if (_imageChanged && !string.IsNullOrEmpty(_sourceImagePath))
                {
                    _imagePath = SaveImageFile(_sourceImagePath);
                }
                person.ImagePath = _imagePath;
            }
            else
            {
                // Edit mode: only update if image was changed
                if (_imageChanged)
                {
                    if (!string.IsNullOrEmpty(_sourceImagePath))
                    {
                        // New image selected - save it
                        _imagePath = SaveImageFile(_sourceImagePath);
                    }
                    // else: image was removed (_imagePath = null)
                    
                    person.ImagePath = _imagePath;
                }
            }

            bool saveSucceed = person.Save();
            
            if (saveSucceed)
            {
                if (_mode == FormMode.Edit && _imageChanged)
                {
                    DeleteOldImageFile();
                }

                string successMessage = _mode == FormMode.Add ? "Person Added Successfully." : "Person Updated Successfully.";
                MessageBox.Show(successMessage, "Saved", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                _personId = person.PersonId;
                lblPersonId.Text = _personId.ToString();

                // Switch to Edit mode if this was a new person
                if (_mode == FormMode.Add)
                {
                    AddedPersonId = person.PersonId;
                    PersonSaved?.Invoke(this, new PersonSavedEventArgs(person.PersonId));
                    _mode = FormMode.Edit;
                    SwitchToMode();
                }

                _originalImagePath = _imagePath;
                _sourceImagePath = null;  // Clear source after successful save
                _imageChanged = false;
            }
            else
            {
                string errorMessage = _mode == FormMode.Add ? "Unable to save new person." : "Unable to update person.";
                MessageBox.Show(errorMessage, "Failed to Save",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            string nationalNo = personDetailsControl.NationalNo.Trim();

            // Validate National Number is not empty
            if (string.IsNullOrWhiteSpace(nationalNo))
            {
                personDetailsControl.ErrorProvider.SetError(
                    personDetailsControl.NationalNumberTextBox,
                    "National Number is required.");
                isValid = false;
            }
            // Check for duplicates
            else if (_mode == FormMode.Add && Person.IsNationalNoExists(nationalNo))
            {
                personDetailsControl.ErrorProvider.SetError(
                    personDetailsControl.NationalNumberTextBox,
                    "National Number already exists.");
                isValid = false;
            }
            else if (_mode == FormMode.Edit)
            {
                if (_currentPerson == null)
                {
                    _currentPerson = Person.Find(_personId);
                }

                if (_currentPerson != null && _currentPerson.NationalNo != nationalNo
                    && Person.IsNationalNoExists(nationalNo))
                {
                    personDetailsControl.ErrorProvider.SetError(
                        personDetailsControl.NationalNumberTextBox,
                        "National Number already exists for another person.");
                    isValid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(personDetailsControl.FirstName))
            {
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(personDetailsControl.LastName))
            {
                isValid = false;
            }

            if (string.IsNullOrEmpty(personDetailsControl.SecondName))
            {
                isValid = false;
            }

            if (string.IsNullOrEmpty(personDetailsControl.Phone))
            {
                isValid = false;
            }

            if (string.IsNullOrEmpty(personDetailsControl.Address))
            {
                isValid = false;
            }

            return isValid;
        }

        private void PersonProfileForm_Load(object sender, EventArgs e)
        {
            LoadCountries();
            SwitchToMode();

            // Load person data if in Edit mode
            if (_mode == FormMode.Edit)
            {
                LoadPersonData();
            }
        }

        private void LoadPersonData()
        {
            Person person = Person.Find(_personId);
            if (person == null)
            {
                MessageBox.Show("Person not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            _currentPerson = person;

            // Populate the control with person data
            personDetailsControl.FirstName = person.FirstName;
            personDetailsControl.SecondName = person.SecondName;
            personDetailsControl.ThirdName = person.ThirdName;
            personDetailsControl.LastName = person.LastName;
            personDetailsControl.NationalNo = person.NationalNo;
            personDetailsControl.DateOfBirth = person.DateOfBirth;
            personDetailsControl.Gender = person.Gender;
            personDetailsControl.Address = person.Address;
            personDetailsControl.Phone = person.Phone;
            personDetailsControl.Email = person.Email;
            personDetailsControl.SelectedCountryId = person.NationalityCountryID;

            // Load image if exists
            if (!string.IsNullOrEmpty(person.ImagePath) && File.Exists(person.ImagePath))
            {
                using (var img = Image.FromFile(person.ImagePath))
                {
                    personDetailsControl.PersonImage.Image = new Bitmap(img);
                }
                _imagePath = person.ImagePath;
                _originalImagePath = person.ImagePath;
                personDetailsControl.RemovePersonImageLinkVisible = true;
            }
            else
            {
                // Set default image based on gender
                if (person.Gender == 0)
                {
                    personDetailsControl.PersonImage.Image = personDetailsControl.DefaultMaleImage;
                }
                else
                {
                    personDetailsControl.PersonImage.Image = personDetailsControl.DefaultFemaleImage;
                }
                _originalImagePath = null;
                personDetailsControl.RemovePersonImageLinkVisible = false;
            }

            lblPersonId.Text = person.PersonId.ToString();
        }

        private void LoadCountries()
        {
            try
            {
                DataTable countries = Country.GetAllCountries();
                personDetailsControl.LoadCountries(countries);

                // Find and set default country by name from config
                string defaultCountryName = AppSettings.DefaultCountryName;
                DataRow defaultCountry = countries.AsEnumerable().FirstOrDefault(c =>
                    c.Field<string>("CountryName").Equals(defaultCountryName, StringComparison.OrdinalIgnoreCase));

                if (defaultCountry != null)
                {
                    personDetailsControl.SelectedCountryId = defaultCountry.Field<int>("CountryId");
                }
                else
                {
                    // Fallback: if country name not found, select the first one
                    if (countries.Rows.Count > 0)
                    {
                        personDetailsControl.SelectedCountryId = countries.Rows[0].Field<int>("CountryId");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading countries: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SwitchToMode()
        {
            if (_mode == FormMode.Add)
            {
                lblTitle.Text = "Add New Person";
                personDetailsControl.RemovePersonImageLinkVisible = false;
            }
            else if (_mode == FormMode.Edit)
            {
                lblTitle.Text = "Update Person";
            }
        }

        private void PersonDetailsControl_ImageSelected(object sender, ImageSelectedEventArgs e)
        {
            // Store the source image path
            _sourceImagePath = e.FilePath;
            _imageChanged = true;

            try
            {
                using (var img = Image.FromFile(_sourceImagePath))
                {
                    personDetailsControl.PersonImage.Image = new Bitmap(img);
                }

                personDetailsControl.RemovePersonImageLinkVisible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _sourceImagePath = null;
                _imageChanged = false;
            }
        }

        private string SaveImageFile(string sourceFilePath)
        {
            try
            {
                string targetDirectory = ConfigurationManager.AppSettings["PersonImagesDirectory"];

                if (string.IsNullOrEmpty(targetDirectory))
                {
                    MessageBox.Show(
                        "PersonImagesDirectory is not configured in App.config",
                        "Configuration Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return null;
                }

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                // Generate GUID filename
                string extension = Path.GetExtension(sourceFilePath);
                string newFileName = Guid.NewGuid().ToString() + extension;
                string destinationPath = Path.Combine(targetDirectory, newFileName);

                // Copy file to destination
                File.Copy(sourceFilePath, destinationPath);

                return destinationPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving image: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void DeleteOldImageFile()
        {
            // Only delete if there was an original image and it's different from the new one
            if (!string.IsNullOrEmpty(_originalImagePath) &&
                _originalImagePath != _imagePath &&
                File.Exists(_originalImagePath))
            {
                try
                {
                    File.Delete(_originalImagePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Warning: Could not delete old image file.\n{ex.Message}",
                        "File Deletion Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void personDetailsControl_Load(object sender, EventArgs e)
        {

        }
    }
}
