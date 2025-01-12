using CommunityToolkit.Mvvm.Input;
using GammaGear.Extensions;
using Microsoft.Extensions.Logging;
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
        public ItemViewModel ComparedItem { get; private set; } = null;
        public ItemViewModel SelectedItem { get; private set; } = null;
        private bool IsComparisonEnabled = false;

        private IRelayCommand _compareSelectionChangedCommand;
        public IRelayCommand CompareSelectionChangedCommand => _compareSelectionChangedCommand ??= new RelayCommand<bool>(CompareChanged);

        private IRelayCommand _compareUpdate;
        public IRelayCommand CompareUpdate => _compareUpdate ??= new RelayCommand(OnCompareUpdate);

        private ILogger _logger;

        public ItemsPageViewModel(
            ILogger<HomePageViewModel> logger)
        {
            _logger = logger;
            _items = new ObservableCollection<ItemViewModel>();

            for (int i = 0; i < 100; i++)
            {
                _items.Add(new ItemViewModel(GammaExtensions.GenerateRandomItem()));
            }
        }

        public void OverrideItems(ObservableCollection<ItemViewModel> items)
        {
            _items.Clear();
            foreach (ItemViewModel item in items)
            {
                _items.Add(item);
            }
        }

        private void CompareChanged(bool currentState)
        {
            IsComparisonEnabled = currentState;
            if (currentState)
            {
                // Swapping from false to true
                _logger.LogDebug("Compare switch toggled to true");

                // Get currently selected item
                ComparedItem = SelectedItem;
                OnPropertyChanged(nameof(ComparedItem));
            }
            else
            {
                // Swapping from true to false
                _logger.LogDebug("Compare switch toggled to false");

                ComparedItem = null;
                OnPropertyChanged(nameof(ComparedItem));
            }
        }

        private void OnCompareUpdate()
        {
            ComparedItem = SelectedItem;
            OnPropertyChanged(nameof(ComparedItem));
        }

        public void SelectedItemChanged(ItemViewModel newItem)
        {
            SelectedItem = newItem;
            OnPropertyChanged(nameof(SelectedItem));
        }
    }
}
