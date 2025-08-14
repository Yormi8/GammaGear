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

namespace GammaGear.ViewModels.Controls
{
    public class ItemLoadoutViewModel(ItemLoadout loadout) : ViewModelBase
    {
        public string Name => loadout.Name;
        public string Description => loadout.Description;
        public string Creator => loadout.Creator;
        public string TimeCreated => loadout.TimeCreated.ToString("g");
        public string TimeUpdated => loadout.TimeUpdated.ToString("g");
        public ReadOnlyCollection<Item> Items => loadout.Items.AsReadOnly();
        public string WizardName => loadout.WizardName;
        public int WizardLevel => loadout.WizardLevel;
        public Uri WizardSchoolIcon => loadout.WizardSchool.ToIconUri();
        public School WizardSchool => loadout.WizardSchool;
        public ArenaRank WizardPvpRank => loadout.WizardPvpRank;
        public ArenaRank WizardPetRank => loadout.WizardPetRank;
        public bool IsLegal => loadout.IsLegal;
        public bool ContainsRetired => loadout.ContainsRetired;
        public bool ContainsDebug => loadout.ContainsDebug;

        // TODO: ItemViewModel.CalculatedStats is not very performant
        // Don't really like this but cannot think of another way to have it
        // so that the stats are up to date.
        public ItemViewModel CalculatedStats => new ItemViewModel(loadout.CalculateStats());

        public ICommand EditLoadoutCommand { get; }
    }
}
