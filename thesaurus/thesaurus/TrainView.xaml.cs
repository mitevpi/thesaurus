using System.Windows.Forms;
using System.Collections.Generic;

namespace thesaurus
{
    /// <summary>
    /// Interaction logic for TrainView.xaml
    /// </summary>
    public partial class TrainView
    {
        public TrainView()
        {
            InitializeComponent();
        }

        private void Train_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((this.DataContext as TrainViewModel).Files.Count != 0)
            {
                TrainViewModel trainViewModel = this.DataContext as TrainViewModel;
                List<string[]> trainingData = ParseDYN.ParseDynData(trainViewModel.Files);

                if (TrainModel.trainingMode == "bayes")
                {
                    trainViewModel.Model.TrainNaiveBayesClassifier(trainingData);
                }
                else if (TrainModel.trainingMode == "markov")
                {
                    trainViewModel.Model.TrainHiddenMarkovModel(trainingData);
                }
            }
            else
            {
                // Initializes the variables to pass to the MessageBox.Show method.
                string message = "You did not select a folder yet, please specify";
                string caption = "Error Detected in Folder Selection";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.
                result = MessageBox.Show(message, caption, buttons);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    // Closes the parent form.
                    this.Close();
                }
            }
        }
    }
}
