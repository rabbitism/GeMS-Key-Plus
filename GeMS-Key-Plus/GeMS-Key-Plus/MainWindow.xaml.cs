using AutoHotkey.Interop;
using GeMS_Key_Plus.Global;
using GeMS_Key_Plus.Models;
using GeMS_Key_Plus.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeMS_Key_Plus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HotKey _hotKey;
        private AutoHotkeyEngine Ahk;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
            _hotKey = new HotKey(Key.G, KeyModifier.Alt, Copy);
        }

        private void ColorZone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Copy(HotKey hotkey)
        {
            if (hotkey == null) throw new ArgumentNullException(nameof(hotkey));
            Ahk = AutoHotkeyEngine.Instance;
            if (WindowState == WindowState.Minimized)
            {
                Ahk.ExecRaw("Send, ^c");
                Thread.Sleep(300);
                (this.DataContext as MainViewModel).QueryString = Clipboard.GetText().Trim();
                WindowState = WindowState.Normal;
                Activate();
                Focus();
                ShowInTaskbar = true;              
            }
            else
            {
                WindowState = WindowState.Minimized;
                ShowInTaskbar = true;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _hotKey.Dispose();
            base.OnClosing(e);
        }

        private void ButtonPanelView_KeyUp(object sender, KeyEventArgs e)
        {
            if(this.DataContext is MainViewModel vm)
            {
                vm.Query(e);
            }
        }
    }
}
