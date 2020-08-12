using GeMS_Key_Plus.Data;
using GeMS_Key_Plus.Global;
using GeMS_Key_Plus.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Controls;
using Prism.Commands;

namespace GeMS_Key_Plus.ViewModels
{
    public class MainViewModel:BindableBase
    {
        public ButtonPanelViewModel ButtonPanel { get; set; } = new ButtonPanelViewModel();
        public DelegateCommand ReloadActionHistoryCommand { get; set; }
        public DelegateCommand<ActionRecordViewModel> QueryHistoryCommand { get; set; }
        public DelegateCommand ReloadButtonsCommand { get; set; }
        public DelegateCommand ClearCacheCommand { get; set; }

        private string _queryString;
        public string QueryString {
            get { return _queryString; }
            set { 
                _queryString = value;
                GlobalVariables.QueryString = value;
                RaisePropertyChanged(nameof(QueryString));
            }
        }

        private ObservableCollection<ActionRecordViewModel> _actionHistory;

        public ObservableCollection<ActionRecordViewModel> ActionHistory {
            get => _actionHistory; 
            set { 
                _actionHistory = value;
                RaisePropertyChanged(nameof(ActionHistory));
            }
        }

        private ActionRecordViewModel _selectedActionRecord;

        public ActionRecordViewModel SelectedActionRecord {
            get => _selectedActionRecord; 
            set { 
                _selectedActionRecord = value;
                if(value != null)
                {
                    this.QueryString = value.Query;
                }
                RaisePropertyChanged(nameof(SelectedActionRecord));
            }
        }


        public MainViewModel()
        {
            using(ApplicationContext context = new ApplicationContext())
            {
                if (context.Buttons.Count() == 0)
                {
                    Debug.WriteLine("No Button in database");
                    context.Buttons.Add(new LinkButton()
                    {
                        SpecialDelimiters = "",
                        Suffix = "",
                        Prefix = "https://www.google.com/search?q=",
                        RequireSplit=  false,
                        ButtonName = "Google",
                        Category = "Search",
                        Hotkey = "G",
                        IsPrimary = true,
                    });
                    context.Buttons.Add(new LinkButton()
                    {
                        SpecialDelimiters = "",
                        Suffix = "",
                        Prefix = "https://www.youdao.com/w/eng/",
                        RequireSplit = false,
                        ButtonName = "Youdao",
                        Category = "Search",
                        Hotkey = "Y",
                        IsPrimary = false,
                    });
                    context.SaveChanges();
                }
                this.ButtonPanel.Buttons = context.Buttons.ToList();
            }
            ReloadActionHistoryCommand = new DelegateCommand(ReloadActionHistory);
            QueryHistoryCommand = new DelegateCommand<ActionRecordViewModel>(QueryHistory);
            ReloadButtonsCommand = new DelegateCommand(ReloadButtons);
            ClearCacheCommand = new DelegateCommand(ClearCache);
        }

        public void Query(KeyEventArgs args)
        {
            this.ButtonPanel.Query(args.Key);
        }

        private void ReloadButtons()
        {
            using(ApplicationContext context = new ApplicationContext())
            {
                this.ButtonPanel.Buttons = context.Buttons.ToList();
            }
        }

        private void ReloadActionHistory()
        {
            this.ActionHistory = new ObservableCollection<ActionRecordViewModel>(
                GlobalVariables.ActionHistory.GetAll().Select(a => new ActionRecordViewModel() { Query = a.Item1, Hotkey = a.Item2 })
                );
        }

        private void QueryHistory(ActionRecordViewModel record)
        {
            if(Enum.TryParse<Key>(record.Hotkey, out Key key))
            {
                this.QueryString = record.Query;
                this.ButtonPanel.Query(key);
            }
        }

        private void ClearCache()
        {
            GlobalVariables.ActionHistory.Clear();
            ReloadActionHistory();
        }
        
    }
}
