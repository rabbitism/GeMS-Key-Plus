using GeMS_Key_Plus.Models;
using GeMS_Key_Plus.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GeMS_Key_Plus.ViewModels
{
    public class ButtonPanelViewModel : BindableBase
    {
        private ObservableCollection<CategoryViewModel> _categories;
        private List<LinkButton> _buttons;
        private Dictionary<Key, LinkButtonViewModel> _keyMapping;

        public List<LinkButton> Buttons { 
            get => _buttons;
            set {
                _buttons = value;
                IntializeButtons();
                RaisePropertyChanged(nameof(Categories));
            }
        }
        public ObservableCollection<CategoryViewModel> Categories {
            get => _categories;
            set {
                _categories = value;
                RaisePropertyChanged(nameof(Categories));
            }
        }
        public ButtonPanelViewModel()
        {
            Categories = new ObservableCollection<CategoryViewModel>();
            Buttons = new List<LinkButton>();
            _keyMapping = new Dictionary<Key, LinkButtonViewModel>();
        }

        private void IntializeButtons()
        {
            Dictionary<string, List<LinkButton>> dict = new Dictionary<string, List<LinkButton>>();
            Categories.Clear();
            foreach (LinkButton button in _buttons)
            {
                if (!dict.ContainsKey(button.Category))
                {
                    dict[button.Category] = new List<LinkButton>();
                }
                dict[button.Category].Add(button);

            }
            foreach(string key in dict.Keys)
            {
                CategoryViewModel category = new CategoryViewModel();
                category.CategoryName = key;
                category.Buttons = new ObservableCollection<LinkButtonViewModel>(dict[key].Select(a => new LinkButtonViewModel(a)));
                Categories.Add(category);
                foreach(LinkButtonViewModel button in category.Buttons)
                {
                    if (Enum.TryParse<Key>(button.HotKey, out Key hotkey))
                    {
                        _keyMapping[hotkey] = button;
                    }
                }
            }
        }

        public void Query(KeyEventArgs args)
        {
            if (_keyMapping.ContainsKey(args.Key))
            {
                _keyMapping[args.Key].Query();
            }
        }
    }
}
