using SharpDX.Direct3D10;
using SharpDX.Mathematics.Interop;
using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RenderSpy.Universal.FPS.Draws
{
    public class d3d10
    {

        SharpDX.DXGI.SwapChain swapChain;


        public void SetDevice(IntPtr swapChainPtr)
        {
            swapChain = (SharpDX.DXGI.SwapChain)swapChainPtr;
        }

        public void DrawText(string msg, System.Drawing.Color FontColor, System.Drawing.Point location)
        {
            try
            {
                using (Texture2D texture = Texture2D.FromSwapChain<SharpDX.Direct3D10.Texture2D>(swapChain, 0))
                {
                   
                        FontDescription fd = new SharpDX.Direct3D10.FontDescription()
                        {
                            Height = 20,
                            FaceName = "Arial",
                            Italic = false,
                            Width = 0,
                            MipLevels = 1,
                            CharacterSet = SharpDX.Direct3D10.FontCharacterSet.Default,
                            OutputPrecision = SharpDX.Direct3D10.FontPrecision.Default,
                            Quality = SharpDX.Direct3D10.FontQuality.ClearTypeNatural,
                            PitchAndFamily = FontPitchAndFamily.Default | FontPitchAndFamily.DontCare,
                            Weight = FontWeight.Bold
                        };

                        // TODO: Font should not be created every frame!
                        using (SharpDX.Direct3D10.Font font = new SharpDX.Direct3D10.Font(texture.Device, fd))
                        {
                          font.DrawText(null, msg, new RawRectangle((int)location.X, 0, (int)location.Y, 0), SharpDX.Direct3D10.FontDrawFlags.NoClip, new RawColor4(FontColor.R, FontColor.G, FontColor.B, FontColor.A));
                        }
                }
            }
            catch 
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
        }

    }
}
