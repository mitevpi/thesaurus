using System;
using System.IO;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System.Windows.Controls;

namespace thesaurus
{
    public class ThesaurusViewExtension : IViewExtension
    {
        private MenuItem _thesaurusMenuItem;
        private MenuItem _thesaurusTrainMenuItem;
        private MenuItem _thesaurusShowHideMenuItem;
        public static string ThesaurusDirectory { get; set; }

        public void Loaded(ViewLoadedParams p)
        {
            _thesaurusMenuItem = new MenuItem { Header = "the-Saurus" };
            var dynamoViewModel = p.DynamoWindow.DataContext as DynamoViewModel;

            #region Train Menu Item

            _thesaurusTrainMenuItem = new MenuItem { Header = "Train" };
            _thesaurusTrainMenuItem.ToolTip = new ToolTip { Content = "Train the-Saurus Machine Learning model." };
            _thesaurusTrainMenuItem.Click += (sender, args) =>
            {
                var m = new TrainModel();
                var viewModel = new TrainViewModel(m);
                var window = new TrainView
                {
                    DataContext = viewModel,
                    Owner = p.DynamoWindow
                };
                window.Left = window.Owner.Left + 400;
                window.Top = window.Owner.Top + 200;
                window.Show();
            };
            _thesaurusMenuItem.Items.Add(_thesaurusTrainMenuItem);

            #endregion

            #region Show/Hide

            _thesaurusShowHideMenuItem = new MenuItem { Header = "Show/Hide" };
            _thesaurusShowHideMenuItem.ToolTip = new ToolTip { Content = "Show the-Saurus suggestions window." };
            _thesaurusShowHideMenuItem.Click += (sender, args) =>
            {
                var m = new SuggestionsModel(dynamoViewModel);
                var viewModel = new SuggestionsViewModel(m);
                var window = new SuggestionsView
                {
                    DataContext = viewModel,
                    Owner = p.DynamoWindow
                };
                window.Left = window.Owner.Left + 400;
                window.Top = window.Owner.Top + 200;
                window.Show();
            };
            _thesaurusMenuItem.Items.Add(_thesaurusShowHideMenuItem);

            #endregion

            p.dynamoMenu.Items.Add(_thesaurusMenuItem);
        }

        public string Name
        {
            get { return "the-Saurus"; }
        }

        public string UniqueId
        {
            get { return "181fe1bd-2762-4845-878e-e4e98d55f28f"; }
        }

        public void Dispose()
        {
        }

        public void Shutdown()
        {
        }

        public void Startup(ViewStartupParams p)
        {
            ThesaurusDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "thesaurus");
            if (!Directory.Exists(ThesaurusDirectory)) Directory.CreateDirectory(ThesaurusDirectory);
        }
    }
}
