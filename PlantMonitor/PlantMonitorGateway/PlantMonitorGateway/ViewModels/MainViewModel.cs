using Maple;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlantMonitorGateway.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public static int HIGH = 1;
        public static int MEDIUM = 2;
        public static int LOW = 3;

        RestClient plantClient;

        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(nameof(IsBusy)); }
        }

        bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set { _isRefreshing = value; OnPropertyChanged(nameof(IsRefreshing)); }
        }

        bool _isEmpty;
        public bool IsEmpty
        {
            get => _isEmpty;
            set { _isEmpty = value; OnPropertyChanged(nameof(IsEmpty)); }
        }

        ServerItem _selectedServer;
        public ServerItem SelectedServer
        {
            get => _selectedServer;
            set { _selectedServer = value; OnPropertyChanged(nameof(SelectedServer)); }
        }

        public ObservableCollection<ServerItem> ServerList { get; set; }
        public ObservableCollection<HumidityModel> LevelList { get; set; }

        public ICommand GetHumidityCommand { private set; get; }
        public ICommand RefreshServersCommand { private set; get; }

        public MainViewModel()
        {
            plantClient = new RestClient();

            LevelList = new ObservableCollection<HumidityModel>();
            ServerList = new ObservableCollection<ServerItem>();

            GetHumidityCommand = new BaseCommand(async () => await GetHumidityCommandExecute());
            RefreshServersCommand = new BaseCommand(async () => await GetServersAsync());

            GetServersAsync();
        }

        async Task GetServersAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            //ServerList.Clear();

            var servers = await plantClient.FindMapleServersAsync();
            foreach (var server in servers)
                ServerList.Add(server);

            if (servers.Count > 0)
            {
                SelectedServer = ServerList[0];
                await GetHumidityCommandExecute();
            }

            IsEmpty = servers.Count == 0;

            IsBusy = false;
        }

        async Task GetHumidityCommandExecute()
        {
            if (SelectedServer == null)
                return;

            //LevelList.Clear();

            var humitidyLogs = await plantClient.GetHumidityAsync(SelectedServer);
            foreach (var log in humitidyLogs)
            {
                int humidity = (int) (log.Humidity * 100);

                LevelList.Insert(0, new HumidityModel()
                {
                    Humidity = humidity,
                    Level = (humidity >= 75) ? HIGH : (humidity >= 50) ? MEDIUM : LOW,
                    Date = DateTime.Now.ToString("hh:mm tt dd/MMM/yyyy")
                });
            }

            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 28/Aug/2018", Humidity = 77, Level = HIGH });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 27/Aug/2018", Humidity = 80, Level = HIGH });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 26/Aug/2018", Humidity = 47, Level = LOW });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 25/Sep/2018", Humidity = 30, Level = LOW });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 24/Aug/2018", Humidity = 64, Level = MEDIUM });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 23/Aug/2018", Humidity = 55, Level = MEDIUM });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 22/Sep/2018", Humidity = 90, Level = HIGH });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 21/Aug/2018", Humidity = 61, Level = MEDIUM });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 20/Aug/2018", Humidity = 57, Level = MEDIUM });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 19/Aug/2018", Humidity = 51, Level = MEDIUM });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 18/Aug/2018", Humidity = 47, Level = LOW });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 17/Aug/2018", Humidity = 42, Level = LOW });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 16/Aug/2018", Humidity = 38, Level = LOW });
            //LevelList.Add(new HumidityModel() { Date = "10:00 AM 15/Sep/2018", Humidity = 30, Level = LOW });

            IsRefreshing = false;
        }

        //string _someText;
        //public string SomeText
        //{
        //    get => _someText;
        //    set { _someText = value; OnPropertyChanged(nameof(SomeText)); }
        //}

        //readonly ObservableCollection<string> _history = new ObservableCollection<string>();
        //public IEnumerable<string> History
        //{
        //    get => _history;
        //}

        //readonly TextConverter _textConverter = new TextConverter(s => s.ToUpper());
        //public ICommand ConvertTextCommand
        //{
        //    get => new BaseCommand(ConvertText);
        //}

        //void ConvertText()
        //{
        //    if (string.IsNullOrWhiteSpace(SomeText))
        //        return;

        //    AddToHistory(_textConverter.ConvertText(SomeText));
        //    SomeText = string.Empty;
        //}

        //void AddToHistory(string item)
        //{
        //    if (!_history.Contains(item))
        //        _history.Add(item);
        //}
    }
}