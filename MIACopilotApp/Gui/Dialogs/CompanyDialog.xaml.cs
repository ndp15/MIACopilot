using MIACopilotApp.Models;
using System.Windows;

namespace MIACopilotApp.Gui.Dialogs
{
    public partial class CompanyDialog : Window
    {
        public Company? Result { get; private set; }

        // Add mode
        public CompanyDialog()
        {
            InitializeComponent();
        }

        // Edit mode
        public CompanyDialog(Company existing)
        {
            InitializeComponent();
            TitleText.Text = "Firma bearbeiten";
            Title          = "Firma bearbeiten";
            TbName   .Text = existing.Name;
            TbAddress.Text = existing.Address;
            Result         = existing;
        }

        // Validates and saves the company data from the dialog
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TbName.Text))
            {
                MessageBox.Show("Der Name ist ein Pflichtfeld.",
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Result != null)
            {
                Result.Name    = TbName   .Text.Trim();
                Result.Address = TbAddress.Text.Trim();
            }
            else
            {
                Result = new Company
                {
                    Name    = TbName   .Text.Trim(),
                    Address = TbAddress.Text.Trim()
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
