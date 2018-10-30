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
            DirectoryPath = string.Empty;
            Files = new List<string>();
            SelectDirectory = new RelayCommand(OnSelectDirectory);
            Train = new RelayCommand(OnTrain);
            WindowLoaded = new RelayCommand<Window>(OnWindowLoaded);
        }

        #region Handlers

        /// <summary>
        /// Handler for Window Loaded event. Stores reference to Window on VM.
        /// </summary>
        /// <param name="win">Train Window.</param>
        private void OnWindowLoaded(Window win)
        {
            Win = win;
        }

        /// <summary>
        /// Handler for Train button. Parses selected DYN files to create train dataset.
        /// </summary>
        private void OnTrain()
        {
            if (Files.Any())
            {
                var trainingData = ParseDYN.ParseDynData(Files);
                Model.TrainHiddenMarkovModel(trainingData);

                switch (TrainModel.TrainingMode)
                {
                    case "bayes":
                        Model.TrainNaiveBayesClassifier(trainingData);
                        break;
                    case "markov":
                        Model.TrainHiddenMarkovModel(trainingData);
                        break;
                }
            }
            else
            {
                const string message = "You did not select a folder yet, please specify";
                const string caption = "Error Detected in Folder Selection";
                const MessageBoxButtons buttons = MessageBoxButtons.OK;

                var result = MessageBox.Show(message, caption, buttons);

                if (result == DialogResult.Yes)
                {
                    Win?.Close();
                }
            }
        }

        /// <summary>
        /// Handler for Select Directory button.
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
        /// Recursively search the folder to get all the DYN files. Stores them in Files variable.
        /// </summary>
        /// <param name="path">Directory path.</param>
        private void DirSearch(string path)
        {
            try
            {
                // (Konrad) Handler sub-directories recursively.
                foreach (var d in Directory.GetDirectories(path))
                {
                    foreach (var f in Directory.GetFiles(d))
                    {
                        // (Konrad) Filter out all the DYN files
                        if (f.EndsWith("dyn", StringComparison.OrdinalIgnoreCase)) Files.Add(f);
                    }
                    DirSearch(d);
                }

                // (Konrad) Handle main directory.
                foreach (var f in Directory.GetFiles(path))
                {
                    if (f.EndsWith("dyn", StringComparison.OrdinalIgnoreCase)) Files.Add(f);
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
