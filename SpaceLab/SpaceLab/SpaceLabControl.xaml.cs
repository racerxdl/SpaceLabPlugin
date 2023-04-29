using System.Windows;
using System.Windows.Controls;

namespace SpaceLab
{
    public partial class SpaceLabControl : UserControl
    {

        private SpaceLab Plugin { get; }

        private SpaceLabControl()
        {
            InitializeComponent();
        }

        public SpaceLabControl(SpaceLab plugin) : this()
        {
            Plugin = plugin;
            DataContext = plugin.Config;
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Plugin.Save();
        }
        private void ForceReloadButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Plugin.Save();
            Plugin.ForceReload();
        }
    }
}
