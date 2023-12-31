﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace pong.GameObjects
{
    public interface IHitboxedGameObject : INotifyPropertyChanged
    {
        public Size Size { get; set; }

        public double X { get; set; }
        public double Y { get; set; }

        public System.Drawing.Rectangle Hitbox { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
