﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.ViewModels
{
    public class AboutPageViewModel : ViewModelBase
    {
        public string VersionInfo => ApplicationInfo.AppDisplayVersion;
    }
}
