#region References

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using thesaurus.Utilities;
using MessageBox = System.Windows.Forms.MessageBox;

#endregion

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
        /// Handler for Train button. Parses selected DYN files to create train data-set.
        /// </summary>
        private void OnTrain()
        {
            if (Files.Any())
            {
                // (Aaron) Parse all the Dyn files under DirectoryPath to CSV format
                var trainingData = ParseDYN.ParseDynData(Files);

                // (Konrad) Export the CSV to the %AppData%\thesaurus folder
                // That's the only location that we are sure user will have write access
                // This makes it possible for user to train his/her model on graphs
                // stored somewhere on the network where they are "read-only" for non-admins.
                ParseDYN.csvParser.ExportCSV();
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

                var message = Properties.Resources.Train_Success;
                const string caption = "Success";
                const MessageBoxButtons buttons = MessageBoxButtons.OK;
                var result = MessageBox.Show(message, caption, buttons, MessageBoxIcon.Information);
                if (result == DialogResult.OK)
                {
                    Win?.Close();
                }
            }
            else
            {
                var message = Properties.Resources.Train_FolderError;
                const string caption = "Error";
                const MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, caption, buttons, MessageBoxIcon.Error);
            }

            // (Konrad) Reset MenuItem icon.
            ThesaurusViewExtension.SetIcon();
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
