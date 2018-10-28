using GalaSoft.MvvmLight.Command;
using ViewModelBase = GalaSoft.MvvmLight.ViewModelBase;

namespace thesaurus
{
    public class SuggestionsNodeViewModel : ViewModelBase
    {
        public SuggestionsModel Model { get; set; }
        public RelayCommand PlaceNode { get; set; }

        private string _nodeName;
        public string NodeName {
            get { return _nodeName; }
            set { _nodeName = value; RaisePropertyChanged(() => NodeName); }
        }

        public SuggestionsNodeViewModel(SuggestionsModel model)
        {
            Model = model;

            PlaceNode = new RelayCommand(OnPlaceNode);
        }

        private void OnPlaceNode()
        {
            Model.PlaceNode(NodeName);
        }
    }
}
