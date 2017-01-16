using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using terradbtag.Framework;
using terradbtag.Models;
using terradbtag.Services;
using WordCloudCalculator.Contract.Word;

namespace terradbtag.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _searchRequest;
        private ObservableCollection<BusinessObject> _businessObjectList = new ObservableCollection<BusinessObject>();

        private IList<IWeightedWord> _tags = new List<IWeightedWord>();
        private bool _isReady;
        private int _currentProgressValue;
        private int _maximumProgressValue = 1;
        private bool _isInProgress;
        public event PropertyChangedEventHandler PropertyChanged;

        private Repository Repository { get; set; }

        public bool IsReady
        {
            get { return _isReady; }
            private set
            {
                _isReady = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadDatabaseCommand { get; set; }
        public ICommand GenerateDataCommand { get; set; }
        public ICommand ConvertTerraDbCommand { get; set; }
        public ICommand NewBusinessObjectCommand { get; set; }
        public ICommand DeleteBusinessObjectCommand { get; set; }
        public ICommand EditBusinessObjectCommand { get; set; }
        public ICommand ClearDatabaseCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand ClearSearchCommand { get; set; }

        public ICommand SelectTag { get; set; }
        public ICommand UnSelectTag { get; set; }

        private SqliteDatabaseConnection Connection { get; } = new SqliteDatabaseConnection();

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel()
        {
            PropertyChanged += ExecutePropertyChanged;
            LoadDatabaseCommand = new RelayCommand(ExecuteLoadDatabaseCommand);
            NewBusinessObjectCommand = new RelayCommand(ExecuteNewBusinessObject);
            DeleteBusinessObjectCommand = new RelayCommand(ExecuteDeleteBusinessObjectCommand);
            EditBusinessObjectCommand = new RelayCommand(ExecuteEditBusinessObjectCommand);
            GenerateDataCommand = new RelayCommand(ExecuteGenerateDataCommand);
            ConvertTerraDbCommand = new RelayCommand(ExecuteConvertTerraDbCommand);
            ClearDatabaseCommand = new RelayCommand(ExecuteClearDatabaseCommand);
            SearchCommand = new RelayCommand(ExecuteSearchCommand);
            ClearSearchCommand = new RelayCommand(ExecuteClearSearchCommand);
            SelectTag = new RelayCommand(ExecuteSelectTagCommand);
            UnSelectTag = new RelayCommand(ExecuteUnselectTag);
            IsReady = false;
        }

        private void ExecuteClearSearchCommand(object o)
        {
            SearchRequest = null;
            UpdateSearchResult();
        }

        private void ExecuteUnselectTag(object o)
        {
            var tag = o as Tag;

            if (tag == null) return;

            SelectedTags.Remove(tag);

            UpdateSearchResult();
        }

        private void ExecuteSelectTagCommand(object o)
        {
            var tag = o as IWord;

            if (tag == null) return;

            SelectedTags.Add(new Tag {Text = tag.Text});

            UpdateSearchResult();
        }

        private void ExecuteSearchCommand(object o)
        {
            UpdateSearchResult();
        }

        private void UpdateSearchResult()
        {
            Logger.Clear();
            var query = new SearchQuery
            {
                SelectedTags = SelectedTags,
                TextQuery = SearchRequest,
                TagLimit = 50
            };

            LoadTags(query);
            LoadBusinessObjects(query);
        }

        private void ExecuteConvertTerraDbCommand(object o)
        {
            try
            {
                var importer = new TerraConvertionService();
                importer.ProgressChanged += (sender, tuple) =>
                {
                    CurrentProgressValue = tuple.Item1;
                    MaximumProgressValue = tuple.Item2;
                    IsInProgress = true;
                };

                importer.Finished += (sender, isSucceed) =>
                {
                    if (isSucceed)
                    {
                        MessageBox.Show($"Terradb konvertiert", "Erfolg", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(importer.Error, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    CurrentProgressValue = 0;
                    MaximumProgressValue = 1;
                    IsInProgress = false;
                };

                importer.Execute();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteClearDatabaseCommand(object o)
        {
            new DatabaseService {Connection = Connection}.ResetDatabase();
            LoadAllDataFromDatabase();
            MessageBox.Show($"Datenbank geleert!", "Erfolg", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void ExecuteGenerateDataCommand(object o)
        {
            var srv = new DataGenerationService {Repository = Repository};
            srv.ProgressChanged += (sender, tuple) => { SetProgress(tuple); };
            srv.Finished += (sender, isSucceed) =>
            {
                if (isSucceed)
                {
                    MessageBox.Show("Daten importiert", "Erfolg", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    LoadAllDataFromDatabase();
                }
                else
                {
                    MessageBox.Show(srv.Error, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                ResetProgress();
            };
            srv.Execute();
        }

        private void ExecuteEditBusinessObjectCommand(object o)
        {
            var obj = o as BusinessObject;
            if (obj == null) return;
            EditBusinessObject(obj);
        }

        private void ExecuteDeleteBusinessObjectCommand(object o)
        {
            var obj = o as BusinessObject;
            if (obj == null) return;
            try
            {
                Repository.Delete(obj.Id);
                BusinessObjectList.Remove(obj);
                MessageBox.Show($"Objekt {obj.Id} gelöscht", "Erfolg", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteNewBusinessObject(object o)
        {
            var obj = new BusinessObject();
            if (EditBusinessObject(obj))
            {
                BusinessObjectList.Add(obj);
            }
        }

        private void ExecuteLoadDatabaseCommand(object o)
        {
            var dbFileOpener = new OpenFileService()
            {
                AcceptNonExistingFiles = true
            };

            if (!dbFileOpener.OpenFile(OpenFileService.SqliteDatabaseFilter)) return;

            Connection.Connect(dbFileOpener.SelectedFile);

            if (!dbFileOpener.SelectedFileExists)
            {
                new DatabaseService {Connection = Connection}.InitializeDatabase();
            }

            Repository = new Repository() {Connection = Connection};

            BusinessObjectList.Clear();

            LoadAllDataFromDatabase();

            LoadTags(SearchQuery.NoFilter);

            IsReady = true;
        }

        public int CurrentProgressValue
        {
            get { return _currentProgressValue; }
            set
            {
                _currentProgressValue = value;
                OnPropertyChanged();
            }
        }

        public int MaximumProgressValue
        {
            get { return _maximumProgressValue; }
            set
            {
                _maximumProgressValue = value;
                OnPropertyChanged();
            }
        }

        public bool IsInProgress
        {
            get { return _isInProgress; }
            set
            {
                _isInProgress = value;
                OnPropertyChanged();
            }
        }

        private void SetProgress(Tuple<int, int, object> data)
        {
            SetProgress(data.Item1, data.Item2);
        }

        private void SetProgress(int current, int max)
        {
            CurrentProgressValue = current;
            MaximumProgressValue = max;
            IsInProgress = true;
        }

        private void ResetProgress()
        {
            SetProgress(0, 1);
            IsInProgress = false;
        }

        private void AlertError(string message)
        {
            MessageBox.Show(message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private Dictionary<string, BitmapImage> BusinessObjectImageCache = new Dictionary<string, BitmapImage>();

        private void LoadBusinessObjects(ISearchQuery query)
        {
            try
            {
                var srv = new DataLoadingService {Repository = Repository};

                srv.ProgressChanged += (sender, tuple) =>
                {
                    SetProgress(tuple);
                    var businessObject = tuple.Item3 as BusinessObject;
                    if (businessObject == null) return;

                    if (BusinessObjectImageCache.ContainsKey(businessObject.Id))
                    {
                        businessObject.Image = BusinessObjectImageCache[businessObject.Id];
                    }
                    else
                    {
                        try
                        {
                            businessObject.Image =
                                LoadImage(
                                    Convert.FromBase64CharArray(businessObject.Data.ToCharArray(), 0,
                                        businessObject.Data.Length)
                                );
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError("Es ist ein Fehler Aufgetreten!: " + ex.Message);
                            businessObject.Image = new BitmapImage();
                        }

                        BusinessObjectImageCache.Add(businessObject.Id, businessObject.Image);
                    }

                    BusinessObjectList.Add(businessObject);
                };
                srv.Finished += (sender, isSucceed) =>
                {
                    if (!isSucceed)
                    {
                        AlertError(srv.Error);
                    }
                    ResetProgress();
                };

                BusinessObjectList.Clear();
                srv.Execute(query);
            }
            catch (Exception ex)
            {
                AlertError(ex.Message);
            }
        }

        private void LoadAllDataFromDatabase()
        {
            LoadBusinessObjects(SearchQuery.NoFilter);
        }

        private void ExecutePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(SearchRequest))
            {
            }
        }

        private bool EditBusinessObject(BusinessObject obj)
        {
            var id = obj.Id;
            var viewModel = new BusinessObjectEditorViewModel()
            {
                BusinessObject = obj
            };
            var window = new Views.BusinessObjectEditor()
            {
                DataContext = viewModel
            };
            if (window.ShowDialog() == true)
            {
                try
                {
                    if (!Repository.Exists(obj.Id))
                    {
                        Repository.Create(obj);
                    }
                    else
                    {
                        Repository.Update(obj);
                    }

                    MessageBox.Show($"Objekt {obj.Id} gespeichert", "Erfolg", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(id)) return false;
                var objOriginal = Repository.Find(id);
                obj.Id = objOriginal.Id;
                obj.Name = obj.Name;
                obj.Data = objOriginal.Data;

                obj.Tags.Clear();
                obj.Tags.AddRange(objOriginal.Tags);

                //MessageBox.Show($"Bearbeitung abgebrochen", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return false;
        }

        public string SearchRequest
        {
            get { return _searchRequest; }
            set
            {
                _searchRequest = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BusinessObject> BusinessObjectList
        {
            get { return _businessObjectList; }
            set
            {
                _businessObjectList = value;
                OnPropertyChanged();
            }
        }

        public IList<IWeightedWord> Tags
        {
            get { return _tags; }
            set
            {
                _tags = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Tag> SelectedTags { get; set; } = new ObservableCollection<Tag>();

        private void LoadTags(ISearchQuery query)
        {
            var tags = new List<IWeightedWord>();
            var srv = new TagLoadingService {Connection = Connection};
            srv.ProgressChanged += (sender, tuple) =>
            {
                if (tuple.Item3 != null)
                    tags.Add(tuple.Item3 as Tag);
            };
            srv.Finished += (sender, isSucceed) =>
            {
                if (!isSucceed) AlertError(srv.Error);
                else Tags = tags;
            };
            srv.Execute(query);
        }

        // Stackoverflow saves the day. #WegGefunden
        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}