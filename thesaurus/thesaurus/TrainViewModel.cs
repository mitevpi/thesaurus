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
        public TrainModel Model { get; set; }
        private string _directoryPath;
        public string DirectoryPath
        {
            get { return _directoryPath; }
            set { _directoryPath = value; RaisePropertyChanged(() => DirectoryPath); }
        }

        private List<string> _files;
        public List<string> Files
        {
            get
            {
                return _files;
            }

            set { _files = value; }
        }

        public RelayCommand SelectDirectory { get; set; }

        public TrainViewModel(TrainModel model)
        {
            Model = model;
            DirectoryPath = "";
            Files = new List<string>();
            SelectDirectory = new RelayCommand(OnSelectDirectory);
        }

        private void OnSelectDirectory()
        {
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false
            };
            var unused = dialog.ShowDialog();
            if (string.IsNullOrEmpty(dialog.SelectedPath)) return;

            var path = dialog.SelectedPath;
            DirectoryPath = path;
            Files = new List<string>();
            DirSearch(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private void DirSearch(string path)
        {
            try
            {
                foreach (var d in Directory.GetDirectories(path))
                {
                    Console.Out.WriteLine(d);
                    foreach (var f in Directory.GetFiles(d))
                    {
                        // Filter out all the DYN files
                        if (f.EndsWith("dyn", StringComparison.OrdinalIgnoreCase))
                            Files.Add(f);
                    }
                    DirSearch(d);
                }

                foreach (var f in Directory.GetFiles(path))
                {
                    if (f.EndsWith("dyn", StringComparison.OrdinalIgnoreCase))
                        Files.Add(f);
                }
            }
            catch (Exception)
            {
                // ignore 
            }
        }
    }
}
