using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using thesaurus.Utilities;
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
                // (Aaron) Parse all the Dyns under DirectoryPath to CSV format
                var trainingData = ParseDYN.ParseDynData(Files);
                // (Aaron) Export the CSV to the folder user select
                ParseDYN.csvParser.ExportCSV(DirectoryPath);
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

                const string message = "You finished training ML module";
                const string caption = "Success";
                const MessageBoxButtons buttons = MessageBoxButtons.OK;
                var result = MessageBox.Show(message, caption, buttons);
                if (result == DialogResult.OK)
                {
                    Win?.Close();
                }
            }
            else
            {
                const string message = "Invalid folder, this could be that your are missing folder selection, or empty folder, or folder only containing Dynamo 1.X definitions. Please specify again.";
                const string caption = "Error Detected in Folder Selection";
                const MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, caption, buttons);
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
            Files = Helpers.DirSearch(path);
        }

        #endregion
    }
}
