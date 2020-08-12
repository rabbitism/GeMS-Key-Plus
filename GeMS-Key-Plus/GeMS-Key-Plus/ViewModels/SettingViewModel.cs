using GeMS_Key_Plus.Data;
using GeMS_Key_Plus.Global;
using GeMS_Key_Plus.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GeMS_Key_Plus.ViewModels
{
    public class SettingViewModel : BindableBase
    {
        private LinkButtonViewModel _newButton = new LinkButtonViewModel();

        public LinkButtonViewModel NewButton { 
            get => _newButton;
            set { 
                _newButton = value;
                RaisePropertyChanged(nameof(NewButton));
            } 
        }

        private ObservableCollection<string> _availableKeys;

        public ObservableCollection<string> AvailableKeys {
            get { return _availableKeys; }
            set { _availableKeys = value; RaisePropertyChanged(nameof(AvailableKeys)); }
        }

        private ObservableCollection<string> _availableCategories;

        public ObservableCollection<string> AvailableCategories {
            get { return _availableCategories; }
            set { _availableCategories = value; RaisePropertyChanged(nameof(AvailableCategories)); }
        }



        private ObservableCollection<LinkButton> _buttons;

        public ObservableCollection<LinkButton> Buttons {
            get { return _buttons; }
            set { _buttons = value; RaisePropertyChanged(nameof(Buttons)); }
        }

        public DelegateCommand SubmitCommand { get; set; }

        public SettingViewModel()
        {
            ReloadButtons();
            SubmitCommand = new DelegateCommand(SubmitNewButton);
        }

        private void ReloadButtons()
        {
            HashSet<string> AllKeys = new HashSet<string>(GlobalVariables.AllHotKeys);
            HashSet<string> AllCategories = new HashSet<string>();
            using (ApplicationContext context = new ApplicationContext())
            {
                Buttons = new ObservableCollection<LinkButton>(context.Buttons.ToList());
                List<string> dominatedHotkeys = Buttons.Select(a => a.Hotkey).ToList();
                foreach (string k in dominatedHotkeys)
                {
                    AllKeys.Remove(k);
                }
                foreach(var b in Buttons)
                {
                    AllCategories.Add(b.Category);
                }
            }
            this.AvailableKeys = new ObservableCollection<string>(AllKeys);
            this.AvailableCategories = new ObservableCollection<string>(AllCategories);
        }

        private void SubmitNewButton()
        {
            LinkButton button = new LinkButton()
            {
                SpecialDelimiters = NewButton.SpecialDelimiters,
                Suffix = NewButton.Suffix,
                RequireSplit = NewButton.RequireSplit,
                ButtonName = NewButton.ButtonName,
                Category = NewButton.Category,
                Hotkey = NewButton.HotKey,
                IsPrimary = NewButton.IsPrimary,
                Prefix = NewButton.Prefix,
            };
            using(ApplicationContext context = new ApplicationContext())
            {
                context.Buttons.Add(button);
                context.SaveChanges();
            }
            ReloadButtons();
            this.NewButton = new LinkButtonViewModel();
        }
    }
}
