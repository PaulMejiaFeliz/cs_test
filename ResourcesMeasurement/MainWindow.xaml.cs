using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ApiClient;

namespace ResourcesMeasurement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SynchronizationContext _sc;
        public ResourceMeasure Resource { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _sc = SynchronizationContext.Current;
        }

        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(async() =>
            {
                try
                {
                    await ApiClient.ApiClient.RunAsync();
                    var leads = await ApiClient.ApiClient.GetLeadsAsync("v1/leads");
                    ShowApiInfo(leads);
                }
                catch(Exception)
                {

                }
            });
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Resource = new ResourceMeasure();
            await Task.Run(() =>
            {
                while (true)
                {
                    var cpus = Resource.MeasureCpu();
                    ShowCpuInfo(cpus);

                    var ram = Resource.MeasureRam();
                    ShowRamInfo(ram);

                    Thread.Sleep(5000);
                }
            });
        }

        private void ShowCpuInfo(List<CpuInformation> cpus)
        {
            _sc.Post(new SendOrPostCallback(o =>
            {
                TxtCpu.Text = ((List<CpuInformation>) o).FirstOrDefault(c => c.Name == "_Total").Usage;
                DgCpus.ItemsSource = ((List<CpuInformation>)o).Where(c => c.Name != "_Total");
            }), cpus);
        }

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

        private void ShowApiInfo(List<Lead> leads)
        {
            _sc.Post(new SendOrPostCallback(o =>
            {
                DgJson.ItemsSource = ((List<Lead>)o);
            }), leads);
        }
    }
}
