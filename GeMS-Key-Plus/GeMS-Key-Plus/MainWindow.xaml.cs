using AutoHotkey.Interop;
using GeMS_Key_Plus.Events;
using GeMS_Key_Plus.Models;
using GeMS_Key_Plus.ViewModels;
using GeMS_Key_Plus.Views;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace GeMS_Key_Plus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        private HotKey _hotKey;
        private AutoHotkeyEngine _ahk;
        private System.Windows.Forms.NotifyIcon _icon;
        private bool disposedValue;

        public MainWindow()
        {
            InitializeComponent();
            InitializeNotifyIcon();
            this.DataContext = new MainViewModel();
            _hotKey = new HotKey(Key.G, KeyModifier.Alt, Copy);
            EventAggregatorRepository
                .GetInstance()
                .EventAggregator
                .GetEvent<MinimizeWindowEvent>()
                .Subscribe(OnMinimizeWindowEventRaised);
        }

        private void InitializeNotifyIcon()
        {
            _icon ??= new System.Windows.Forms.NotifyIcon();
            _icon.Icon = new System.Drawing.Icon("Key.ico");
            _icon.MouseClick += ShowWindow;
            _icon.Visible = true;
            _icon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

            _icon.ContextMenuStrip.Items.AddRange(
                new System.Windows.Forms.ToolStripItem[]
                {
                    new System.Windows.Forms.ToolStripLabel("Keyz"),
                    new System.Windows.Forms.ToolStripSeparator(),
                    new System.Windows.Forms.ToolStripMenuItem("Close", null, (o, e) =>
                    {
                        this.Dispose(disposedValue);
                        this.Close();
                    }),
                }

                );

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
            // this.WindowState = WindowState.Minimized;
            SystemCommands.MinimizeWindow(this);
            Hide();
        }

        private void Copy(HotKey hotkey)
        {
            if (hotkey == null) throw new ArgumentNullException(nameof(hotkey));
            _ahk = AutoHotkeyEngine.Instance;
            if (WindowState == WindowState.Minimized)
            {
                _ahk.ExecRaw("Send, ^c");
                Thread.Sleep(200);

                if (this.DataContext is MainViewModel vm)
                {
                    vm.QueryString = Clipboard.GetText().Trim();
                }
                SystemCommands.RestoreWindow(this);
                Show();
                Activate();
                // Keyboard.Focus(buttonPanelView);
                buttonPanelView.Focus();
                FocusManager.SetFocusedElement(this, buttonPanelView);
            }
            else
            {
                OnMinimizeWindowEventRaised();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _hotKey?.Dispose();
            base.OnClosing(e);
        }

        private void ButtonPanelView_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.DataContext is MainViewModel vm)
            {
                vm.Query(e);
            }
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            Setting settingWindow = new Setting();
            settingWindow.Show();
        }

        private void ShowWindow(object sender, System.Windows.Forms.MouseEventArgs args)
        {
            if (args.Button == System.Windows.Forms.MouseButtons.Left)
            {
                SystemCommands.RestoreWindow(this);
                Show();
                Activate();
            }

        }

        private void OnMinimizeWindowEventRaised()
        {
            SystemCommands.MinimizeWindow(this);
            Hide();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _hotKey?.Dispose();
                    _icon?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MainWindow()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
