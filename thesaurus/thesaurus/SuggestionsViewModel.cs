using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Dynamo.Graph.Nodes;

namespace thesaurus
{
    public class SuggestionsViewModel : ViewModelBase
    {
        public SuggestionsModel Model { get; set; }

        private ObservableCollection<SuggestionsNodeViewModel> _nodes;
        public ObservableCollection<SuggestionsNodeViewModel> Nodes
        {
            get { return _nodes; }
            set { _nodes = value; RaisePropertyChanged(() => Nodes); }
        }

        public SuggestionsViewModel(SuggestionsModel model)
        {
            Model = model;
            Nodes = new ObservableCollection<SuggestionsNodeViewModel> { };

            this.Model.DynamoViewModel.Model.CurrentWorkspace.NodeAdded += delegate(NodeModel nodeModel)
            {
                Nodes.Clear();
                var inputName = nodeModel.CreationName;
                if (string.IsNullOrEmpty(inputName)) inputName = nodeModel.GetType().FullName;
                string[] predictions = model.Predict(inputName);

                // For each prediction, construct a SuggestionsNodeViewModel ready 
                // for user to select as next potential node
                foreach (var predictedNode in predictions)
                {
                    Nodes.Add(new SuggestionsNodeViewModel(model) { NodeName = predictedNode.Split('@')[0] });
                }
            };
        }
    }
}
