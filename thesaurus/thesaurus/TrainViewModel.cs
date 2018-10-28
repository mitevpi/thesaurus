using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MessageBox = System.Windows.Forms.MessageBox;

namespace thesaurus
{
    public class TrainViewModel : ViewModelBase
    {
        #region Properties

        private Window Win { get; set; }
        public TrainModel Model { get; set; }
        public List<string> Files { get; set; }
        public RelayCommand SelectDirectory { get; set; }
        public RelayCommand Train { get; set; }
        public RelayCommand<Window> WindowLoaded { get; set; }

        private string _directoryPath;
        public string DirectoryPath
        {
            get { return _directoryPath; }
            set { _directoryPath = value; RaisePropertyChanged(() => DirectoryPath); }
        }

        #endregion

        public TrainViewModel(TrainModel model)
        {
            Model = model;
            DirectoryPath = "";
            Files = new List<string>();
            SelectDirectory = new RelayCommand(OnSelectDirectory);
            Train = new RelayCommand(OnTrain);
            WindowLoaded = new RelayCommand<Window>(OnWindowLoaded);
        }

        #region Handlers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void OnWindowLoaded(Window obj)
        {
            Win = obj;
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnTrain()
        {
            if (Files.Any())
            {
                var trainingData = ParseDYN.ParseDynData(Files);
                Model.TrainHiddenMarkovModel(trainingData);
            }
            else
            {
                // Initializes the variables to pass to the MessageBox.Show method.
                const string message = "You did not select a folder yet, please specify";
                const string caption = "Error Detected in Folder Selection";
                const MessageBoxButtons buttons = MessageBoxButtons.OK;

                // Displays the MessageBox.
                var result = MessageBox.Show(message, caption, buttons);

                if (result == DialogResult.Yes)
                {
                    // Closes the parent form.
                    Win?.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
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

        #endregion

        #region Utilities

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

        #endregion
    }
}
