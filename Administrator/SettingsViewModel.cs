using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Model;
using MahalluManager.Model.EventTypes;
using Microsoft.Practices.Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
//using System.Windows.Forms;

namespace Administrator {
    public class SettingsViewModel : ViewModelBase {
        public SettingsViewModel() {
            CommonDetails = new CommonDetailsType();
            AddAreaCommand = new DelegateCommand(ExecuteAddArea, CanExecuteAddArea);
            DeleteCommand = new DelegateCommand<Area>(ExecuteDelete);

            TransactionCommand = new DelegateCommand(ExecuteTransaction, CanExecuteTransaction);
            DeleteTransactionCommand = new DelegateCommand<CashSource>(DeleteTransaction);

            SelectLocationCommand = new DelegateCommand(ExecuteSelectLocation);
            BackupCommand = new DelegateCommand(ExecuteBackup, CanExecuteBackupOrRestore);
            RestoreCommand = new DelegateCommand(ExecuteRestore, CanExecuteBackupOrRestore);

            AddContributionCategoryCommand = new DelegateCommand(ExecuteAddContributionCategory, CanExecuteAddContributionCategory);
            DeleteContributionCategoryCommand = new DelegateCommand<IncomeCategory>(ExecuteContributionCategoryDelete);

            AddExpenseCategoryCommand = new DelegateCommand(ExecuteAddExpenseCategory, CanExecuteAddExpenseCategory);
            DeleteExpenseCategoryCommand = new DelegateCommand<ExpenseCategory>(ExecuteExpenseCategoryDelete);

            SaveCommonDetailCommand = new DelegateCommand(ExecuteSaveCommonDetail);
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                AreaList = new ObservableCollection<Area>(unitofWork.Areas.GetAll());
                ContributionCategoryList = new ObservableCollection<IncomeCategory>(unitofWork.IncomeCategories.GetAll());
                ExpenseCategoryList = new ObservableCollection<ExpenseCategory>(unitofWork.ExpenseCategories.GetAll());
                CashSourceList = new ObservableCollection<CashSource>(unitofWork.CashSources.GetAll());
            }
            if(CashSourceList != null && CashSourceList.Count > 0) {
                CurrentCashSource = CashSourceList[0];
            }
            MasjidName = ConfigurationManager.AppSettings.Get("MasjidName");
            RegistrationNumber = ConfigurationManager.AppSettings.Get("RegistrationNumber");

            eventAggregator.GetEvent<PubSubEvent<SystemTotalType>>().Subscribe((e) => {
                SystemTotalType systemTotalType = ((SystemTotalType)e);
                if(Convert.ToInt32(systemTotalType.SelectedYear) == DateTime.Now.Year) {
                    SystemTotal = systemTotalType.Balance;
                } else {
                    DeltaAmount = "Select Current Year";
                }
            });
            calculateSourceTotal();
        }



        private void calculateSourceTotal() {
            decimal total = 0;
            foreach(var item in CashSourceList) {
                total = total + item.Amount;
            }
            SourceTotal = total;
        }

        private Decimal systemTotal;
        public Decimal SystemTotal {
            get { return systemTotal; }
            set {
                systemTotal = value;
                DeltaAmount = (SystemTotal - SourceTotal).ToString();
                OnPropertyChanged("SystemTotal");
            }
        }

        private Decimal sourceTotal;
        public Decimal SourceTotal {
            get { return sourceTotal; }
            set {
                sourceTotal = value;
                DeltaAmount = (SystemTotal - SourceTotal).ToString();
                OnPropertyChanged("SourceTotal");
            }
        }

        private string deltaAmount;
        public string DeltaAmount {
            get { return deltaAmount; }
            set {
                deltaAmount = value;
                OnPropertyChanged("DeltaAmount");
            }
        }

        public CommonDetailsType CommonDetails { get; private set; }

        private String masjidName;
        public String MasjidName {
            get { return masjidName; }
            set {
                masjidName = value;
                OnPropertyChanged("MasjidName");
                CommonDetails.MasjidName = MasjidName;
                eventAggregator.GetEvent<PubSubEvent<CommonDetailsType>>().Publish(CommonDetails);
            }
        }

        private String registrationNumber;
        public String RegistrationNumber {
            get { return registrationNumber; }
            set {
                registrationNumber = value;
                OnPropertyChanged("RegistrationNumber");
                CommonDetails.RegistrationNumber = RegistrationNumber;
                eventAggregator.GetEvent<PubSubEvent<CommonDetailsType>>().Publish(CommonDetails);
            }
        }
        private string areaText;
        public string AreaText {
            get { return areaText; }
            set {
                areaText = value;
                AddAreaCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("AreaText");
            }
        }

        private ObservableCollection<Area> areaList;
        public ObservableCollection<Area> AreaList {
            get { return areaList; }
            set {
                areaList = value;
                OnPropertyChanged("AreaList");
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Area>>>().Publish(AreaList);
            }
        }

        private string contributionCategoryText;
        public string ContributionCategoryText {
            get { return contributionCategoryText; }
            set {
                contributionCategoryText = value;
                AddContributionCategoryCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("ContributionCategoryText");
            }
        }
        private bool contributionDetailsRequired;
        public bool ContributionDetailsRequired {
            get { return contributionDetailsRequired; }
            set {
                contributionDetailsRequired = value;
                OnPropertyChanged("ContributionDetailsRequired");
            }
        }

        private ObservableCollection<IncomeCategory> contributionCategoryList;
        public ObservableCollection<IncomeCategory> ContributionCategoryList {
            get { return contributionCategoryList; }
            set {
                contributionCategoryList = value;
                OnPropertyChanged("ContributionCategoryList");
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<IncomeCategory>>>().Publish(ContributionCategoryList);
            }
        }

        private string sourceNameText;
        public string SourceNameText {
            get { return sourceNameText; }
            set {
                sourceNameText = value;
                TransactionCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("SourceNameText");
            }
        }

        private string sourceAmount;
        public string SourceAmount {
            get { return sourceAmount; }
            set {
                sourceAmount = value;
                TransactionCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("SourceAmount");
            }
        }

        private ObservableCollection<CashSource> cashSourceList;
        public ObservableCollection<CashSource> CashSourceList {
            get { return cashSourceList; }
            set {
                cashSourceList = value;
                OnPropertyChanged("CashSourceList");
            }
        }

        private DelegateCommand<CashSource> deleteTransactionCommand;
        public DelegateCommand<CashSource> DeleteTransactionCommand {
            get { return deleteTransactionCommand; }
            set { deleteTransactionCommand = value; }
        }

        private DelegateCommand transactionCommand;
        public DelegateCommand TransactionCommand {
            get { return transactionCommand; }
            set { transactionCommand = value; }
        }

        private DelegateCommand selectLocationCommand;
        public DelegateCommand SelectLocationCommand {
            get { return selectLocationCommand; }
            set { selectLocationCommand = value; }
        }
        private DelegateCommand backupCommand;
        public DelegateCommand BackupCommand {
            get { return backupCommand; }
            set { backupCommand = value; }
        }
        private DelegateCommand restoreCommand;
        public DelegateCommand RestoreCommand {
            get { return restoreCommand; }
            set { restoreCommand = value; }
        }
        private Boolean isDeposite;
        public Boolean IsDeposite {
            get { return isDeposite; }
            set {
                isDeposite = value;
                if(isDeposite) {
                    SourceAmount = String.Empty;
                    IsEnableAmount = true;
                }
                OnPropertyChanged("IsDeposite");
            }
        }

        private Boolean isWithdrawal;

        public Boolean IsWithdrawal {
            get { return isWithdrawal; }
            set {
                isWithdrawal = value;
                if(isWithdrawal) {
                    SourceAmount = String.Empty;
                    IsEnableAmount = true;
                }
                OnPropertyChanged("IsWithdrawal");
            }
        }

        private Boolean isEnaleAmount;

        public Boolean IsEnableAmount {
            get { return isEnaleAmount; }
            set {
                isEnaleAmount = value;
                OnPropertyChanged("IsEnableAmount");
            }
        }

        private CashSource currentCashSource;

        public CashSource CurrentCashSource {
            get { return currentCashSource; }
            set {
                currentCashSource = value;
                CurrentCashSourceChange();
                OnPropertyChanged("CurrentCashSource");
            }
        }

        private void CurrentCashSourceChange() {
            if(CurrentCashSource != null) {
                SourceNameText = CurrentCashSource.SourceName;
                SourceAmount = CurrentCashSource.Amount.ToString();
                IsEnableAmount = IsDeposite = IsWithdrawal = false;
            } else {
                ClearCurrentCashSource();
            }
        }

        private void ClearCurrentCashSource() {
            SourceAmount = SourceNameText = String.Empty;
        }

        private void ExecuteTransaction() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                Decimal amount;
                if(!Decimal.TryParse(SourceAmount, out amount)) {
                    MessageBox.Show("Enter a valid amount");
                    return;
                }
                if(CurrentCashSource != null &&
                    !String.IsNullOrEmpty(SourceNameText) &&
                    CurrentCashSource.SourceName.Equals(SourceNameText.Trim())) {
                    if(isWithdrawal) {
                        amount = 0 - amount;
                    }

                    currentCashSource.Amount += amount;
                    unitofWork.CashSources.Update(CurrentCashSource);
                    if(isWithdrawal) {
                        MessageBox.Show("Withrawal of " + SourceAmount + " is successfull from " + CurrentCashSource.SourceName + " ", "Withdrawal", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    if(IsDeposite) {
                        MessageBox.Show("Deposite of " + SourceAmount + " is successfull to " + CurrentCashSource.SourceName + " ", "Deposite", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                } else {
                    if(isWithdrawal) {
                        MessageBox.Show("You can't withdraw from a new source");
                        return;
                    }
                    var cashSource = new CashSource() { SourceName = SourceNameText.Trim(), Amount = amount };
                    unitofWork.CashSources.Add(cashSource);
                    CashSourceList.Add(cashSource);
                    CurrentCashSource = cashSource;
                }
                SourceTotal = SourceTotal + amount;
                SourceAmount = CurrentCashSource.Amount.ToString();

                IsEnableAmount = IsWithdrawal = IsDeposite = false;
                unitofWork.Complete();
            }
        }

        private bool CanExecuteTransaction() {
            return SourceNameText != null && SourceNameText != String.Empty && IsEnableAmount;
        }
        private void DeleteTransaction(CashSource cashSource) {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(messageBoxResult == MessageBoxResult.Yes) {
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    CashSourceList.Remove(cashSource);
                    var result = unitofWork.CashSources.Find((x) => x.Id == cashSource.Id).FirstOrDefault();
                    SourceTotal -= result.Amount;
                    unitofWork.CashSources.Remove(result);
                    unitofWork.Complete();
                }
            }
        }

        private string expenseCategoryText;
        public string ExpenseCategoryText {
            get { return expenseCategoryText; }
            set {
                expenseCategoryText = value;
                AddExpenseCategoryCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("ExpenseCategoryText");
            }
        }

        private bool expenseDetailsRequired;
        public bool ExpenseDetailsRequired {
            get { return expenseDetailsRequired; }
            set {
                expenseDetailsRequired = value;
                OnPropertyChanged("ExpenseDetailsRequired");
            }
        }

        private ObservableCollection<ExpenseCategory> expenseCategoryList;
        public ObservableCollection<ExpenseCategory> ExpenseCategoryList {
            get { return expenseCategoryList; }
            set {
                expenseCategoryList = value;
                OnPropertyChanged("ExpenseCategoryList");
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<ExpenseCategory>>>().Publish(ExpenseCategoryList);
            }
        }

        private DelegateCommand addAreaCommand;
        public DelegateCommand AddAreaCommand {
            get { return addAreaCommand; }
            set { addAreaCommand = value; }
        }

        private void ExecuteAddArea() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                var area = new Area() { Name = AreaText };
                unitofWork.Areas.Add(area);
                AreaList.Add(area);
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Area>>>().Publish(AreaList);
                unitofWork.Complete();
                AreaText = String.Empty;
            }
        }
        private bool CanExecuteAddArea() {
            return AreaText != null && AreaText != String.Empty;
        }

        private DelegateCommand<Area> deleteCommand;
        public DelegateCommand<Area> DeleteCommand {
            get { return deleteCommand; }
            set { deleteCommand = value; }
        }

        private void ExecuteDelete(Area area) {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(messageBoxResult == MessageBoxResult.Yes) {
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    AreaList.Remove(area);
                    eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Area>>>().Publish(AreaList);
                    var result = unitofWork.Areas.Find((x) => x.Id == area.Id).FirstOrDefault();
                    unitofWork.Areas.Remove(result);
                    unitofWork.Complete();
                }
            }
        }

        private DelegateCommand addContributionCategoryCommand;
        public DelegateCommand AddContributionCategoryCommand {
            get { return addContributionCategoryCommand; }
            set { addContributionCategoryCommand = value; }
        }

        private void ExecuteAddContributionCategory() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                var category = new IncomeCategory() { Name = ContributionCategoryText, DetailsRequired = ContributionDetailsRequired };
                unitofWork.IncomeCategories.Add(category);
                ContributionCategoryList.Add(category);
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<IncomeCategory>>>().Publish(ContributionCategoryList);
                ContributionCategoryText = String.Empty;
                ContributionDetailsRequired = default(bool);
                unitofWork.Complete();
            }
        }
        private bool CanExecuteAddContributionCategory() {
            return ContributionCategoryText != null && ContributionCategoryText != String.Empty;
        }

        private DelegateCommand<IncomeCategory> deleteContributionCategoryCommand;
        public DelegateCommand<IncomeCategory> DeleteContributionCategoryCommand {
            get { return deleteContributionCategoryCommand; }
            set { deleteContributionCategoryCommand = value; }
        }

        private void ExecuteContributionCategoryDelete(IncomeCategory category) {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(messageBoxResult == MessageBoxResult.Yes) {
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    ContributionCategoryList.Remove(category);
                    eventAggregator.GetEvent<PubSubEvent<ObservableCollection<IncomeCategory>>>().Publish(ContributionCategoryList);
                    var result = unitofWork.IncomeCategories.Find((x) => x.Id == category.Id).FirstOrDefault();
                    unitofWork.IncomeCategories.Remove(result);
                    unitofWork.Complete();
                }
            }
        }

        private DelegateCommand addExpenseCategoryCommand;
        public DelegateCommand AddExpenseCategoryCommand {
            get { return addExpenseCategoryCommand; }
            set { addExpenseCategoryCommand = value; }
        }

        private void ExecuteAddExpenseCategory() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                var expense = new ExpenseCategory() { Name = ExpenseCategoryText, DetailsRequired = ExpenseDetailsRequired };
                unitofWork.ExpenseCategories.Add(expense);
                ExpenseCategoryList.Add(expense);
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<ExpenseCategory>>>().Publish(ExpenseCategoryList);
                ExpenseCategoryText = String.Empty;
                ExpenseDetailsRequired = default(bool);
                unitofWork.Complete();
            }
        }

        private bool CanExecuteAddExpenseCategory() {
            return ExpenseCategoryText != null && ExpenseCategoryText != String.Empty;
        }

        private DelegateCommand<ExpenseCategory> deleteExpenseCategoryCommand;
        public DelegateCommand<ExpenseCategory> DeleteExpenseCategoryCommand {
            get { return deleteExpenseCategoryCommand; }
            set { deleteExpenseCategoryCommand = value; }
        }

        private void ExecuteExpenseCategoryDelete(ExpenseCategory expenseCategory) {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(messageBoxResult == MessageBoxResult.Yes) {
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    ExpenseCategoryList.Remove(expenseCategory);
                    eventAggregator.GetEvent<PubSubEvent<ObservableCollection<ExpenseCategory>>>().Publish(ExpenseCategoryList);
                    var result = unitofWork.ExpenseCategories.Find((x) => x.Id == expenseCategory.Id).FirstOrDefault();
                    unitofWork.ExpenseCategories.Remove(result);
                    unitofWork.Complete();
                }
            }
        }

        private DelegateCommand saveCommonDetailCommand;

        public DelegateCommand SaveCommonDetailCommand {
            get { return saveCommonDetailCommand; }
            set { saveCommonDetailCommand = value; }
        }
        private void ExecuteSaveCommonDetail() {
            Configuration config = ConfigurationManager.OpenExeConfiguration(System.Windows.Forms.Application.ExecutablePath);
            config.AppSettings.Settings.Remove("MasjidName");
            config.AppSettings.Settings.Add("MasjidName", MasjidName);
            config.AppSettings.Settings.Remove("RegistrationNumber");
            config.AppSettings.Settings.Add("RegistrationNumber", RegistrationNumber);
            config.Save(ConfigurationSaveMode.Minimal);
        }

        private string backupAndRestoreLocation;

        public string BackupAndRestoreLocation {
            get { return backupAndRestoreLocation; }
            set {
                backupAndRestoreLocation = value;
                BackupCommand.RaiseCanExecuteChanged();
                RestoreCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("BackupAndRestoreLocation");
            }
        }

        private void ExecuteSelectLocation() {
            using(var fbd = new System.Windows.Forms.FolderBrowserDialog()) {
                System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                if(result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath)) {
                    BackupAndRestoreLocation = fbd.SelectedPath;
                }
            }
        }
        private void ExecuteRestore() {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to Restore", "Restore", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(messageBoxResult == MessageBoxResult.Yes) {
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    unitofWork.Residences.RemoveRange(unitofWork.Residences.GetAll());
                    unitofWork.ResidenceMembers.RemoveRange(unitofWork.ResidenceMembers.GetAll());
                    unitofWork.Areas.RemoveRange(unitofWork.Areas.GetAll());
                    unitofWork.ExpenseCategories.RemoveRange(unitofWork.ExpenseCategories.GetAll());
                    unitofWork.IncomeCategories.RemoveRange(unitofWork.IncomeCategories.GetAll());
                    unitofWork.CashSources.RemoveRange(unitofWork.CashSources.GetAll());
                    //unitofWork.Expenses.RemoveRange(unitofWork.Expenses.GetAll());
                    //unitofWork.ExpenseDetails.RemoveRange(unitofWork.ExpenseDetails.GetAll());
                    //unitofWork.Contributions.RemoveRange(unitofWork.Contributions.GetAll());
                    //unitofWork.ContributionDetails.RemoveRange(unitofWork.ContributionDetails.GetAll());
                    //unitofWork.MarriageCertificates.RemoveRange(unitofWork.MarriageCertificates.GetAll());
                    unitofWork.Complete();

                    string[] directories = Directory.GetDirectories(BackupAndRestoreLocation);
                    string directoryPath = backupAndRestoreLocation + @"\MMBackup";
                    if(Directory.Exists(directoryPath)) {
                        string filePath = directoryPath + @"\Residences.csv";
                        if(File.Exists(filePath)) {
                            String[] backupData = File.ReadAllLines(filePath);
                            for(int i = 1; i < backupData.Length; i++) {
                                string[] fields = backupData[i].Split(new char[] { ',' });
                                Residence residence = new Residence();
                                residence.Number = fields[0];
                                residence.Name = fields[1];
                                residence.Area = fields[2];
                                unitofWork.Residences.Add(residence);
                            }
                        }
                        unitofWork.Complete();

                        filePath = directoryPath + @"\ResidenceMembers.csv";
                        if(File.Exists(filePath)) {
                            String[] backupData = File.ReadAllLines(filePath);
                            for(int i = 1; i < backupData.Length; i++) {
                                string[] fields = backupData[i].Split(new char[] { ',' });
                                Residence residence = unitofWork.Residences.GetAll().Where(x => x.Number == fields[0]).FirstOrDefault();
                                ResidenceMember residenceMember = new ResidenceMember();
                                residenceMember.Residence_Id = residence.Id;
                                residenceMember.MemberName = fields[1];
                                residenceMember.DOB = Convert.ToDateTime(fields[2]);
                                residenceMember.Job = fields[3];
                                residenceMember.Mobile = fields[4];
                                residenceMember.Abroad = Convert.ToBoolean(fields[5]);
                                residenceMember.Country = fields[6];
                                residenceMember.IsGuardian = Convert.ToBoolean(fields[7]);
                                residenceMember.Gender = fields[8];
                                residenceMember.MarriageStatus = fields[9];
                                residenceMember.Qualification = fields[10];
                                residenceMember.Remarks = fields[11];
                                unitofWork.ResidenceMembers.Add(residenceMember);
                            }
                        }
                        unitofWork.Complete();

                        filePath = directoryPath + @"\Areas.csv";
                        if(File.Exists(filePath)) {
                            String[] backupData = File.ReadAllLines(filePath);
                            for(int i = 1; i < backupData.Length; i++) {
                                string[] fields = backupData[i].Split(new char[] { ',' });
                                Area area = new Area();
                                area.Name = fields[0];
                                unitofWork.Areas.Add(area);
                            }
                        }
                        unitofWork.Complete();

                        filePath = directoryPath + @"\ContributionDetail.csv";
                        if(File.Exists(filePath)) {
                            String[] backupData = File.ReadAllLines(filePath);
                            for(int i = 1; i < backupData.Length; i++) {
                                string[] fields = backupData[i].Split(new char[] { ',' });
                                ContributionDetail contributionDetail = new ContributionDetail();

                                unitofWork.ContributionDetails.Add(contributionDetail);
                            }
                        }
                        unitofWork.Complete();

                        filePath = directoryPath + @"\CashSources.csv";
                        if(File.Exists(filePath)) {
                            String[] backupData = File.ReadAllLines(filePath);
                            for(int i = 1; i < backupData.Length; i++) {
                                string[] fields = backupData[i].Split(new char[] { ',' });
                                CashSource cashSource = new CashSource();
                                cashSource.SourceName = fields[0];
                                cashSource.Amount = Convert.ToDecimal(fields[1]);
                                unitofWork.CashSources.Add(cashSource);
                            }
                        }
                        unitofWork.Complete();

                        filePath = directoryPath + @"\ExpenseCategories.csv";
                        if(File.Exists(filePath)) {
                            String[] backupData = File.ReadAllLines(filePath);
                            for(int i = 1; i < backupData.Length; i++) {
                                string[] fields = backupData[i].Split(new char[] { ',' });
                                ExpenseCategory expenseCategory = new ExpenseCategory();
                                expenseCategory.Name = fields[0];
                                expenseCategory.DetailsRequired = Convert.ToBoolean(fields[1]);
                                unitofWork.ExpenseCategories.Add(expenseCategory);
                            }
                        }
                        unitofWork.Complete();

                        filePath = directoryPath + @"\IncomeCategories.csv";
                        if(File.Exists(filePath)) {
                            String[] backupData = File.ReadAllLines(filePath);
                            for(int i = 1; i < backupData.Length; i++) {
                                string[] fields = backupData[i].Split(new char[] { ',' });
                                IncomeCategory incomeCategory = new IncomeCategory();
                                incomeCategory.Name = fields[0];
                                incomeCategory.DetailsRequired = Convert.ToBoolean(fields[1]);
                                unitofWork.IncomeCategories.Add(incomeCategory);
                            }
                        }
                        unitofWork.Complete();

                        MessageBox.Show("Restore is completed successfully..!");
                    } else {
                        MessageBox.Show(backupAndRestoreLocation + " doesn't contain back up directory as MMBackup");
                    }
                }
            }
        }

        private bool CanExecuteBackupOrRestore() {
            return !String.IsNullOrEmpty(BackupAndRestoreLocation);
        }

        private void ExecuteBackup() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to Backup", "Backup", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if(messageBoxResult == MessageBoxResult.Yes) {

                    List<String> backupData = new List<string>();
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(BackupAndRestoreLocation + @"\MMBackup");
                    backupData.Add("House Number,Name,Area");
                    foreach(var item in unitofWork.Residences.GetAll()) {
                        backupData.Add(item.Number + "," + item.Name + "," + item.Area);
                    }
                    File.WriteAllLines(directoryInfo.FullName + @"\Residences.csv", backupData);
                    backupData.Clear();

                    backupData.Add("House Number,Member Name,DOB,Job,Mobile,Is Abroad,Country,Is Guardian,Gender,Marriage Status,Qualification,Remarks");
                    foreach(var item in unitofWork.ResidenceMembers.GetAll()) {
                        //Add house number in backup of members as id
                        Residence residence = unitofWork.Residences.Get(item.Residence_Id);
                        backupData.Add(residence.Number + "," + item.MemberName + "," + item.DOB + "," + item.Job + "," +
                            item.Mobile + "," + item.Abroad + "," + item.Country + "," + item.IsGuardian + "," +
                            item.Gender + "," + item.MarriageStatus + "," + item.Qualification + "," + item.Remarks);
                    }
                    File.WriteAllLines(directoryInfo.FullName + @"\ResidenceMembers.csv", backupData);
                    backupData.Clear();

                    backupData.Add("Name,Is Details Required");
                    foreach(var item in unitofWork.ExpenseCategories.GetAll()) {
                        backupData.Add(item.Name + "," + item.DetailsRequired);
                    }
                    File.WriteAllLines(directoryInfo.FullName + @"\ExpenseCategories.csv", backupData);
                    backupData.Clear();

                    backupData.Add("Name,Is Details Required");
                    foreach(var item in unitofWork.IncomeCategories.GetAll()) {
                        backupData.Add(item.Name + "," + item.DetailsRequired);
                    }
                    File.WriteAllLines(directoryInfo.FullName + @"\IncomeCategories.csv", backupData);
                    backupData.Clear();

                    backupData.Add("Name");
                    foreach(var item in unitofWork.Areas.GetAll()) {
                        backupData.Add(item.Name);
                    }
                    File.WriteAllLines(directoryInfo.FullName + @"\Areas.csv", backupData);
                    backupData.Clear();

                    backupData.Add("Source Name,Amount");
                    foreach(var item in unitofWork.CashSources.GetAll()) {
                        backupData.Add(item.SourceName + "," + item.Amount);
                    }
                    File.WriteAllLines(directoryInfo.FullName + @"\CashSources.csv", backupData);
                    backupData.Clear();

                    backupData.Add("Category Name,Toatal Amount,Created On,ReceiptNo");
                    foreach(var item in unitofWork.Contributions.GetAll()) {
                        backupData.Add(item.CategoryName + "," + item.ToatalAmount + "," + item.CreatedOn + "," + item.ReceiptNo);
                    }
                    File.WriteAllLines(directoryInfo.FullName + @"\Contributions.csv", backupData);
                    backupData.Clear();

                    backupData.Add("House  Number,House Name,Member Name,Amount,Created On,Receipt No,Care Of");
                    foreach(var item in unitofWork.ContributionDetails.GetAll()) {
                        backupData.Add(item.HouserNumber + "," + item.HouserName + "," + item.MemberName + "," + item.Amount
                            + "," + item.CreatedOn + "," + item.ReceiptNo + "," + item.CareOf);
                    }
                    File.WriteAllLines(directoryInfo.FullName + @"\ContributionDetail.csv", backupData);
                    backupData.Clear();


                    backupData.Add("Category Name,Created On,Toatal Amount,BillNo");
                    foreach(var item in unitofWork.Expenses.GetAll()) {
                        backupData.Add(item.CategoryName + "," + item.CreatedOn + "," + item.ToatalAmount + "," + item.BillNo);
                    }
                    File.WriteAllLines(directoryInfo.FullName + @"\Expenses.csv", backupData);
                    backupData.Clear();

                    backupData.Add("Name,Amount,Created On,Bill No,Care Of");
                    foreach(var item in unitofWork.ExpenseDetails.GetAll()) {
                        backupData.Add(item.Name + "," + item.Amount + "," + item.CreatedOn + "," + item.BillNo + "," + item.CareOf);
                    }
                    File.WriteAllLines(directoryInfo.FullName + @"\ExpenseDetails.csv", backupData);
                    backupData.Clear();

                    backupData.Add("Bride Name,Bride Photo String,Bride DOB,Bride Father Name,Bride House Name," +
                            "Bride Area,Bride Pincode,Bride Post Office,Bride District,Bride State," +
                            "Bride Country,Groom Name,Groom Photo String,Groom DOB,Groom Father Name,Groom House Name," +
                            "Groom Area,GroomPincode,Groom Post Office,Groom District,Groom State" +
                            "GroomCountry,MarriageDate,MarriagePlace"
                            );
                    foreach(var item in unitofWork.MarriageCertificates.GetAll()) {
                        string bridePhotoString = item.BridePhoto != null ? BitConverter.ToString(item.BridePhoto) : "NULL";
                        string groomPhotoString = item.GroomPhoto != null ? BitConverter.ToString(item.GroomPhoto) : "NULL";
                        backupData.Add(item.BrideName + "," + bridePhotoString + "," + item.BrideDOB + "," + item.BrideFatherName + "," + item.BrideHouseName +
                            "," + item.BrideArea + "," + item.BridePincode + "," + item.BridePostOffice + "," + item.BrideDistrict + "," + item.BrideState +
                            "," + item.BrideCountry + "," + item.GroomName + "," + groomPhotoString + "," + item.GroomDOB + "," + item.GroomFatherName + "," + item.GroomHouseName +
                            "," + item.GroomArea + "," + item.GroomPincode + "," + item.GroomPostOffice + "," + item.GroomDistrict + "," + item.GroomState +
                            "," + item.GroomCountry + "," + item.MarriageDate + "," + item.MarriagePlace
                            );
                    }
                    File.WriteAllLines(directoryInfo.FullName + @"\MarriageCertificates.csv", backupData);
                    backupData.Clear();

                    MessageBox.Show("Backup is completed successfully..!");
                }
            }
        }
    }
}
