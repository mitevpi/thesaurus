using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace thesaurus
{
    public class TrainViewModel : ViewModelBase
    {
        private string _directoryPath;
        public string DirectoryPath
        {
            get { return _directoryPath; }
            set { _directoryPath = value; RaisePropertyChanged(() => DirectoryPath); }
        }

        public RelayCommand SelectDirectory { get; set; }

        public TrainViewModel()
        {
            SelectDirectory = new RelayCommand(OnSelectDirectory);
        }

        private void OnSelectDirectory()
        {
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false
            };
            //var result = dialog.ShowDialog();
            if (string.IsNullOrEmpty(dialog.SelectedPath)) return;

            var path = dialog.SelectedPath;
            DirectoryPath = path;

            var files = DirSearch(path);

            //TODO: files is a List<string> now you can parse them all!
        }

        private List<string> DirSearch(string path)
        {
            var files = new List<string>();
            try
            {
                foreach (var d in Directory.GetDirectories(path))
                {
                    foreach (var f in Directory.GetFiles(d))
                    {
                        // Filter out all the DYN files
                        if(f.Contains("DYN"))
                            files.Add(f);
                    }
                    DirSearch(d);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
                return new List<string>();
            }

            return files;
        }
    }
}
