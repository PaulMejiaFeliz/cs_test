using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ResourcesMeasurement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SynchronizationContext _sc;
        private CancellationTokenSource _cancelationToken;
        public ResourceMeasure Resource { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _sc = SynchronizationContext.Current;
        }

        //Request the API information in a new thread
        private async void BtnGetApiInfoClick(object sender, RoutedEventArgs e)
        {
            await Task.Run(async () =>
            {
                try
                {
                    ChangeRequestBtnState(false);
                    var client = new ApiClient.ApiClient("http://apidev.gewaer.io/");
                    var leads = await client.GetAsync("v1/leads");
                    ShowApiInfo(leads);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, "Error");
                }
            });
        }

        //Enables or disables the BtnGetApiInfo button
        private void ChangeRequestBtnState(bool state)
        {
            _sc.Post(new SendOrPostCallback(o =>
            {
                BtnGetApiInfo.IsEnabled = (bool) o;
            }), state);
        }

        //Starts a new thead that get the CUP and RAM infformation
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Resource = new ResourceMeasure();
            _cancelationToken = new CancellationTokenSource();
            await Task.Run(() =>
            {
                while (!_cancelationToken.IsCancellationRequested)
                {
                    var cpus = Resource.MeasureCpu();
                    ShowCpuInfo(cpus);

                    var ram = Resource.MeasureRam();
                    ShowRamInfo(ram);

                    Thread.Sleep(5000);
                }
            }, _cancelationToken.Token);
        }
        
        //Updates the CPU UI controls
        private void ShowCpuInfo(List<CpuInformation> cpus)
        {
            _sc.Post(new SendOrPostCallback(o =>
            {
                TxtCpu.Text = ((List<CpuInformation>)o).FirstOrDefault(c => c.Name == "_Total").Usage;
                DgCpus.ItemsSource = ((List<CpuInformation>)o).Where(c => c.Name != "_Total");
            }), cpus);
        }

        //Updates the RAM UI controls
        private void ShowRamInfo(RamInformation ram)
        {
            _sc.Post(new SendOrPostCallback(o =>
            {
                var ramInfo = (RamInformation)o;

                TxtTotalRam.Text = ramInfo.Total.ToString();
                TxtFreeRam.Text = ramInfo.Free.ToString();
                TxtPercentUsedRam.Text = ramInfo.Used.ToString();
            }), ram);
        }

        //Updates the API UI Data Grid
        private void ShowApiInfo(object leads)
        {
            _sc.Post(new SendOrPostCallback(o =>
            {
                DgJson.ItemsSource = (IEnumerable<object>)o;
            }), leads);
        }

        //If the token exists Request a cancelation
        private void StopThread(object sender, RoutedEventArgs e)
        {
            if (_cancelationToken != null)
            {
                _cancelationToken.Cancel();
            }
        }
    }
}
