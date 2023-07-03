using pong.GameObjects;
using pong.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

namespace pong
{
    internal class BallManager
    {
        private Ball _ball;
        private Config _config;
        private (double X, double Y) _direction;
        private IHitboxedGameObject[] _colliables;

        public BallManager(Ball ball, Config config, IHitboxedGameObject[] colliables)
        {
            _ball = ball;
            _config = config;
            _direction = (1, 1);
            _colliables = colliables;
        }

        public void RenderNext()
        {
            if (WillOverflow(out Side[] overflowingSides))
            {
                foreach (Side side in overflowingSides)
                {
                    switch (side)
                    {
                        case Side.Left:
                        case Side.Right:
                            _direction.X = InvertDirection(_direction.X);
                            break;

                        case Side.Top:
                        case Side.Bottom:
                            _direction.Y = InvertDirection(_direction.Y);
                            break;
                    }
                }
            }

            foreach (var collidable in _colliables)
            {
                if (Game.ObjectsCollide(_ball.Hitbox, collidable.Hitbox, out Side? objectSide))
                {
                    switch (objectSide)
                    {
                        case Side.Left:
                        case Side.Right:
                            _direction.X = InvertDirection(_direction.X);
                            break;

                        case Side.Top:
                        case Side.Bottom:
                            _direction.Y = InvertDirection(_direction.Y);
                            break;
                    }
                }
            }
            _ball.X += _direction.X;
            _ball.Y += _direction.Y;
        }

        public bool TouchesPlayerSide(out Side? side)
        {
            bool willOverflow = WillOverflow(out Side[] sides);

            if (willOverflow)
            {
                foreach (var s in sides)
                {
                    if (s == Side.Left || s == Side.Right)
                    {
                        side = s;
                        return true;
                    }
                }
            }

            side = null;
            return willOverflow;
        }

        //to prevent ifinitelly bouncing ball between top and bottom side
        private const int safeMinimalBallAngle = 35;
        public void RandomizeDirection()
        {
            // -90 for shift base angle to top side
            double baseAngle = Random.Shared.Next(safeMinimalBallAngle, 360 - safeMinimalBallAngle) - 90;

            //correct angle if do not fullfill safe minimal angle
            if(baseAngle > 90 - safeMinimalBallAngle && baseAngle < 90)
            {
                baseAngle = 90 - safeMinimalBallAngle;
            } else if (baseAngle < 90 + safeMinimalBallAngle && baseAngle > 90) 
            {
                baseAngle = 90 + safeMinimalBallAngle;
            }

            double radian = (baseAngle) * Math.PI / 180;            
            double X = _config.ballSpeed * Math.Cos(radian);
            double Y = _config.ballSpeed * Math.Sin(radian);

            _direction.X = X;
            _direction.Y = Y;
        }

        private double InvertDirection(double direction)
        {
            return direction > 0 ? -Math.Abs(direction) : Math.Abs(direction);
        }

        private bool WillOverflow(out Side[] result)
        {
            List<Side> overflows = new List<Side>();
            if (_direction.X > 0)
            {
                if (_ball.X + _config.ballDiameter + _direction.X > _config.playArea.width) overflows.Add(Side.Right);
            }
            else
            {
                if (_ball.X - 1 < 0) overflows.Add(Side.Left);
            }

            if (_direction.Y > 0)
            {
                if (_ball.Y + _config.ballDiameter + _direction.Y > _config.playArea.height) overflows.Add(Side.Bottom);
            }
            else
            {
                if (_ball.Y - _direction.Y < 0) overflows.Add(Side.Top);
            }

            result = overflows.ToArray();

            return overflows.Count > 0;
        }
    }
}
