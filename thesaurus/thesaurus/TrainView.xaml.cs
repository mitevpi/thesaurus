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

        private void Train_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if ((this.DataContext as TrainViewModel).Files.Count != 0)
            {
                ParseDYN.ParseDynData((this.DataContext as TrainViewModel).Files);
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
