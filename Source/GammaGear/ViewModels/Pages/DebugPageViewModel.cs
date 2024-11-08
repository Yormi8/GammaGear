using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GammaGear.ViewModels.Pages
{
    public class DebugPageViewModel : ViewModelBase
    {
        private string _logText = "";
        public string LogText
        {
            get => _logText;
            set
            {
                _logText = value;
                OnPropertyChanged(nameof(LogText));
            }
        }

        public DebugPageViewModel()
        {
            for (int i = 0; i < 50; i++)
            {
                _logText += $"This is a line of text that is filling up our stream! This might be line: {i}\n";
            }
        }
    }
}
