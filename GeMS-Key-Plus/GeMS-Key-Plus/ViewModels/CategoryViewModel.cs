using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeMS_Key_Plus.ViewModels
{
    public class CategoryViewModel:BindableBase
    {
        private string _categoryName;

        public string CategoryName {
            get { return _categoryName; }
            set { _categoryName = value; RaisePropertyChanged(nameof(CategoryName)); }
        }

        private ObservableCollection<LinkButtonViewModel> _buttons;

        public ObservableCollection<LinkButtonViewModel> Buttons {
            get { return _buttons; }
            set { _buttons = value; RaisePropertyChanged(nameof(Buttons)); }
        }


    }
}
