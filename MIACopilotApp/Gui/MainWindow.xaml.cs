using MIACopilotApp.Gui.Dialogs;
using MIACopilotApp.Models;
using System.Windows;
using System.Windows.Controls;

namespace MIACopilotApp.Gui
{
    public partial class MainWindow : Window
    {
        private class ApprenticeVM
        {
            public int    Id                  { get; }
            public string FirstName           { get; }
            public string LastName            { get; }
            public int?   CompanyId           { get; }
            public int?   VocationalTrainerId { get; }

            public string CompanyName =>
                CompanyId.HasValue
                    ? DataStore.Data.Companies.FirstOrDefault(c => c.Id == CompanyId)?.Name ?? "—"
                    : "—";

            public string TrainerName =>
                VocationalTrainerId.HasValue
                    ? DataStore.Data.VocationalTrainers.FirstOrDefault(t => t.Id == VocationalTrainerId)
                          is VocationalTrainer vt ? $"{vt.FirstName} {vt.LastName}" : "—"
                    : "—";

            // Creates an apprentice view model from a domain object
            public ApprenticeVM(Apprentice a)
            {
                Id = a.Id; FirstName = a.FirstName; LastName = a.LastName;
                CompanyId = a.CompanyId; VocationalTrainerId = a.VocationalTrainerId;
            }
        }

        private class TrainerVM
        {
            public int    Id          { get; }
            public string FirstName   { get; }
            public string LastName    { get; }
            public int?   CompanyId   { get; }

            public string CompanyName =>
                CompanyId.HasValue
                    ? DataStore.Data.Companies.FirstOrDefault(c => c.Id == CompanyId)?.Name ?? "—"
                    : "—";

            // Creates a trainer view model from a domain object
            public TrainerVM(VocationalTrainer t)
            {
                Id = t.Id; FirstName = t.FirstName; LastName = t.LastName;
                CompanyId = t.CompanyId;
            }
        }

        private class GradeVM
        {
            public int      Id             { get; }
            public string   Subject        { get; }
            public double   Value          { get; }
            public int      ApprenticeId   { get; }
            public DateTime Date           { get; }
            public string?  Remarks        { get; }

            public string ValueDisplay =>
                Value.ToString("F1", System.Globalization.CultureInfo.InvariantCulture);

            public string ApprenticeName =>
                DataStore.Data.Apprentices.FirstOrDefault(a => a.Id == ApprenticeId)
                    is Apprentice ap ? $"{ap.FirstName} {ap.LastName}" : "—";

            // Creates a grade view model from a domain object
            public GradeVM(Grade g)
            {
                Id = g.Id; Subject = g.Subject; Value = g.Value;
                ApprenticeId = g.ApprenticeId; Date = g.Date; Remarks = g.Remarks;
            }
        }

        private class JournalVM
        {
            public int      Id             { get; }
            public string   Title          { get; }
            public string   Content        { get; }
            public DateTime DateEntry      { get; }
            public int      ApprenticeId   { get; }

            public string ApprenticeName =>
                DataStore.Data.Apprentices.FirstOrDefault(a => a.Id == ApprenticeId)
                    is Apprentice ap ? $"{ap.FirstName} {ap.LastName}" : "—";

            public string ContentPreview =>
                Content?.Length > 70 ? Content[..70] + "…" : Content ?? "";

            // Creates a journal view model from a domain object
            public JournalVM(WorkJournal j)
            {
                Id = j.Id; Title = j.Title; Content = j.Content;
                DateEntry = j.DateEntry; ApprenticeId = j.ApprenticeId;
            }
        }

        private class PersonFilterItem(string label, string role = "Alle", int id = 0)
        {
            public string Label { get; } = label;
            public string Role  { get; } = role;  // "Alle", "Lernender", "Berufsbildner"
            public int    Id    { get; } = id;
            public override string ToString() => Label;
        }

        // Populates a person-filter ComboBox with "Alle", apprentices, and/or trainers
        private static void PopulatePersonFilter(ComboBox cb, bool includeLernende, bool includeBerufsbildner)
        {
            var prev = cb.SelectedItem as PersonFilterItem;
            cb.Items.Clear();
            cb.Items.Add(new PersonFilterItem("Alle"));

            if (includeLernende)
            {
                var apprentices = DataStore.Data.Apprentices;
                if (apprentices.Count > 0)
                {
                    cb.Items.Add(new ComboBoxItem
                    {
                        Content = "Lernende", IsEnabled = false,
                        Foreground = System.Windows.Media.Brushes.Gray,
                        FontSize = 11, FontWeight = FontWeights.SemiBold,
                        Padding = new Thickness(8, 4, 8, 2)
                    });
                    foreach (var a in apprentices)
                        cb.Items.Add(new PersonFilterItem($"  {a.FirstName} {a.LastName}", "Lernender", a.Id));
                }
            }

            if (includeBerufsbildner)
            {
                var trainers = DataStore.Data.VocationalTrainers;
                if (trainers.Count > 0)
                {
                    cb.Items.Add(new ComboBoxItem
                    {
                        Content = "Berufsbildner", IsEnabled = false,
                        Foreground = System.Windows.Media.Brushes.Gray,
                        FontSize = 11, FontWeight = FontWeights.SemiBold,
                        Padding = new Thickness(8, 4, 8, 2)
                    });
                    foreach (var t in trainers)
                        cb.Items.Add(new PersonFilterItem($"  {t.FirstName} {t.LastName}", "Berufsbildner", t.Id));
                }
            }

            // Restore previous selection if still valid
            if (prev != null && prev.Role != "Alle")
            {
                var match = cb.Items.OfType<PersonFilterItem>()
                    .FirstOrDefault(p => p.Role == prev.Role && p.Id == prev.Id);
                cb.SelectedItem = match ?? cb.Items[0];
            }
            else
            {
                cb.SelectedIndex = 0;
            }
        }

        // Initializes the main window and loads all data grids
        public MainWindow()
        {
            InitializeComponent();
            PopulatePersonFilter(CbApprenticePersonFilter, includeLernende: false, includeBerufsbildner: true);
            PopulatePersonFilter(CbTrainerPersonFilter,   includeLernende: true,  includeBerufsbildner: false);
            PopulatePersonFilter(CbJournalPersonFilter,   includeLernende: true,  includeBerufsbildner: true);
            PopulatePersonFilter(CbGradePersonFilter,     includeLernende: true,  includeBerufsbildner: true);
            RefreshAll();
        }

        // Shows the apprentices panel when its navigation button is selected
        private void NavApprentices_Checked(object sender, RoutedEventArgs e) => ShowPanel(0);
        // Shows the companies panel when its navigation button is selected
        private void NavCompanies_Checked  (object sender, RoutedEventArgs e) => ShowPanel(1);
        // Shows the trainers panel when its navigation button is selected
        private void NavTrainers_Checked   (object sender, RoutedEventArgs e) => ShowPanel(2);
        // Shows the journals panel when its navigation button is selected
        private void NavJournals_Checked   (object sender, RoutedEventArgs e) => ShowPanel(3);
        // Shows the grades panel when its navigation button is selected
        private void NavGrades_Checked     (object sender, RoutedEventArgs e) => ShowPanel(4);

        // Toggles visibility so only the selected management panel is visible
        private void ShowPanel(int index)
        {
            if (PanelApprentices == null) return;
            PanelApprentices.Visibility = index == 0 ? Visibility.Visible : Visibility.Collapsed;
            PanelCompanies  .Visibility = index == 1 ? Visibility.Visible : Visibility.Collapsed;
            PanelTrainers   .Visibility = index == 2 ? Visibility.Visible : Visibility.Collapsed;
            PanelJournals   .Visibility = index == 3 ? Visibility.Visible : Visibility.Collapsed;
            PanelGrades     .Visibility = index == 4 ? Visibility.Visible : Visibility.Collapsed;
        }

        // Refreshes all tabs and their data sources
        private void RefreshAll()
        {
            RefreshApprentices();
            RefreshCompanies();
            RefreshTrainers();
            RefreshJournals();
            RefreshGrades();
        }

        private string _apprenticeFilter = "";
        private PersonFilterItem? _apprenticePersonFilter;

        // Refreshes the apprentices grid using the active search and person filters
        private void RefreshApprentices()
        {
            var all = DataStore.Data.Apprentices;
            IEnumerable<Apprentice> filtered = all;

            if (_apprenticePersonFilter?.Role == "Berufsbildner")
                filtered = filtered.Where(a => a.VocationalTrainerId == _apprenticePersonFilter.Id);
            else if (_apprenticePersonFilter?.Role == "Lernender")
                filtered = filtered.Where(a => a.Id == _apprenticePersonFilter.Id);

            var items = filtered
                .Where(a => string.IsNullOrEmpty(_apprenticeFilter) ||
                            a.FirstName.Contains(_apprenticeFilter, StringComparison.OrdinalIgnoreCase) ||
                            a.LastName .Contains(_apprenticeFilter, StringComparison.OrdinalIgnoreCase))
                .Select(a => new ApprenticeVM(a))
                .ToList();

            GridApprentices.ItemsSource = items;
            TxtApprenticeCount.Text = items.Count == all.Count
                ? $"{all.Count} Einträge"
                : $"{items.Count} von {all.Count} Einträgen";

            BtnEditApprentice  .IsEnabled = false;
            BtnDeleteApprentice.IsEnabled = false;
        }

        // Applies person filter and refreshes the apprentices grid
        private void CbApprenticePersonFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _apprenticePersonFilter = CbApprenticePersonFilter.SelectedItem as PersonFilterItem;
            RefreshApprentices();
        }

        // Applies apprentice search text and refreshes the grid
        private void TbApprenticeSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _apprenticeFilter = TbApprenticeSearch.Text;
            RefreshApprentices();
        }

        // Enables or disables apprentice action buttons based on selection
        private void GridApprentices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool sel = GridApprentices.SelectedItem != null;
            BtnEditApprentice  .IsEnabled = sel;
            BtnDeleteApprentice.IsEnabled = sel;
        }

        // Opens the add apprentice dialog and creates a new apprentice
        private void BtnAddApprentice_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ApprenticeDialog { Owner = this };
            if (dlg.ShowDialog() == true && dlg.Result != null)
            {
                Apprentice.Create(dlg.Result);
                RefreshApprentices();
            }
        }

        // Opens the edit apprentice dialog and updates the selected apprentice
        private void BtnEditApprentice_Click(object sender, RoutedEventArgs e)
        {
            if (GridApprentices.SelectedItem is not ApprenticeVM vm) return;
            var apprentice = Apprentice.Read(vm.Id);
            if (apprentice == null) return;
            var dlg = new ApprenticeDialog(apprentice) { Owner = this };
            if (dlg.ShowDialog() == true && dlg.Result != null)
            {
                Apprentice.Update(dlg.Result);
                RefreshApprentices();
            }
        }

        // Deletes the selected apprentice after confirmation
        private void BtnDeleteApprentice_Click(object sender, RoutedEventArgs e)
        {
            if (GridApprentices.SelectedItem is not ApprenticeVM vm) return;
            if (MessageBox.Show($"Lernenden \"{vm.FirstName} {vm.LastName}\" wirklich löschen?",
                    "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Apprentice.Delete(vm.Id);
                RefreshApprentices();
            }
        }

        private string _companyFilter = "";

        // Refreshes the companies grid using the active search filter
        private void RefreshCompanies()
        {
            var all   = DataStore.Data.Companies;
            var items = all
                .Where(c => string.IsNullOrEmpty(_companyFilter) ||
                            c.Name   .Contains(_companyFilter, StringComparison.OrdinalIgnoreCase) ||
                            (c.Address ?? "").Contains(_companyFilter, StringComparison.OrdinalIgnoreCase))
                .ToList();

            GridCompanies.ItemsSource = items;
            TxtCompanyCount.Text = items.Count == all.Count
                ? $"{all.Count} Einträge"
                : $"{items.Count} von {all.Count} Einträgen";

            BtnEditCompany  .IsEnabled = false;
            BtnDeleteCompany.IsEnabled = false;
        }

        // Applies company search text and refreshes the grid
        private void TbCompanySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _companyFilter = TbCompanySearch.Text;
            RefreshCompanies();
        }

        // Enables or disables company action buttons based on selection
        private void GridCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool sel = GridCompanies.SelectedItem != null;
            BtnEditCompany  .IsEnabled = sel;
            BtnDeleteCompany.IsEnabled = sel;
        }

        // Opens the add company dialog and creates a new company
        private void BtnAddCompany_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CompanyDialog { Owner = this };
            if (dlg.ShowDialog() == true && dlg.Result != null)
            {
                Company.Create(dlg.Result);
                RefreshCompanies();
            }
        }

        // Opens the edit company dialog and updates the selected company
        private void BtnEditCompany_Click(object sender, RoutedEventArgs e)
        {
            if (GridCompanies.SelectedItem is not Company company) return;
            var dlg = new CompanyDialog(company) { Owner = this };
            if (dlg.ShowDialog() == true && dlg.Result != null)
            {
                Company.Update(dlg.Result);
                RefreshCompanies();
            }
        }

        // Deletes the selected company after confirmation
        private void BtnDeleteCompany_Click(object sender, RoutedEventArgs e)
        {
            if (GridCompanies.SelectedItem is not Company company) return;
            if (MessageBox.Show($"Firma \"{company.Name}\" wirklich löschen?",
                    "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Company.Delete(company.Id);
                RefreshCompanies();
            }
        }

        private string _trainerFilter = "";
        private PersonFilterItem? _trainerPersonFilter;

        // Refreshes the trainers grid using the active search and person filters
        private void RefreshTrainers()
        {
            var all = DataStore.Data.VocationalTrainers;
            IEnumerable<VocationalTrainer> filtered = all;

            if (_trainerPersonFilter?.Role == "Lernender")
            {
                var apprentice = DataStore.Data.Apprentices.FirstOrDefault(a => a.Id == _trainerPersonFilter.Id);
                if (apprentice?.VocationalTrainerId.HasValue == true)
                    filtered = filtered.Where(t => t.Id == apprentice.VocationalTrainerId);
                else
                    filtered = Enumerable.Empty<VocationalTrainer>();
            }

            var items = filtered
                .Where(t => string.IsNullOrEmpty(_trainerFilter) ||
                            t.FirstName.Contains(_trainerFilter, StringComparison.OrdinalIgnoreCase) ||
                            t.LastName .Contains(_trainerFilter, StringComparison.OrdinalIgnoreCase))
                .Select(t => new TrainerVM(t))
                .ToList();

            GridTrainers.ItemsSource = items;
            TxtTrainerCount.Text = items.Count == all.Count
                ? $"{all.Count} Einträge"
                : $"{items.Count} von {all.Count} Einträgen";

            BtnEditTrainer  .IsEnabled = false;
            BtnDeleteTrainer.IsEnabled = false;
        }

        // Applies person filter and refreshes the trainers grid
        private void CbTrainerPersonFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _trainerPersonFilter = CbTrainerPersonFilter.SelectedItem as PersonFilterItem;
            RefreshTrainers();
        }

        // Applies trainer search text and refreshes the grid
        private void TbTrainerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _trainerFilter = TbTrainerSearch.Text;
            RefreshTrainers();
        }

        // Enables or disables trainer action buttons based on selection
        private void GridTrainers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool sel = GridTrainers.SelectedItem != null;
            BtnEditTrainer  .IsEnabled = sel;
            BtnDeleteTrainer.IsEnabled = sel;
        }

        // Opens the add trainer dialog and creates a new trainer
        private void BtnAddTrainer_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new TrainerDialog { Owner = this };
            if (dlg.ShowDialog() == true && dlg.Result != null)
            {
                VocationalTrainer.Create(dlg.Result);
                RefreshTrainers();
            }
        }

        // Opens the edit trainer dialog and updates the selected trainer
        private void BtnEditTrainer_Click(object sender, RoutedEventArgs e)
        {
            if (GridTrainers.SelectedItem is not TrainerVM vm) return;
            var trainer = VocationalTrainer.Read(vm.Id);
            if (trainer == null) return;
            var dlg = new TrainerDialog(trainer) { Owner = this };
            if (dlg.ShowDialog() == true && dlg.Result != null)
            {
                VocationalTrainer.Update(dlg.Result);
                RefreshTrainers();
            }
        }

        // Deletes the selected trainer after confirmation
        private void BtnDeleteTrainer_Click(object sender, RoutedEventArgs e)
        {
            if (GridTrainers.SelectedItem is not TrainerVM vm) return;
            if (MessageBox.Show($"Berufsbildner \"{vm.FirstName} {vm.LastName}\" wirklich löschen?",
                    "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                VocationalTrainer.Delete(vm.Id);
                RefreshTrainers();
            }
        }

        private string _journalFilter = "";
        private PersonFilterItem? _journalPersonFilter;

        // Refreshes the journals grid using the active search and person filters
        private void RefreshJournals()
        {
            var all = DataStore.Data.WorkJournals;
            IEnumerable<WorkJournal> filtered = all;

            if (_journalPersonFilter?.Role == "Lernender")
                filtered = filtered.Where(j => j.ApprenticeId == _journalPersonFilter.Id);
            else if (_journalPersonFilter?.Role == "Berufsbildner")
            {
                var ids = DataStore.Data.Apprentices
                    .Where(a => a.VocationalTrainerId == _journalPersonFilter.Id)
                    .Select(a => a.Id).ToHashSet();
                filtered = filtered.Where(j => ids.Contains(j.ApprenticeId));
            }

            var items = filtered
                .Where(j => string.IsNullOrEmpty(_journalFilter) ||
                            j.Title  .Contains(_journalFilter, StringComparison.OrdinalIgnoreCase) ||
                            (j.Content ?? "").Contains(_journalFilter, StringComparison.OrdinalIgnoreCase))
                .Select(j => new JournalVM(j))
                .ToList();

            GridJournals.ItemsSource = items;
            TxtJournalCount.Text = items.Count == all.Count
                ? $"{all.Count} Einträge"
                : $"{items.Count} von {all.Count} Einträgen";

            BtnEditJournal  .IsEnabled = false;
            BtnDeleteJournal.IsEnabled = false;
        }

        // Applies person filter and refreshes the journals grid
        private void CbJournalPersonFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _journalPersonFilter = CbJournalPersonFilter.SelectedItem as PersonFilterItem;
            RefreshJournals();
        }

        // Applies journal search text and refreshes the grid
        private void TbJournalSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _journalFilter = TbJournalSearch.Text;
            RefreshJournals();
        }

        // Enables or disables journal action buttons based on selection
        private void GridJournals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool sel = GridJournals.SelectedItem != null;
            BtnEditJournal  .IsEnabled = sel;
            BtnDeleteJournal.IsEnabled = sel;
        }

        // Opens the add journal dialog and creates a new journal entry
        private void BtnAddJournal_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new JournalDialog { Owner = this };
            if (dlg.ShowDialog() == true && dlg.Result != null)
            {
                WorkJournal.Create(dlg.Result);
                RefreshJournals();
            }
        }

        // Opens the edit journal dialog and updates the selected journal entry
        private void BtnEditJournal_Click(object sender, RoutedEventArgs e)
        {
            if (GridJournals.SelectedItem is not JournalVM vm) return;
            var journal = WorkJournal.Read(vm.Id);
            if (journal == null) return;
            var dlg = new JournalDialog(journal) { Owner = this };
            if (dlg.ShowDialog() == true && dlg.Result != null)
            {
                WorkJournal.Update(dlg.Result);
                RefreshJournals();
            }
        }

        // Deletes the selected journal entry after confirmation
        private void BtnDeleteJournal_Click(object sender, RoutedEventArgs e)
        {
            if (GridJournals.SelectedItem is not JournalVM vm) return;
            if (MessageBox.Show($"Arbeitsjournal \"{vm.Title}\" wirklich löschen?",
                    "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                WorkJournal.Delete(vm.Id);
                RefreshJournals();
            }
        }

        private string _gradeFilter = "";
        private PersonFilterItem? _gradePersonFilter;

        // Refreshes the grades grid using the active search and person filters; updates average
        private void RefreshGrades()
        {
            var all = DataStore.Data.Grades;
            IEnumerable<Grade> filtered = all;

            if (_gradePersonFilter?.Role == "Lernender")
                filtered = filtered.Where(g => g.ApprenticeId == _gradePersonFilter.Id);
            else if (_gradePersonFilter?.Role == "Berufsbildner")
            {
                var ids = DataStore.Data.Apprentices
                    .Where(a => a.VocationalTrainerId == _gradePersonFilter.Id)
                    .Select(a => a.Id).ToHashSet();
                filtered = filtered.Where(g => ids.Contains(g.ApprenticeId));
            }

            var items = filtered
                .Where(g => string.IsNullOrEmpty(_gradeFilter) ||
                            g.Subject.Contains(_gradeFilter, StringComparison.OrdinalIgnoreCase) ||
                            (g.Remarks ?? "").Contains(_gradeFilter, StringComparison.OrdinalIgnoreCase))
                .Select(g => new GradeVM(g))
                .ToList();

            GridGrades.ItemsSource = items;
            TxtGradeCount.Text = items.Count == all.Count
                ? $"{all.Count} Einträge"
                : $"{items.Count} von {all.Count} Einträgen";

            TxtGradeAverage.Text = items.Count > 0
                ? $"Ø  {items.Average(g => g.Value):F2}"
                : "";

            BtnEditGrade  .IsEnabled = false;
            BtnDeleteGrade.IsEnabled = false;
        }

        // Applies person filter and refreshes the grades grid
        private void CbGradePersonFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _gradePersonFilter = CbGradePersonFilter.SelectedItem as PersonFilterItem;
            RefreshGrades();
        }

        // Applies grade search text and refreshes the grid
        private void TbGradeSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _gradeFilter = TbGradeSearch.Text;
            RefreshGrades();
        }

        // Enables or disables grade action buttons based on selection
        private void GridGrades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool sel = GridGrades.SelectedItem != null;
            BtnEditGrade  .IsEnabled = sel;
            BtnDeleteGrade.IsEnabled = sel;
        }

        // Opens the add grade dialog and creates a new grade
        private void BtnAddGrade_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new GradeDialog { Owner = this };
            if (dlg.ShowDialog() == true && dlg.Result != null)
            {
                Grade.Create(dlg.Result);
                RefreshGrades();
            }
        }

        // Opens the edit grade dialog and updates the selected grade
        private void BtnEditGrade_Click(object sender, RoutedEventArgs e)
        {
            if (GridGrades.SelectedItem is not GradeVM vm) return;
            var grade = Grade.Read(vm.Id);
            if (grade == null) return;
            var dlg = new GradeDialog(grade) { Owner = this };
            if (dlg.ShowDialog() == true && dlg.Result != null)
            {
                Grade.Update(dlg.Result);
                RefreshGrades();
            }
        }

        // Deletes the selected grade after confirmation
        private void BtnDeleteGrade_Click(object sender, RoutedEventArgs e)
        {
            if (GridGrades.SelectedItem is not GradeVM vm) return;
            if (MessageBox.Show($"Note \"{vm.Subject} ({vm.ValueDisplay})\" wirklich löschen?",
                    "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Grade.Delete(vm.Id);
                RefreshGrades();
            }
        }
    }
}
