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

namespace GeMS_Key_Plus.ViewModels
{
    public class MainViewModel:BindableBase
    {
        public ButtonPanelViewModel ButtonPanel { get; set; } = new ButtonPanelViewModel();

        private string _queryString;
        public string QueryString {
            get { return _queryString; }
            set { 
                _queryString = value;
                GlobalVariables.QueryString = value;
                RaisePropertyChanged(nameof(QueryString));
            }
        }

        public MainViewModel()
        {
            using(ApplicationContext context = new ApplicationContext())
            {
                this.ButtonPanel.Buttons = context.Buttons.ToList();
            }
        }

        public void Query(KeyEventArgs args)
        {
            this.ButtonPanel.Query(args);
        }
        
    }
}
