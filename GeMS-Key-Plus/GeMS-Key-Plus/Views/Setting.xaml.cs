using GeMS_Key_Plus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GeMS_Key_Plus.Views
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : Window
    {
        private MainViewModel _mainViewModel;
        public Setting()
        {
            InitializeComponent();
            _mainViewModel = null;
            this.DataContext = new SettingViewModel();
        }

        public Setting(MainViewModel mainViewModel)
        {
            InitializeComponent();
            _mainViewModel = new MainViewModel();
            this.DataContext = new SettingViewModel();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _mainViewModel?.ReloadButtonsCommand?.Execute();
            this.Close();
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
