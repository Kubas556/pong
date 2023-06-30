using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using pong.GameObjects;
using pong.Pages;

namespace pong
{
    internal class ControlsManager
    {
        private Paddle _leftPaddle, _rightPaddle;

        private (int left, int right) _paddleDirection;

        private Config _config;

        public ControlsManager(MainWindow window, Config config, Paddle leftPaddle, Paddle rightPaddle) {
            _config = config;

            _leftPaddle = leftPaddle;
            _rightPaddle = rightPaddle;

            window.KeyDown += onDown;
            window.KeyUp += onUp;
        }

        public void RenderNext()
        {
            if (PaddleCanMove(_leftPaddle, _paddleDirection.left))
            {
                _leftPaddle.Y += _paddleDirection.left;
            }

            if (PaddleCanMove(_rightPaddle, _paddleDirection.right))
            {
                _rightPaddle.Y += _paddleDirection.right;
            }
        }

        private bool PaddleCanMove(Paddle paddle, int direction) 
        {
            return paddle.Y + direction >= 0 && paddle.Y + direction + _config.paddleSizeY <= _config.playArea.height;
        }

        private void onDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                        _paddleDirection.right = -_config.paddleSpeed;
                        break;
                case Key.Down:
                        _paddleDirection.right = _config.paddleSpeed;
                    break;

                case Key.W:
                    _paddleDirection.left = -_config.paddleSpeed;
                    break;
                case Key.S:
                    _paddleDirection.left = _config.paddleSpeed;
                    break;
            }
        }

        private void onUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                case Key.Down:
                    if (_paddleDirection.right != 0 && (Keyboard.IsKeyUp(Key.Up) && Keyboard.IsKeyUp(Key.Down))) _paddleDirection.right = 0;
                    break;
                case Key.W:
                case Key.S:
                    if (_paddleDirection.left != 0 && (Keyboard.IsKeyUp(Key.W) && Keyboard.IsKeyUp(Key.S))) _paddleDirection.left = 0;
                    break;
            }
        }
    }
}
