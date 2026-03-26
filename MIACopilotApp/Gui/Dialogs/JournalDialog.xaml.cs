using MIACopilotApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace MIACopilotApp.Gui.Dialogs
{
    public partial class JournalDialog : Window
    {
        public WorkJournal? Result { get; private set; }

        private class ComboItem
        {
            public int    Id          { get; set; }
            public string DisplayName { get; set; } = "";
        }

        // Add mode
        public JournalDialog()
        {
            InitializeComponent();
            DpDate.SelectedDate = DateTime.Today;
            LoadCombos();
        }

        // Edit mode
        public JournalDialog(WorkJournal existing)
        {
            InitializeComponent();
            TitleText.Text    = "Arbeitsjournal bearbeiten";
            Title             = "Arbeitsjournal bearbeiten";
            TbTitle  .Text    = existing.Title;
            TbContent.Text    = existing.Content;
            DpDate.SelectedDate = existing.DateEntry;
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

        // Validates and saves the journal data from the dialog
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbTitle.Text))
            {
                MessageBox.Show("Der Titel ist ein Pflichtfeld.",
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

            int apprenticeId = ((ComboItem)CbApprentice.SelectedItem).Id;

            if (Result != null)
            {
                Result.Title       = TbTitle  .Text.Trim();
                Result.Content     = TbContent.Text.Trim();
                Result.DateEntry   = DpDate.SelectedDate.Value;
                Result.ApprenticeId = apprenticeId;
            }
            else
            {
                Result = new WorkJournal
                {
                    Title        = TbTitle  .Text.Trim(),
                    Content      = TbContent.Text.Trim(),
                    DateEntry    = DpDate.SelectedDate.Value,
                    ApprenticeId = apprenticeId
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
