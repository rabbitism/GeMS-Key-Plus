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
        }

        public void Query(KeyEventArgs args)
        {
            this.ButtonPanel.Query(args);
        }
        
    }
}
