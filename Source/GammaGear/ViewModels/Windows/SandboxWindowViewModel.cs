using GammaGear.Extensions;
using GammaItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.ViewModels.Windows
{
    public partial class SandboxWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ItemViewModel> Items { get; set; }

        public SandboxWindowViewModel()
        {
            Items = new ObservableCollection<ItemViewModel>();

            for (int i = 0; i < 150; i++)
            {
                Items.Add(new ItemViewModel(GammaExtensions.GenerateRandomItem()));
            }
        }
    }
}
