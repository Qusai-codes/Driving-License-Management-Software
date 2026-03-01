using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Presentation.Helpers
{
    internal static class FilterHelper
    {
        private static readonly (string Display, string Column)[] PeopleFilters =
        {
            ("None", null),
            ("Person ID", "PersonId"),
            ("National No.", "NationalNo"),
            ("First Name", "FirstName"),
            ("Second Name", "SecondName"),
            ("Third Name", "ThirdName"),
            ("Last Name", "LastName"),
            ("Nationality", "NationalityCountryId"),
            ("Gender", "Gender"),
            ("Phone", "Phone"),
            ("Email", "Email")
        };

        public static void PopulateFilters(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            foreach (var item in PeopleFilters)
                comboBox.Items.Add(item.Display);
            comboBox.SelectedIndex = 0;
        }

        public static string GetColumnName(string displayName)
        {
            foreach (var item in PeopleFilters)
            {
                if (string.Equals(item.Display, displayName, StringComparison.OrdinalIgnoreCase))
                    return item.Column;
            }
            return null;
        }
    }
}