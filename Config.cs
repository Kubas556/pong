using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pong
{
    public class Config
    {
        public int paddleSizeX, paddleSizeY;

        public int paddleSpeed;

        public int ballDiameter;

        public (int width, int height) playArea;
    }
}
