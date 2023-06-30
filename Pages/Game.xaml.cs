using pong.GameObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace pong.Pages
{
    public enum Side
    {
        Left, Top, Right, Bottom
    }
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Page, INotifyPropertyChanged
    {
        private Config _config;

        private Paddle _leftPaddle, _rightPaddle;
        private Ball _ball;

        private ControlsManager _controlsManager;
        private BallManager _ballManager;

        private int _score1, _score2;
        public int Score1 { get { return _score1; } private set { _score1 = value; OnPropertyChanged(nameof(Score1)); } }
        public int Score2 { get { return _score2; } private set { _score2 = value; OnPropertyChanged(nameof(Score2)); } }

        public Game(MainWindow window, Config config)
        {
            InitializeComponent();
            window.KeyDown += OnKeyDown;
            window.KeyUp += OnKeyUp;

            _config = config;

            _leftPaddle = new Paddle() { Width = _config.paddleSizeX, Height = _config.paddleSizeY, X = 0, Y = 0 };
            _rightPaddle = new Paddle() { Width = _config.paddleSizeX, Height = _config.paddleSizeY, X = 0, Y = 0 };
            //addToCanvas(_leftPaddle);
            //addToCanvas(_rightPaddle);

            _ball = new Ball(config.ballDiameter) { X = 0, Y = 0};
            addToCanvas(_ball);

            _controlsManager = new ControlsManager(window, _config, _leftPaddle, _rightPaddle);
            _ballManager = new BallManager(_ball, _config, new IHitboxedGameObject[] { _leftPaddle, _rightPaddle });
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            if(_ballManager.TouchesSide(out Side? side))
            {
                switch(side) 
                {
                    case Side.Left:
                        Score1++;
                        break;
                    case Side.Right:
                        Score2++;
                        break;
                }
            }

            _controlsManager.RenderNext();
            _ballManager.RenderNext();
        }

        public static bool ObjectsCollide(System.Drawing.Rectangle a, System.Drawing.Rectangle b, out Side? side) 
        {
            var result = System.Drawing.Rectangle.Intersect(a, b);
            /*var rect = new System.Windows.Shapes.Rectangle() { Width = result.Width, Height = result.Height, Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)Random.Next(0,255), (byte)Random.Next(0, 255), (byte)Random.Next(0, 255))) };
            Canvas.SetLeft(rect, result.X);
            Canvas.SetTop(rect, result.Y);
            test.Children.Add(rect);*/

            /*if (result.Width > 0 && result.Height > 0)
            {*/
                /*double bCenterX = b.X + (b.Width / 2);
                double bCenterY = b.Y + (b.Height / 2);*/

                var dx = (b.X + b.Width / 2) - (a.X + a.Width / 2);
                var dy = (b.Y + b.Height / 2) - (a.Y + a.Height / 2);
                var width = (b.Width + a.Width) / 2;
                var height = (b.Height + a.Height) / 2;
                var crossWidth = width * dy;
                var crossHeight = height * dx;

                if (Math.Abs(dx) <= width && Math.Abs(dy) <= height)
                {
                    if (crossWidth > crossHeight)
                    {
                        side = (crossWidth > (-crossHeight)) ? Side.Bottom : Side.Left;
                        return true;
                    }
                    else
                    {
                        side = (crossWidth > -(crossHeight)) ? Side.Right : Side.Top;
                        return true;
                    }
                }


                /*Debug.WriteLine("collision");
                if (result.X > bCenterX)
                    side = Side.Right;
                else if (result.X < bCenterX)
                    side = Side.Left;
                else if (result.Y > bCenterY)
                    side = Side.Bottom;
                else if (result.Y < bCenterY)
                    side = Side.Top;
                else throw new Exception("edge case");

                return true;*/
                
            //}

            side = null;
            return false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }

        private void addToCanvas(UIElement element)
        {
            PlayingCanvas.Children.Add(element);
        }

        private void CanvasLoaded(object sender, EventArgs e)
        {
            _config.playArea = ((int)PlayingCanvas.ActualWidth, (int)PlayingCanvas.ActualHeight);
            PositionGameObjects();
            CompositionTarget.Rendering += GameLoop;
        }

        private void CanvasSizeChanged(object sender, SizeChangedEventArgs e)
        {
            _config.playArea = ((int)PlayingCanvas.ActualWidth, (int)PlayingCanvas.ActualHeight);
            PositionGameObjects();
        }

        private void PositionGameObjects()
        {
            _ball.X = _config.playArea.width / 2;
            _ball.Y = _config.playArea.height / 2;

            _leftPaddle.Y = _rightPaddle.Y = (_config.playArea.height / 2) - (_config.paddleSizeY / 2);

            _rightPaddle.X = _config.playArea.width - _config.paddleSizeX;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
        }
    }
}
