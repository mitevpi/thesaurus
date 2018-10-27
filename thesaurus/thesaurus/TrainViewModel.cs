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
            var files = DirSearch(path);


            //TODO: files is a List<string> now you can parse them all!
            ParseDYN.ParseDynData(DirectoryPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static List<string> DirSearch(string path)
        {
            var files = new List<string>();
            try
            {
                foreach (var d in Directory.GetDirectories(path))
                {
                    foreach (var f in Directory.GetFiles(d))
                    {
                        // Filter out all the DYN files
                        if (f.EndsWith("dyn", StringComparison.OrdinalIgnoreCase))
                            files.Add(f);
                    }
                    DirSearch(d);
                }

                foreach (var f in Directory.GetFiles(path))
                {
                    if (f.EndsWith("dyn", StringComparison.OrdinalIgnoreCase))
                        files.Add(f);
                }
            }
            catch (Exception e)
            {
                // ignore 
                return new List<string>();
            }

            return files;
        }
    }
}
