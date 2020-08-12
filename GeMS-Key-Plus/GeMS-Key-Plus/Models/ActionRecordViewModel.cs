using Prism.Mvvm;
using System.Windows.Input;

namespace GeMS_Key_Plus.Models
{
    public class ActionRecordViewModel : BindableBase
    {
        private string _query;

        public string Query {
            get => _query; 
            set {
                _query = value;
                RaisePropertyChanged(nameof(Query));
            }
        }

        private string _hotkey;

        public string Hotkey {
            get => _hotkey;
            set { 
                _hotkey = value;
                RaisePropertyChanged(nameof(Hotkey));
            }
        }

    }
}
