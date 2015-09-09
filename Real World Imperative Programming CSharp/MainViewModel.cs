using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

namespace RealWorldImperativeProgrammingCSharp
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string currentPath = @"C:\";
        private List<FileEntry> files = new List<FileEntry>();
        private FileEntry selectedFile = null;
        private IFileFilter selectedFileFilter;
        private string filterString;
        private IFileAction selectedFileAction;

        public MainViewModel()
        {
            FileFilters = new List<IFileFilter> { new AcceptAllFilter(), new ExtensionFilter(), new SubstringFilter() };
            SelectedFileFilter = FileFilters[0];
            FileActions = new List<IFileAction> { new OpenFileAction(), new DeleteFileAction() };
            SelectedFileAction = FileActions[0];
            ExecuteCommand = new DelegateCommand(Execute);
            ChooseDirectoryCommand = new DelegateCommand(ChooseDirectory);

            Files = LoadFiles(currentPath);
            SelectedFile = Files.Any() ? Files[0] : null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<IFileFilter> FileFilters { get; private set; }

        public List<IFileAction> FileActions { get; private set; }

        public ICommand ExecuteCommand { get; private set; }

        public ICommand ChooseDirectoryCommand { get; private set; }

        public string CurrentPath
        {
            get
            {
                return currentPath;
            }

            set
            {
                currentPath = value;
                OnPropertyChanged("CurrentPath");
                Files = LoadFiles(currentPath);
            }
        }

        public List<FileEntry> Files
        {
            get
            {
                return files;
            }

            set
            {
                files = value;
                OnPropertyChanged("Files");
                OnPropertyChanged("FilteredFiles");
            }
        }

        public List<FileEntry> FilteredFiles
        {
            get
            {
                return Files.Where(f => selectedFileFilter.Apply(filterString, f.Path)).ToList();
            }
        }

        public FileEntry SelectedFile
        {
            get
            {
                return selectedFile;
            }

            set
            {
                selectedFile = value;
                OnPropertyChanged("SelectedFile");
            }
        }

        public IFileFilter SelectedFileFilter
        {
            get
            {
                return selectedFileFilter;
            }

            set
            {
                selectedFileFilter = value;
                OnPropertyChanged("SelectedFileFilter");
                OnPropertyChanged("FilteredFiles");
            }
        }

        public string FilterString
        {
            get
            {
                return filterString;
            }

            set
            {
                filterString = value;
                OnPropertyChanged("FilterString");
                OnPropertyChanged("FilteredFiles");
            }
        }

        public IFileAction SelectedFileAction
        {
            get
            {
                return selectedFileAction;
            }

            set
            {
                selectedFileAction = value;
                OnPropertyChanged("SelectedFileAction");
            }
        }

        private void Execute()
        {
            selectedFileAction.Execute(selectedFile.Path);
            Files = LoadFiles(currentPath);
        }

        private void ChooseDirectory()
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CurrentPath = dialog.SelectedPath;
            }
        }

        private List<FileEntry> LoadFiles(string path)
        {
            var files = new List<FileEntry>();
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    files.Add(new FileEntry(file));   
                }
            }
            return files;
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
