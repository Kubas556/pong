using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using pong.Pages;

namespace pong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Page? _page;

        public Page? Page
        {
            get { return _page; }
            set { 
                _page = value;
                OnPropertyChanged("Page");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        private void StartClicked(object sender, RoutedEventArgs e)
        {
            //((Button)sender).Visibility = Visibility.Hidden;

            Config config = new Config()
            {
                paddleSizeX = 10,
                paddleSizeY = 50,
                paddleSpeed = 2,
                ballDiameter = 10,
                paddleOffset = 50,
                ballSpeed = 2,
                randomizeAfterPoint = true
            };
            Game game = new Game(this, config);
            Page = game;
        }

        private void WindowLoaded(object sender, EventArgs e)
        {
        }
    }
}
