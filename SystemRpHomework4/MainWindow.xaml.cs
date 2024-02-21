using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SystemRpHomework4.Model;

namespace SystemRpHomework4
{

    public partial class MainWindow : Window
    {
        public ObservableCollection<Car> cars { get; set; }
        public DispatcherTimer dispatcherTimer { get; set; }
        public double secund = 0;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            cars = new();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += art;
        }
        public void art(object sender, EventArgs e)
        {
            secund+=1;
            TimerLabel.Content = secund;
        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (secund == 0)
            {
                if (SingelRadioButton.IsChecked == true)
                {
                    cars.Clear();
                    new Thread(() =>
                    {

                        dispatcherTimer.Start();



                        int i = 1;
                        while (File.Exists($"../../../Static/car{i}.json"))
                        {
                            var carsJson = JsonSerializer.Deserialize<List<Car>>(File.ReadAllText($"../../../Static/car{i}.json"));

                            foreach (var car in carsJson)
                            {
                                Dispatcher.Invoke(() => { cars.Add(car); });
                                Thread.Sleep(3);

                            }

                            i++;
                        }

                        dispatcherTimer.Stop();
                        secund = 0;
                    }).Start();


                }
                else if (MultiRadioButton.IsChecked == true)
                {


                    cars.Clear();
                    new Thread(() =>
                    {

                        dispatcherTimer.Start();

                        List<Thread> threads = new();

                        int i = 1;
                        while (File.Exists($"../../../Static/car{i}.json"))
                        {
                            int car_i = i;
                            threads.Add(new Thread(() =>
                            {
                                
                                var carsJson = JsonSerializer.Deserialize<List<Car>>(File.ReadAllText($"../../../Static/car{car_i}.json"));

                                foreach (var car in carsJson)
                                {
                                    Dispatcher.Invoke(() => { cars.Add(car); });
                                    Thread.Sleep(3);

                                }
                            }));
                            i++;
                        }

                        foreach (var th in threads)
                        {
                            th.Start();
                        }
                        foreach (var th in threads)
                        {
                            th.Join();
                        }

                        dispatcherTimer.Stop();
                        secund = 0;
                    }).Start();





                }

            }
            else { MessageBox.Show("isin qutarmaqini gozleyin"); }


        }
    }
}