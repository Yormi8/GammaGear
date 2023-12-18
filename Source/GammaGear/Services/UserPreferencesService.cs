using GammaGear.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.Services
{
    public class UserPreferencesService
    {
        public ApplicationTheme Theme
        {
            get => (ApplicationTheme)Properties.UserPrefs.Default.Theme;
            set
            {
                if (value != (ApplicationTheme)Properties.UserPrefs.Default.Theme)
                {
                    Properties.UserPrefs.Default.Theme = (int)value;
                    Properties.UserPrefs.Default.Save();
                }
            }
        }
    }
}
