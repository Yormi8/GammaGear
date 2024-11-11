using GammaGear.Logging;
using System.Collections.ObjectModel;

namespace GammaGear.Services.Contracts
{
    public interface ILogViewService
    {
        ObservableCollection<Log> LogMessages { get; }
    }
}
