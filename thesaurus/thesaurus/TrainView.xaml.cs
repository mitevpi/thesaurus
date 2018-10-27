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
            // TODO: Parse all the files here
            foreach (var f in (this.DataContext as TrainViewModel).Files)
            {
                
            }   
        }
    }
}
