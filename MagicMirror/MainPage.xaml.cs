using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MagicMirror
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer _timer = null;
        public enum WeatherType
        {
            None = 0,
            ClearSky = 01,
            FewClouds = 02,
            ScatteredClouds = 03,
            BrokenClouds = 04,
            ShowerRain = 09,
            Rain = 10,
            Thunderstorm = 11,
            Snow = 13,
            Mist = 50
        }

        public MainPage()
        {
            this.InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;
            _timer.Start();
            UpdateDateAndTime();
            GetWeather();
        }

        private void _timer_Tick(object sender, object e)
        {
            UpdateDateAndTime();
        }

        void UpdateDateAndTime()
        {
            TimeTB.Text = DateTime.Now.ToString("HH:MM:ss");
            DateTB.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
        }

        async void GetWeather()
        {
            CHANGE ME
            WeatherNet.ClientSettings.SetApiKey("");
            var city = 0;


            var current = await WeatherNet.Clients.CurrentWeather.GetByCityIdAsync(city, "EN","metric");



            var week = await WeatherNet.Clients.FiveDaysForecast.GetByCityIdAsync(city);


            CurrentWeatherCC.ContentTemplate = GetWeatherIcon(current.Item.Icon);
            TemperatureTB.Text = current.Item.Temp.ToString();
        }

        WeatherType GetWeatherType(string weather)
        {
            var numstring = weather.Substring(0, 2);
            if(int.TryParse(numstring, out int num))
            {
                return Enum.GetValues(typeof(WeatherType)).OfType<WeatherType>().Where(x => ((int)x) == num).Select(x => x).FirstOrDefault();
            }
            return WeatherType.None;
        }

        DataTemplate GetWeatherIcon(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return null;
            var wt = GetWeatherType(data);
            if (wt == WeatherType.None) return null;
            var iconres = this.Resources["Icon" + Enum.GetName(typeof(WeatherType), wt)];
            if (iconres == null || !(iconres is DataTemplate)) return null;
            return iconres as DataTemplate;
        }

        public static double ConvertCelsiusToFahrenheit(double c)
        {
            return ((9.0 / 5.0) * c) + 32;
        }


        public static double ConvertFahrenheitToCelsius(double f)
        {
            return f;// Math.Round((5.0 / 9.0) * (f - 32),2);
        }
    }
}
