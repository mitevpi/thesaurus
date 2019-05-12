#region References

using System;
using System.IO;
using System.Windows;
using Dynamo.ViewModels;
using Dynamo.Wpf.Extensions;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

#endregion

namespace thesaurus
{
    public class ThesaurusViewExtension : IViewExtension
    {
        private static MenuItem _thesaurusMenuItem;
        private static MenuItem _thesaurusTrainMenuItem;
        private static MenuItem _thesaurusShowHideMenuItem;
        public static string ThesaurusDirectory { get; set; }

        public void Loaded(ViewLoadedParams p)
        {
            _thesaurusMenuItem = new MenuItem { Header = "the-Saurus" };
            var dynamoViewModel = p.DynamoWindow.DataContext as DynamoViewModel;

            #region Train Menu Item

            _thesaurusTrainMenuItem = new MenuItem
            {
                Header = "Train",
                ToolTip = new ToolTip {Content = "Train the-Saurus Machine Learning model."}
            };
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

            // (Konrad) Show an icon "x" if model has not been trained yet.
            SetIcon();

            #endregion

            #region Show/Hide

            _thesaurusShowHideMenuItem = new MenuItem
            {
                Header = "Show/Hide",
                ToolTip = new ToolTip {Content = "Show the-Saurus suggestions window."}
            };

            _thesaurusShowHideMenuItem.Click += (sender, args) =>
            {
                if (!Trained())
                {
                    var unused = MessageBox.Show("Please make sure to train your dino before letting it out of the cage.",
                        "the-Saurus Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
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
                }
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

        #region Utilites

        /// <summary>
        /// Utility method for checking if ML model was trained and Accord files exist.
        /// </summary>
        /// <returns>True if required files exist, otherwise false.</returns>
        private static bool Trained()
        {
            return File.Exists(Path.Combine(ThesaurusDirectory, "thesaurus_bayes.accord")) &&
                   File.Exists(Path.Combine(ThesaurusDirectory, "thesaurus_codebook.accord"));
        }

        /// <summary>
        /// Utility method for setting an icon on the MenuItem for the-Saurus, that indicates to the used if model was trained.
        /// </summary>
        public static void SetIcon()
        {
            Image icon;
            if (!Trained())
            {
                icon = new Image
                {
                    Source = new BitmapImage(
                        new Uri("pack://application:,,,/thesaurus;component/Resources/warning.png"))
                };
            }
            else
            {
                icon = new Image
                {
                    Source = new BitmapImage(
                        new Uri("pack://application:,,,/thesaurus;component/Resources/check.png"))
                };
            }

            _thesaurusTrainMenuItem.Icon = icon;
        }

        #endregion
    }
}
