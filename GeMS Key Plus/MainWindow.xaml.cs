using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
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
using System.Windows.Interop;
using AutoHotkey.Interop;

namespace GemsKeyPlus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            this.Left = 100;
            this.Top = 100;            
            InitializeComponent();           
            this.WindowState = WindowState.Minimized;
            HotKey MainKey = new HotKey(Key.G, KeyModifier.Win, WinAndG);
            ///This hotkey is for Windows 10 users. Win+G is reserved by system to call out XBOX and game center bar. This might be fixed in the future.
            HotKey MainKeyInWin10 = new HotKey(Key.G, KeyModifier.Alt, WinAndG);
            
        }

        #region Field
        int CheckNumber;
        public string stringForCommunication;
        private List<string> PartNumbers;
        public string stringFroCommunication = "";
        enum PartType {NineDigPart, EightDigECO, SixDigIFR, TenDigDrawing, NineDigDrawing, FourteenDigQuest, FifteenDigPart, Default, Search, SevenDigitWO};
        
        #endregion

        /// <summary>
        /// Method of textChanged:
        /// 1. Adjust font size based on length of text. If it exceeds 30 characters, font size will be reduced to 16. Otherwise it will be kept as default 30.
        /// 2. Change UI layout based on format of text. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            stringForCommunication = (sender as TextBox).Text;           
            if ((sender as TextBox).Text.Length<=30 && (sender as TextBox).Text.Contains('\n')==false)
            {
                (sender as TextBox).FontSize = 30;
            }
            else
            {
                (sender as TextBox).FontSize = 16;
                Box.VerticalContentAlignment = VerticalAlignment.Top;
            }
            SplitNumbers();
            
            if(PartNumbers.Count<=1 && PartNumbers.Count>=0)
            {
                try
                {
                    SetLayout(CheckState(PartNumbers[0]));
                }
                catch(ArgumentOutOfRangeException)
                {
                    SetLayout(PartType.Default);
                }
                
            }
            else
            {
                SetLayout(PartType.Default);
            }
            
        }

        #region UIInteraction
        /// <summary>
        /// Check the state of number in text box, return a PartType.
        /// </summary>
        /// <param name="text">Text to be analyzed</param>
        /// <returns></returns>
        private PartType CheckState(string text)
        {
            if(text.Length == 9 && int.TryParse(text, out CheckNumber))
            {
                return PartType.NineDigPart;
            }
            if (text.Length == 7 && int.TryParse(text, out CheckNumber))
            {
                return PartType.SevenDigitWO;
            }
            if (text.Length == 9 && text.Replace("-",string.Empty).Length==8)
            {
                return PartType.NineDigDrawing;
            }
            if(text.Length == 10 && text.Replace("D",string.Empty).Length==9)
            {
                return PartType.TenDigDrawing;
            }
            if(text.Length == 8 && int.TryParse(text, out CheckNumber))
            {
                return PartType.EightDigECO;
            }
            if(text.Length == 6 && int.TryParse(text, out CheckNumber))
            {
                return PartType.SixDigIFR;
            }
            if(text.Length == 14/* && int.TryParse(text, out CheckNumber)*/)
            {
                return PartType.FourteenDigQuest;
            }
            if(text.Length == 15 && text.Replace("-",string.Empty).Length==13)
            {
                return PartType.FifteenDigPart;
            }
            else
            {
                return PartType.Search;
            }
        }
        /// <summary>
        /// Set UI buttons layout based on PartType returned from CheckState()
        /// </summary>
        /// <param name="partType"></param>
        private void SetLayout(PartType partType)
        {
            switch (partType)
            {
                case PartType.Search:
                    {
                        PartPanel.Visibility = Visibility.Collapsed;
                        BOMPanel.Visibility = Visibility.Collapsed;
                        DocPanel.Visibility = Visibility.Collapsed;
                        DwgPanel.Visibility = Visibility.Collapsed;
                        ChangePanel.Visibility = Visibility.Collapsed;                        
                        EqualityPanel.Visibility = Visibility.Collapsed;
                        eCPUPanel.Visibility = Visibility.Collapsed;
                        SearchPanel.Visibility = Visibility.Visible;
                        OtherPanel.Visibility = Visibility.Collapsed;
                        MoreButton.Visibility = Visibility.Visible;
                        break;
                    }
                case PartType.NineDigPart:
                    {
                        PartPanel.Visibility = Visibility.Visible;
                        BOMPanel.Visibility = Visibility.Visible;
                        DocPanel.Visibility = Visibility.Visible;
                        DwgPanel.Visibility = Visibility.Visible;
                        ChangePanel.Visibility = Visibility.Visible;
                        EqualityPanel.Visibility = Visibility.Collapsed;
                        eCPUPanel.Visibility = Visibility.Collapsed;
                        SearchPanel.Visibility = Visibility.Visible;
                        OtherPanel.Visibility = Visibility.Collapsed;
                        MoreButton.Visibility = Visibility.Visible;
                        break;
                    }
                case PartType.EightDigECO:
                    {
                        PartPanel.Visibility = Visibility.Collapsed;
                        BOMPanel.Visibility = Visibility.Collapsed;
                        DocPanel.Visibility = Visibility.Collapsed;
                        DwgPanel.Visibility = Visibility.Collapsed;
                        ChangePanel.Visibility = Visibility.Visible;
                        EqualityPanel.Visibility = Visibility.Collapsed;
                        eCPUPanel.Visibility = Visibility.Collapsed;
                        SearchPanel.Visibility = Visibility.Visible;
                        OtherPanel.Visibility = Visibility.Collapsed;
                        MoreButton.Visibility = Visibility.Visible;
                        break;
                    }
                case PartType.SixDigIFR:
                    {
                        PartPanel.Visibility = Visibility.Collapsed;
                        BOMPanel.Visibility = Visibility.Collapsed;
                        DocPanel.Visibility = Visibility.Collapsed;
                        DwgPanel.Visibility = Visibility.Collapsed;
                        ChangePanel.Visibility = Visibility.Collapsed;
                        EqualityPanel.Visibility = Visibility.Visible;
                        eCPUPanel.Visibility = Visibility.Collapsed;
                        SearchPanel.Visibility = Visibility.Visible;
                        OtherPanel.Visibility = Visibility.Collapsed;
                        MoreButton.Visibility = Visibility.Visible;
                        break;
                    }
                case PartType.TenDigDrawing:
                    {
                        PartPanel.Visibility = Visibility.Collapsed;
                        BOMPanel.Visibility = Visibility.Collapsed;
                        DocPanel.Visibility = Visibility.Collapsed;
                        DwgPanel.Visibility = Visibility.Visible;
                        ChangePanel.Visibility = Visibility.Collapsed;
                        EqualityPanel.Visibility = Visibility.Collapsed;
                        eCPUPanel.Visibility = Visibility.Collapsed;
                        SearchPanel.Visibility = Visibility.Collapsed;
                        OtherPanel.Visibility = Visibility.Collapsed;
                        MoreButton.Visibility = Visibility.Visible;
                        break;
                    }
                case PartType.NineDigDrawing:
                    {
                        PartPanel.Visibility = Visibility.Collapsed;
                        BOMPanel.Visibility = Visibility.Collapsed;
                        DocPanel.Visibility = Visibility.Visible;
                        DwgPanel.Visibility = Visibility.Visible;
                        ChangePanel.Visibility = Visibility.Collapsed;
                        EqualityPanel.Visibility = Visibility.Collapsed;
                        eCPUPanel.Visibility = Visibility.Collapsed;
                        SearchPanel.Visibility = Visibility.Collapsed;
                        OtherPanel.Visibility = Visibility.Collapsed;
                        MoreButton.Visibility = Visibility.Visible;
                        break;
                    }
                case PartType.FourteenDigQuest:
                    {
                        PartPanel.Visibility = Visibility.Collapsed;
                        BOMPanel.Visibility = Visibility.Collapsed;
                        DocPanel.Visibility = Visibility.Collapsed;
                        DwgPanel.Visibility = Visibility.Collapsed;
                        ChangePanel.Visibility = Visibility.Collapsed;
                        EqualityPanel.Visibility = Visibility.Collapsed;
                        eCPUPanel.Visibility = Visibility.Collapsed;
                        SearchPanel.Visibility = Visibility.Collapsed;
                        OtherPanel.Visibility = Visibility.Visible;
                        MoreButton.Visibility = Visibility.Visible;
                        break;
                    }
                case PartType.FifteenDigPart:
                    {
                        PartPanel.Visibility = Visibility.Visible;
                        BOMPanel.Visibility = Visibility.Visible;
                        DocPanel.Visibility = Visibility.Collapsed;
                        DwgPanel.Visibility = Visibility.Collapsed;
                        ChangePanel.Visibility = Visibility.Collapsed;
                        EqualityPanel.Visibility = Visibility.Collapsed;
                        eCPUPanel.Visibility = Visibility.Collapsed;
                        SearchPanel.Visibility = Visibility.Collapsed;
                        OtherPanel.Visibility = Visibility.Collapsed;
                        MoreButton.Visibility = Visibility.Visible;
                        break;
                    }
                case PartType.SevenDigitWO:
                    {
                        PartPanel.Visibility = Visibility.Collapsed;
                        BOMPanel.Visibility = Visibility.Collapsed;
                        DocPanel.Visibility = Visibility.Collapsed;
                        DwgPanel.Visibility = Visibility.Collapsed;
                        ChangePanel.Visibility = Visibility.Collapsed;
                        EqualityPanel.Visibility = Visibility.Collapsed;
                        eCPUPanel.Visibility = Visibility.Visible;
                        SearchPanel.Visibility = Visibility.Visible;
                        OtherPanel.Visibility = Visibility.Collapsed;
                        MoreButton.Visibility = Visibility.Visible;
                        break;
                    }
                case PartType.Default:
                    {
                        PartPanel.Visibility = Visibility.Visible;
                        BOMPanel.Visibility = Visibility.Visible;
                        DocPanel.Visibility = Visibility.Visible;
                        DwgPanel.Visibility = Visibility.Visible;
                        ChangePanel.Visibility = Visibility.Visible;
                        EqualityPanel.Visibility = Visibility.Visible;
                        eCPUPanel.Visibility = Visibility.Visible;
                        SearchPanel.Visibility = Visibility.Visible;
                        OtherPanel.Visibility = Visibility.Visible;
                        MoreButton.Visibility = Visibility.Collapsed;
                        break;
                    }
            }
            
        }
        /// <summary>
        /// When mouse swipe through this area, the UI will be expanded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoreButton_MouseEnter(object sender, MouseEventArgs e)
        {            
            PartPanel.Visibility = Visibility.Visible;
            BOMPanel.Visibility = Visibility.Visible;
            DocPanel.Visibility = Visibility.Visible;
            DwgPanel.Visibility = Visibility.Visible;
            ChangePanel.Visibility = Visibility.Visible;
            EqualityPanel.Visibility = Visibility.Visible;
            eCPUPanel.Visibility = Visibility.Visible;
            SearchPanel.Visibility = Visibility.Visible;
            OtherPanel.Visibility = Visibility.Visible;
            MoreButton.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// Make the UI moveable by click and drag
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DockPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        #region MenuButtons
        /// <summary>
        /// Click Close button will minimize UI instead of close the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void DocumentButton_Click(object sender, RoutedEventArgs e)
        {
            var CreditWindow = new Credit();
            CreditWindow.Owner = Application.Current.MainWindow;
            CreditWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CreditWindow.Show();
        }

        private void ToolButton_Click(object sender, RoutedEventArgs e)
        {
            var StaticWindow = new Tools(this);
            StaticWindow.Owner = Application.Current.MainWindow;
            StaticWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            StaticWindow.Show();
        }
        #endregion

        #endregion

        /// <summary>
        /// Define Actions for every button: Open specific part number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((sender as Button).Name != "GoogleButton" && (sender as Button).Name != "WikiButton" && (sender as Button).Name != "YouDaoButton")
                {
                    foreach (string PartNumber in PartNumbers)
                    {
                        if (PartNumber != string.Empty && PartNumber != "\n")
                        {
                            PartNumber.Trim();
                            OpenLink(sender as Button, PartNumber);
                            Thread.Sleep(100);
                        }
                    }
                }
                else
                {
                    OpenLink(sender as Button, Box.Text);
                }
                WindowState = WindowState.Minimized;
                ShowInTaskbar = true;
            }
            catch(NullReferenceException )
            {
                WindowState = WindowState.Minimized;
            }
            
        }

        #region Methods
        /// <summary>
        /// Open link based on button x:Name
        /// </summary>
        /// <param name="button"></param>
        /// <param name="number"></param>
        private void OpenLink(Button button, string number)
        {
            switch (button.Name)
            {
                case "ReleasedPartButton": Process.Start("http://www.google.com " + number + "&Rev=!"); break;
                case "CreatedPartButton": Process.Start("http://www.google.com" + number + "&Rev=-"); break;
                case "AllPartButton": Process.Start("http://www.google.com" + number + "&Rev=*"); break;
                case "ReleasedBOMButton": Process.Start("http://www.google.com" + number + "&Rev=!&DefaultCategory=ENCEBOMPowerViewCommand"); break;
                case "CreatedBOMButton": Process.Start("http://www.google.com" + number + "&Rev=-&DefaultCategory=ENCEBOMPowerViewCommand"); break;
                case "ReleasedDocButton": Process.Start("http://www.google.com" + number + "&Rev=!"); break;
                case "CreatedDocButton": Process.Start("http://www.google.com" + number + "&Rev=-"); break;
                case "AllDocButton": Process.Start("http://www.google.com" + number + "&Rev=*"); break;
                case "RelatedDocButton": Process.Start("http://www.google.com" + number + "&Rev=!&DefaultCategory=SLBDOCUMENTSRelatedItemsPowerView"); break;
                case "ReleasedDwgButton": Process.Start("http://www.google.com" + number + "&Rev=!"); break;
                case "CreatedDwgButton": Process.Start("http://www.google.com" + number + "&Rev=-"); break;
                case "AllDwgButton": Process.Start("http://www.google.com" + number + "&Rev=*"); break;
                case "RelatedDwgButton": Process.Start("http://www.google.com" + number + "&Rev=!&DefaultCategory= SLBDOCUMENTSRelatedItemsPowerView"); break;
                case "ECOButton": Process.Start("http://www.google.com" + number + "&Rev=-"); break;
                case "ECRButton": Process.Start("http://www.google.com" + number + "&Rev=-"); break;
                case "IFRButton": Process.Start("http://www.google.com" + number); break;
                case "TFLButton": Process.Start("http://www.google.com" + number); break;
                case "DIButton": Process.Start("http://www.google.com" + number); break;
                case "PackageButton":
                    {
                        try
                        {
                            Process.Start("http://www.google.com" + number);
                        }
                        catch
                        {
                            Process.Start("http://www.google.com");
                        }
                         break;
                    } 

                case "RoutingButton":
                    {
                        try
                        {
                            Process.Start("http://www.google.com" + number + "/Routing/");
                        }
                        catch
                        {
                            Process.Start("http://www.google.com");
                        }
                         break;
                    }                       
                case "GoogleButton": Process.Start("http://www.google.com/search?q=" + number); break;
                case "WikiButton": Process.Start("https://en.wikipedia.org/wiki/" + number); break;
                case "YouDaoButton": Process.Start("http://dict.youdao.com/search?q=" + number); break;
                case "QuestButton": Process.Start("http://www.google.com" + number); break;
                case "IntouchButton": Process.Start("http://www.google.com" + number); break;
                case "ReleasedBOMButtonWithPart":
                    {
                        Process.Start("http://www.google.com" + number + "&Rev=!");
                        Process.Start("http://www.google.com" + number + "&Rev=!&DefaultCategory=ENCEBOMPowerViewCommand");
                        break;
                    }
                case "CreatedBOMButtonWithPart":
                    {
                        Process.Start("http://www.google.com" + number + "&Rev=-");
                        Process.Start("http://www.google.com" + number + "&Rev=-&DefaultCategory=ENCEBOMPowerViewCommand");
                        break;
                    }
            }
            Box.Text = String.Empty;
        }
        /// <summary>
        /// Divide textbox string with , : ; \t \r and \n
        /// </summary>
        private void SplitNumbers()
        {
            string[] Splitter = {" ", ",", ";","\r","\n","\t"};
            //Set split option to remove empty entries to avoid opening empty pages.
            PartNumbers = new List<string>(Box.Text.Split(Splitter, StringSplitOptions.RemoveEmptyEntries));
            PartNumbers = PartNumbers.Distinct().ToList();
        }
        #endregion


        #region HotKeySystem

        //This is to create a ahk instance and send ctrl+c in an easy way. Autohotkey.Interop dll should be packed together with finaly released exe file. 
        AutoHotkeyEngine ahk;
        

        public List<string> PartNumbers1
        {
            get
            {
                return PartNumbers;
            }

            set
            {
                PartNumbers = value;
            }
        }

        /// <summary>
        /// Win+G Interaction logic:
        /// 1. If window is minimized, return to normal size, copy highlighted text to clipboard, pass clipboard text to textbox
        /// 2. if window is normal, minimize instead of close
        /// </summary>
        /// <param name="hotkey"></param>
        private void WinAndG(HotKey hotkey)
        {
            ahk = AutoHotkeyEngine.Instance;            
            if (WindowState == WindowState.Minimized)
            {
                ahk.ExecRaw("Send, ^c");
                Thread.Sleep(300);
                Box.Text = Clipboard.GetText().Trim();
                stringForCommunication = Box.Text;
                this.Top = 100;
                this.Left = 100;               
                WindowState = WindowState.Normal;
                PartLabel.Focus();
                Activate();
                Focus();
                ShowInTaskbar = true;
                //Topmost = true;                
            }
            else
            {
                WindowState = WindowState.Minimized;
                ShowInTaskbar = true;
            }
        }
        /// <summary>
        /// Hotkeys when UI is opened. Space and enter is universal hotkey, its behavior is based on text format in textbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///Hotkeys when window is open
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(!Box.IsFocused && Box.Text!=string.Empty)
            {
                if (e.Key == Key.G) Button_Click(ReleasedPartButton, e);
                if (e.Key == Key.B) Button_Click(ReleasedBOMButton, e);
                if (e.Key == Key.D) Button_Click(ReleasedDocButton, e);
                if (e.Key == Key.W) Button_Click(ReleasedDwgButton, e);
                if (e.Key == Key.E) Button_Click(ECOButton, e);
                if (e.Key == Key.R) Button_Click(ECRButton, e);
                if (e.Key == Key.F) Button_Click(IFRButton, e);
                if (e.Key == Key.S) Button_Click(GoogleButton, e);
                if (e.Key == Key.Q) Button_Click(QuestButton, e);
                if (e.Key == Key.I) Button_Click(IntouchButton, e);
                if (e.Key == Key.A) Button_Click(ReleasedBOMButtonWithPart, e);
                if (e.Key == Key.C) Button_Click(PackageButton, e);
                if (e.Key == Key.T) Button_Click(RoutingButton, e);
                if (e.Key == Key.Escape) WindowState = WindowState.Minimized;
                if (e.Key == Key.Enter || e.Key == Key.Space)
                {
                    //set e.handled to true to override default behavior of space button(perform the last action)
                    e.Handled = true;
                    //Checkstate is based on the first element of list.
                    var ActualState = CheckState(PartNumbers[0].Trim());
                    switch (ActualState)
                    {
                        case PartType.NineDigPart: Button_Click(ReleasedPartButton, e); break;
                        case PartType.EightDigECO: Button_Click(ECOButton, e); break;
                        case PartType.SixDigIFR: Button_Click(IFRButton, e); break;
                        case PartType.TenDigDrawing: Button_Click(ReleasedDwgButton, e); break;
                        case PartType.NineDigDrawing: Button_Click(ReleasedDwgButton, e); break;
                        case PartType.FourteenDigQuest: Button_Click(QuestButton, e); break;
                        case PartType.FifteenDigPart: Button_Click(ReleasedPartButton, e); break;
                        case PartType.Default: Button_Click(GoogleButton, e); break;
                        case PartType.SevenDigitWO: Button_Click(PackageButton, e);break;
                        case PartType.Search: Button_Click(GoogleButton, e); break;

                    }
                }
            }
            else
            {
                Box.Focus();
            }            
        }
        #endregion


    }

    public class HotKey : IDisposable
    {
        private static Dictionary<int, HotKey> _dictHotKeyToCalBackProc;

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, UInt32 fsModifiers, UInt32 vlc);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public const int WmHotKey = 0x0312;

        private bool _disposed = false;

        public Key Key { get; private set; }
        public KeyModifier KeyModifiers { get; private set; }
        public Action<HotKey> Action { get; private set; }
        public int Id { get; set; }

        // ******************************************************************
        public HotKey(Key k, KeyModifier keyModifiers, Action<HotKey> action, bool register = true)
        {
            Key = k;
            KeyModifiers = keyModifiers;
            Action = action;
            if (register)
            {
                Register();
            }
        }

        // ******************************************************************
        public bool Register()
        {
            int virtualKeyCode = KeyInterop.VirtualKeyFromKey(Key);
            Id = virtualKeyCode + ((int)KeyModifiers * 0x10000);
            bool result = RegisterHotKey(IntPtr.Zero, Id, (UInt32)KeyModifiers, (UInt32)virtualKeyCode);

            if (_dictHotKeyToCalBackProc == null)
            {
                _dictHotKeyToCalBackProc = new Dictionary<int, HotKey>();
                ComponentDispatcher.ThreadFilterMessage += new ThreadMessageEventHandler(ComponentDispatcherThreadFilterMessage);
            }

            _dictHotKeyToCalBackProc.Add(Id, this);

            Debug.Print(result.ToString() + ", " + Id + ", " + virtualKeyCode);
            return result;
        }

        // ******************************************************************
        public void Unregister()
        {
            HotKey hotKey;
            if (_dictHotKeyToCalBackProc.TryGetValue(Id, out hotKey))
            {
                UnregisterHotKey(IntPtr.Zero, Id);
            }
        }

        // ******************************************************************
        private static void ComponentDispatcherThreadFilterMessage(ref MSG msg, ref bool handled)
        {
            if (!handled)
            {
                if (msg.message == WmHotKey)
                {
                    HotKey hotKey;

                    if (_dictHotKeyToCalBackProc.TryGetValue((int)msg.wParam, out hotKey))
                    {
                        if (hotKey.Action != null)
                        {
                            hotKey.Action.Invoke(hotKey);
                        }
                        handled = true;
                    }
                }
            }
        }

        // ******************************************************************
        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // ******************************************************************
        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be _disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be _disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this._disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                    Unregister();
                }

                // Note disposing has been done.
                _disposed = true;
            }
        }
    }
    [Flags]
    public enum KeyModifier
    {
        None = 0x0000,
        Alt = 0x0001,
        Ctrl = 0x0002,
        NoRepeat = 0x4000,
        Shift = 0x0004,
        Win = 0x0008
    }
}
