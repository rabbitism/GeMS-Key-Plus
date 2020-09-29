using GeMS_Key_Plus.Events;
using GeMS_Key_Plus.Global;
using GeMS_Key_Plus.Models;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace GeMS_Key_Plus.ViewModels
{
    public class LinkButtonViewModel: BindableBase
    {
        #region Properties
        private int _id;
        public int Id {
            get { return _id; }
            set { _id = value; RaisePropertyChanged(nameof(Id)); }
        }

        private string _hotkey;
        public string HotKey {
            get { return _hotkey; }
            set { _hotkey = value; RaisePropertyChanged(nameof(HotKey)); }
        }

        private string _name;
        public string ButtonName {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(nameof(ButtonName)); }
        }

        private string _category;
        public string Category {
            get { return _category; }
            set { _category = value; RaisePropertyChanged(nameof(Category)); }
        }

        private string _template;
        public string Template {
            get { return _template; }
            set { _template = value; RaisePropertyChanged(nameof(Template)); }
        }

        private bool _requireSplit;

        public bool RequireSplit {
            get { return _requireSplit; }
            set { _requireSplit = value; RaisePropertyChanged(nameof(RequireSplit)); }
        }

        private bool _isPrimary;
        public bool IsPrimary {
            get { return _isPrimary; }
            set { _isPrimary = value; RaisePropertyChanged(nameof(IsPrimary)); }
        }

        private string _specialDelimiters;
        public string SpecialDelimiters {
            get { return _specialDelimiters; }
            set { _specialDelimiters = value; RaisePropertyChanged(nameof(SpecialDelimiters)); }
        }

        public DelegateCommand QueryCommand { get; set; }
        #endregion

        public LinkButtonViewModel()
        {
            QueryCommand = new DelegateCommand(Query);
        }

        public LinkButtonViewModel(LinkButton button)
        {
            this.ButtonName = button.ButtonName;
            this.Category = button.Category;
            this.HotKey = button.Hotkey;
            this.Id = button.Id;
            this.IsPrimary = button.IsPrimary;
            this.Template = button.Template;
            this.RequireSplit = button.RequireSplit;
            this.SpecialDelimiters = button.SpecialDelimiters;
            QueryCommand = new DelegateCommand(Query);
        }

        public void Query()
        {

            if (string.IsNullOrWhiteSpace(GlobalVariables.QueryString))
            {
                return;
            }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                MultipleQuery();
            }
            else
            {
                var target = Template.Replace("{%s}", GlobalVariables.QueryString);
                var psi = new ProcessStartInfo
                {
                    FileName = target,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            GlobalVariables.ActionHistory.Put(GlobalVariables.QueryString, HotKey);
            EventAggregatorRepository
                .GetInstance()
                .EventAggregator
                .GetEvent<MinimizeWindowEvent>()
                .Publish();
        }

        private void MultipleQuery()
        {
            string[] words = GlobalVariables.QueryString.Split((" \t\n\r" + SpecialDelimiters).ToCharArray());
            foreach(string word in words)
            {
                if (string.IsNullOrWhiteSpace(word)) continue;
                var target = Template.Replace("{%s}", word);
                var psi = new ProcessStartInfo
                {
                    FileName = target,
                    UseShellExecute = true
                };
                Process.Start(psi);
                Thread.Sleep(200);
            }
        }
    }
}
