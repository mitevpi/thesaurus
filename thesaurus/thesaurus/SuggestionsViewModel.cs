using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;

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

            Nodes = new ObservableCollection<SuggestionsNodeViewModel>
            {
                new SuggestionsNodeViewModel(model) {NodeName = "Point.ByCoordinates"},
                new SuggestionsNodeViewModel(model) {NodeName = "Line.ByStartPointEndPoint"}
            };
        }
    }
}
