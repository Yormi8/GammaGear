using GammaGear.Extensions;
using GammaItems;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace GammaGear.ViewModels
{
    public class ItemLoadoutViewModel : ViewModelBase
    {
        private readonly ItemLoadout _loadout;

        public string Name => _loadout.Name;
        public string Description => _loadout.Description;
        public string Creator => _loadout.Creator;
        public string TimeCreated => _loadout.TimeCreated.ToString("g");
        public string TimeUpdated => _loadout.TimeUpdated.ToString("g");
        public ReadOnlyCollection<Item> Items => _loadout.Items.AsReadOnly();
        public string WizardName => _loadout.WizardName;
        public int WizardLevel => _loadout.WizardLevel;
        public Uri WizardSchoolIcon => _loadout.WizardSchool.ToIconUri();
        public School WizardSchool => _loadout.WizardSchool;
        public ArenaRank WizardPvpRank => _loadout.WizardPvpRank;
        public ArenaRank WizardPetRank => _loadout.WizardPetRank;
        public bool IsLegal => _loadout.IsLegal;
        public bool ContainsRetired => _loadout.ContainsRetired;
        public bool ContainsDebug => _loadout.ContainsDebug;

        public ICommand EditLoadoutCommand { get; }

        public ItemLoadoutViewModel(ItemLoadout loadout)
        {
            _loadout = loadout;
        }
    }
}
