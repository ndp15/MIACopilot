using MIACopilotApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace MIACopilotApp.Gui.Dialogs
{
    public partial class TrainerDialog : Window
    {
        public VocationalTrainer? Result { get; private set; }

        private class ComboItem
        {
            public int?   Id          { get; set; }
            public string DisplayName { get; set; } = "";
        }

        // Add mode
        public TrainerDialog()
        {
            InitializeComponent();
            LoadCombos();
        }

        // Edit mode
        public TrainerDialog(VocationalTrainer existing)
        {
            InitializeComponent();
            TitleText.Text  = "Berufsbildner bearbeiten";
            Title           = "Berufsbildner bearbeiten";
            TbFirstName.Text = existing.FirstName;
            TbLastName .Text = existing.LastName;
            LoadCombos(existing.CompanyId);
            Result = existing;
        }

        // Loads companies into the combo box and preselects the current company
        private void LoadCombos(int? selCompany = null)
        {
            CbCompany.Items.Add(new ComboItem { Id = null, DisplayName = "(keine)" });
            foreach (var c in DataStore.Data.Companies)
                CbCompany.Items.Add(new ComboItem { Id = c.Id, DisplayName = c.Name });
            CbCompany.SelectedIndex = 0;

            if (selCompany.HasValue)
            {
                for (int i = 0; i < CbCompany.Items.Count; i++)
                {
                    if (CbCompany.Items[i] is ComboItem item && item.Id == selCompany.Value)
                    {
                        CbCompany.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        // Validates and saves the trainer data from the dialog
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbFirstName.Text) ||
                string.IsNullOrWhiteSpace(TbLastName.Text))
            {
                MessageBox.Show("Vorname und Nachname sind Pflichtfelder.",
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int? companyId = (CbCompany.SelectedItem as ComboItem)?.Id;

            if (Result != null)
            {
                Result.FirstName = TbFirstName.Text.Trim();
                Result.LastName  = TbLastName .Text.Trim();
                Result.CompanyId = companyId;
            }
            else
            {
                Result = new VocationalTrainer
                {
                    FirstName = TbFirstName.Text.Trim(),
                    LastName  = TbLastName .Text.Trim(),
                    CompanyId = companyId
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
