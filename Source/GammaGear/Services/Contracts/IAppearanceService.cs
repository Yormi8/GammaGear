using GammaGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.Services.Contracts
{
    public interface IAppearanceService
    {
        public void SetTheme(ApplicationTheme theme);
        public ApplicationTheme GetTheme();
    }
}
