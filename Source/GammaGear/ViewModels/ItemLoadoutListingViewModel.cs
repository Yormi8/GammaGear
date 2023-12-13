using GammaGear.Extensions;
using GammaItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GammaGear.ViewModels
{
    public class ItemLoadoutListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ItemLoadoutViewModel> _itemLoadouts;

        public IEnumerable<ItemLoadoutViewModel> ItemLoadouts => _itemLoadouts;
        public int ItemLoadoutCount => _itemLoadouts.Count;
        public ICommand EditLoadoutCommand { get; }
        public ICommand NewLoadoutCommand { get; }

        public ItemLoadoutListingViewModel()
        {
            _itemLoadouts = new ObservableCollection<ItemLoadoutViewModel>();

            for (int i = 0; i < 100; i++)
            {
                _itemLoadouts.Add(new ItemLoadoutViewModel(GammaExtensions.GenerateRandomLoadout()));
            }
        }
    }
}
