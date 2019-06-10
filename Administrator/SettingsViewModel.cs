using MahalluManager.DataAccess;
using MahalluManager.Infra;
using MahalluManager.Model;
using Microsoft.Practices.Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Administrator {
    public class SettingsViewModel : ViewModelBase {
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

        private string categoryText;
        public string CategoryText {
            get { return categoryText; }
            set {
                categoryText = value;
                AddCategoryCommand.RaiseCanExecuteChanged();
                OnPropertyChanged("CategoryText");
            }
        }
        private bool detailsRequired;

        public bool DetailsRequired {
            get { return detailsRequired; }
            set {
                detailsRequired = value;
                OnPropertyChanged("DetailsRequired");
            }
        }


        private ObservableCollection<Category> categoryList;
        public ObservableCollection<Category> CategoryList {
            get { return categoryList; }
            set {
                categoryList = value;
                OnPropertyChanged("CategoryList");
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Category>>>().Publish(CategoryList);
            }
        }

        public SettingsViewModel() {
            AddAreaCommand = new DelegateCommand(ExecuteAddArea, CanExecuteAddArea);
            DeleteCommand = new DelegateCommand<Area>(ExecuteDelete);
            AddCategoryCommand = new DelegateCommand(ExecuteAddCategory, CanExecuteAddCategory);
            DeleteCategoryCommand = new DelegateCommand<Category>(ExecuteCategoryDelete);

            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                CategoryList = new ObservableCollection<Category>(unitofWork.Categories.GetAll());
                AreaList = new ObservableCollection<Area>(unitofWork.Areas.GetAll());
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

        private DelegateCommand addCategoryCommand;
        public DelegateCommand AddCategoryCommand {
            get { return addCategoryCommand; }
            set { addCategoryCommand = value; }
        }

        private void ExecuteAddCategory() {
            using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                var category = new Category() { Name = CategoryText, DetailsRequired = DetailsRequired };
                unitofWork.Categories.Add(category);
                CategoryList.Add(category);
                eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Category>>>().Publish(CategoryList);
                CategoryText = String.Empty;
                DetailsRequired = default(bool);
                unitofWork.Complete();
            }
        }
        private bool CanExecuteAddCategory() {
            return CategoryText != null && CategoryText != String.Empty;
        }

        private DelegateCommand<Category> deleteCategoryCommand;
        public DelegateCommand<Category> DeleteCategoryCommand {
            get { return deleteCategoryCommand; }
            set { deleteCategoryCommand = value; }
        }

        private void ExecuteCategoryDelete(Category category) {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure to delete", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(messageBoxResult == MessageBoxResult.Yes) {
                using(var unitofWork = new UnitOfWork(new MahalluDBContext())) {
                    CategoryList.Remove(category);
                    eventAggregator.GetEvent<PubSubEvent<ObservableCollection<Category>>>().Publish(CategoryList);
                    var result = unitofWork.Categories.Find((x) => x.Id == category.Id).FirstOrDefault();
                    unitofWork.Categories.Remove(result);
                    unitofWork.Complete();
                }
            }
        }

    }
}
