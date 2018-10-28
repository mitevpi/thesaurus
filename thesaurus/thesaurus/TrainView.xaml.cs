using System.Windows.Forms;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Train_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((DataContext as TrainViewModel).Files.Count != 0)
            {
                var trainViewModel = DataContext as TrainViewModel;
                var trainingData = ParseDYN.ParseDynData(trainViewModel.Files);
                trainViewModel.Model.TrainHiddenMarkovModel(trainingData);
            }
            else
            {
                // Initializes the variables to pass to the MessageBox.Show method.
                const string message = "You did not select a folder yet, please specify";
                const string caption = "Error Detected in Folder Selection";
                const MessageBoxButtons buttons = MessageBoxButtons.OK;

                // Displays the MessageBox.
                var result = MessageBox.Show(message, caption, buttons);

                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    // Closes the parent form.
                    Close();
                }
            }
        }
    }
}