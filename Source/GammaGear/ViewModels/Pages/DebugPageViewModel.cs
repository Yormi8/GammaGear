using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GammaGear.ViewModels.Pages
{
    public class DebugPageViewModel : ViewModelBase
    {
        public string LogText
        {
            get
            {
                TextReader r = new StreamReader(_consoleStream);
                return r.ReadToEnd();
            }
        }
        private ILogger _logger;
        private System.IO.MemoryStream _consoleStream;
        private TextWriter _consoleWriter;

        public DebugPageViewModel(ILogger<DebugPageViewModel> logger)
        {
            _logger = logger;
            _consoleStream = new MemoryStream();
            _consoleWriter = new StreamWriter(_consoleStream);
            Console.SetOut(_consoleWriter);


            for (int i = 0; i < 50; i++)
            {
                //_logText += $"This is a line of text that is filling up our stream! This might be line: {i}\n";
            }

            TextBlock te = new TextBlock();

        }

        public void UpdateLogText()
        {
            OnPropertyChanged(nameof(LogText));
        }
    }
}
