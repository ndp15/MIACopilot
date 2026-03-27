using MIACopilotApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace MIACopilotApp.Gui.Dialogs
{
    public partial class GradeDialog : Window
    {
        public Grade? Result { get; private set; }

        private class ComboItem
        {
            public int    Id          { get; set; }
            public string DisplayName { get; set; } = "";
        }

        // Add mode
        public GradeDialog()
        {
            InitializeComponent();
            DpDate.SelectedDate = DateTime.Today;
            LoadCombos();
        }

        // Edit mode
        public GradeDialog(Grade existing)
        {
            InitializeComponent();
            TitleText.Text      = "Note bearbeiten";
            Title               = "Note bearbeiten";
            TbSubject.Text      = existing.Subject;
            SliderGrade.Value   = existing.Value;
            TbRemarks.Text      = existing.Remarks ?? "";
            DpDate.SelectedDate = existing.Date;
            LoadCombos(existing.ApprenticeId);
            Result = existing;
        }

        // Loads apprentices into the combo box and preselects the current apprentice
        private void LoadCombos(int selApprentice = 0)
        {
            foreach (var a in DataStore.Data.Apprentices)
                CbApprentice.Items.Add(new ComboItem
                {
                    Id          = a.Id,
                    DisplayName = $"{a.FirstName} {a.LastName}"
                });
            CbApprentice.SelectedIndex = 0;

            if (selApprentice > 0)
            {
                for (int i = 0; i < CbApprentice.Items.Count; i++)
                {
                    if (CbApprentice.Items[i] is ComboItem item && item.Id == selApprentice)
                    {
                        CbApprentice.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        // Updates the grade text display when the slider value changes
        private void SliderGrade_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TbGradeDisplay != null)
                TbGradeDisplay.Text = SliderGrade.Value.ToString("F1");
        }

        // Validates and saves the grade data from the dialog
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbSubject.Text))
            {
                MessageBox.Show("Das Fach ist ein Pflichtfeld.",
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DpDate.SelectedDate == null)
            {
                MessageBox.Show("Bitte ein Datum auswählen.",
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (CbApprentice.SelectedItem == null)
            {
                MessageBox.Show("Bitte einen Lernenden auswählen.",
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int    apprenticeId = ((ComboItem)CbApprentice.SelectedItem).Id;
            double gradeValue   = Math.Round(SliderGrade.Value * 2, MidpointRounding.AwayFromZero) / 2.0;
            string? remarks     = string.IsNullOrWhiteSpace(TbRemarks.Text) ? null : TbRemarks.Text.Trim();

            if (Result != null)
            {
                Result.Subject      = TbSubject.Text.Trim();
                Result.Value        = gradeValue;
                Result.ApprenticeId = apprenticeId;
                Result.Date         = DpDate.SelectedDate.Value;
                Result.Remarks      = remarks;
            }
            else
            {
                Result = new Grade
                {
                    Subject      = TbSubject.Text.Trim(),
                    Value        = gradeValue,
                    ApprenticeId = apprenticeId,
                    Date         = DpDate.SelectedDate.Value,
                    Remarks      = remarks
                };
            }

            DialogResult = true;
        }

        // Closes the dialog without saving changes
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
