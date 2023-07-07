using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenderSpy.Universal.FPS.Draws
{
    public class d3d9
    {
        Device device;


        public void SetDevice(IntPtr DevicePtr) {
            device = (Device)DevicePtr;
        }

        public void DrawText(string msg, System.Drawing.Color FontColor, System.Drawing.Point location) {
            try
            {
                using (SharpDX.Direct3D9.Font font = new SharpDX.Direct3D9.Font(device, new FontDescription()
                {
                    Height = 20,
                    FaceName = "Arial",
                    Italic = false,
                    Width = 0,
                    MipLevels = 1,
                    CharacterSet = FontCharacterSet.Default,
                    OutputPrecision = FontPrecision.Default,
                    Quality = FontQuality.ClearTypeNatural,
                    PitchAndFamily = FontPitchAndFamily.Default | FontPitchAndFamily.DontCare,
                    Weight = FontWeight.Bold
                }))
                {
                    font.DrawText(null, msg, location.X, location.Y, new SharpDX.Mathematics.Interop.RawColorBGRA(FontColor.B, FontColor.G, FontColor.R, FontColor.A));
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }
        }


    }
}
