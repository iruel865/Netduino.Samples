using PlantMonitorGateway.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PlantMonitorGateway.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        string _someText;
        public string SomeText
        {
            get => _someText;
            set { _someText = value; OnPropertyChanged(nameof(SomeText)); }
        }

        readonly ObservableCollection<string> _history = new ObservableCollection<string>();
        public IEnumerable<string> History
        {
            get => _history;
        }

        readonly TextConverter _textConverter = new TextConverter(s => s.ToUpper());
        public ICommand ConvertTextCommand
        {
            get => new BaseCommand(ConvertText);
        }

        void ConvertText()
        {
            if (string.IsNullOrWhiteSpace(SomeText))
                return;

            AddToHistory(_textConverter.ConvertText(SomeText));
            SomeText = string.Empty;
        }

        void AddToHistory(string item)
        {
            if (!_history.Contains(item))
                _history.Add(item);
        }
    }
}
