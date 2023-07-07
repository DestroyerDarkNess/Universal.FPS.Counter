using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderSpy.Universal.FPS
{
    public class FramesPerSecond
    {
        int _frames = 0;
        int _lastTickCount = 0;
        float _lastFrameRate = 0;

        public FramesPerSecond() { }

        // update frames
        public void Frame()
        {
            _frames++;
            if (Math.Abs(Environment.TickCount - _lastTickCount) > 1000)
            {
                _lastFrameRate = (float)_frames * 1000 / Math.Abs(Environment.TickCount - _lastTickCount);
                _lastTickCount = Environment.TickCount;
                _frames = 0;
            }
        }

        public float GetFPS()
        {
            return _lastFrameRate;
        }

    }
}
