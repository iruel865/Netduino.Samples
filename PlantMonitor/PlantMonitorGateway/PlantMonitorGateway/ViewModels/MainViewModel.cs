using Maple;
using Newtonsoft.Json;
using PlantMonitorGateway.Azure;
using PlantMonitorGateway.IO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
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
        Timer _timer = null;
        TimerCallback _timerCallback = null;

        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set { _isBusy = value; OnPropertyChanged(nameof(IsBusy)); }
        }

        bool _isEmpty;
        public bool IsEmpty
        {
            get => _isEmpty;
            set { _isEmpty = value; OnPropertyChanged(nameof(IsEmpty)); }
        }

        string _status;
        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
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

            MainAsync();
        }

        async Task MainAsync()
        {
            //Status = "Initializing.........";
            //await InitializeAsync();
            //Status += "done";

            Status = "Loading.........";
            await LoadAsync();
            Status += "done";
            await Task.Delay(1000);

            Status = "Getting servers.........";
            await GetServersAsync();
            Status += "done";
            await Task.Delay(1000);

            _timerCallback = new TimerCallback(OnTimerInterrupt);
            _timer = new Timer(_timerCallback, null, TimeSpan.FromTicks(0), new TimeSpan(1, 0, 0));
        }

        async Task InitializeAsync()
        {
            FileManager.Initialize();
            await BlobManager.Initialize();
        }

        async Task SaveAsync()
        {
            FileManager.SaveLog(JsonConvert.SerializeObject(LevelList));
            await BlobManager.UploadLogFileAsync();
        }

        async Task LoadAsync()
        {
            await BlobManager.DownloadLogFileAsync();
            string json = FileManager.ReadFile();

            var list = JsonConvert.DeserializeObject<List<HumidityModel>>(json);
            foreach (var item in list)
                LevelList.Add(item);
        }

        async void OnTimerInterrupt(object state)
        {
            if (SelectedServer != null)
            {
                Status = "Sensing.........";
                await GetHumidityCommandExecute();
                Status += "done";
                await Task.Delay(1000);
            }

            Status ="Saving.........";
            await SaveAsync();
            Status += "done";
            await Task.Delay(1000);

            Status = "Last update: " + DateTime.Now;
        }

        async Task GetServersAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;

            ServerList.Clear();

            var servers = await plantClient.FindMapleServersAsync();
            foreach (var server in servers)
            {
                if (server.Name.Contains("Plant"))
                {
                    ServerList.Add(server);
                }
            }

            if (servers.Count > 0)
                SelectedServer = ServerList[0];

            IsEmpty = servers.Count == 0;

            IsBusy = false;
        }

        async Task GetHumidityCommandExecute()
        {
            if (SelectedServer == null)
                return;

            var humitidyLogs = await plantClient.GetHumidityAsync(SelectedServer).ConfigureAwait(true);

            await App.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                 foreach (var log in humitidyLogs)
                 {
                     int humidity = (int)(log.Humidity * 100);

                     LevelList.Insert(0, new HumidityModel()
                     {
                         Humidity = humidity,
                         Level = (humidity >= 75) ? HIGH : (humidity >= 50) ? MEDIUM : LOW,
                         Date = DateTime.Now.ToString("hh:mm tt dd/MMM/yyyy")
                     });
                 }
             }));
            
        }
    }
}