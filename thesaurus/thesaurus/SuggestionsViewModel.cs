using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Dynamo.Graph.Nodes;
using GalaSoft.MvvmLight.Command;

namespace thesaurus
{
    public class SuggestionsViewModel : ViewModelBase
    {
        public SuggestionsModel Model { get; set; }
        public RelayCommand WindowClosing { get; set; }

        private ObservableCollection<SuggestionsNodeViewModel> _nodes;
        public ObservableCollection<SuggestionsNodeViewModel> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; RaisePropertyChanged(() => Nodes); }
        }

        public SuggestionsViewModel(SuggestionsModel model)
        {
            Model = model;
            WindowClosing = new RelayCommand(OnWindowClosing);
            Nodes = new ObservableCollection<SuggestionsNodeViewModel>();

            Model.DynamoViewModel.Model.CurrentWorkspace.NodeAdded += OnNodeAdded;
        }

        /// <summary>
        /// Handler for Window closing event.
        /// </summary>
        private void OnWindowClosing()
        {
            // (Konrad) Remove binding to NodeAdded event if the Suggestions window is closed.
            Model.DynamoViewModel.Model.CurrentWorkspace.NodeAdded -= OnNodeAdded;
        }

        /// <summary>
        /// Handler for NodeAdded event in Dynamo Workspace.
        /// </summary>
        /// <param name="nodeModel">Node Model for the added Node.</param>
        private void OnNodeAdded(NodeModel nodeModel)
        {
            // (Aaron) Hook up with running ML module here and provide nodeModel.CreationName as input
            // Then construct a SuggestionsNodeViewModel based on that info, the panel should update automatically

            Nodes.Clear();
            var inputName = nodeModel.CreationName;
            if (string.IsNullOrEmpty(inputName)) inputName = nodeModel.GetType().FullName;
            var predictions = Model.Predict(inputName);

            // (Aaron) For each prediction, construct a SuggestionsNodeViewModel ready 
            // for user to select as next potential node
            foreach (var predictedNode in predictions)
            {
                Nodes.Add(new SuggestionsNodeViewModel(Model) { NodeName = predictedNode.Split('@')[0] });
            }
        }
    }
}