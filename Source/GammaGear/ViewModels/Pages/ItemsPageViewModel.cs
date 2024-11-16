using GammaGear.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.ViewModels.Pages
{
    public class ItemsPageViewModel : ViewModelBase
    {
        private ObservableCollection<ItemViewModel> _items;
        public ObservableCollection<ItemViewModel> Items => _items;

        public ItemsPageViewModel()
        {
            _items = new ObservableCollection<ItemViewModel>();

            for (int i = 0; i < 100; i++)
            {
                _items.Add(new ItemViewModel(GammaExtensions.GenerateRandomItem()));
            }
        }

        public void OverrideItems(ObservableCollection<ItemViewModel> items)
        {
            _items = items;
            // Update the view
            ItemViewModel update = new(new());
            _items.Add(update);
            _items.Remove(update);
        }
    }
}
