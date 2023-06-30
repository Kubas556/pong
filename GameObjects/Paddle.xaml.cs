using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pong.GameObjects
{
    /// <summary>
    /// Interaction logic for Paddle.xaml
    /// </summary>
    public partial class Paddle : UserControl, INotifyPropertyChanged, IHitboxedGameObject
    {
        private Brush _fill = Brushes.Brown;
        public Brush Fill { get { return _fill; } set { _fill = value; OnPropertyChanged("Fill"); } }

        private Size _size = new Size(0, 0);
        public Size Size { get { return _size; } set { _size = value; OnPropertyChanged("Size"); UpdateHitbox(); } }

        private Point _location = new Point(0, 0);
        public int X { get { return _location.X; } set { _location.X = value; Canvas.SetLeft(this, value); UpdateHitbox(); } }
        public int Y { get { return _location.Y; } set { _location.Y = value; Canvas.SetTop(this, value); UpdateHitbox(); } }

        private System.Drawing.Rectangle _hitbox;
        public System.Drawing.Rectangle Hitbox { get { return _hitbox; } }

        public Paddle()
        {
            InitializeComponent();

            var myBinding = new Binding("Fill")
            {
                Source = this,
            };
            RectangleFill.SetBinding(Shape.FillProperty, myBinding);

            _hitbox = new System.Drawing.Rectangle(_location, _size);
            //Debug.WriteLine(this.Width);
            //System.Drawing.Rectangle t = new System.Drawing.Rectangle(,);
        }

        private void UpdateHitbox()
        {
            _hitbox.Location = _location;
            _hitbox.Size = _size;
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

        private void PaddleLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _size = new Size((int)this.Width, (int)this.Height);
        }
    }
}
