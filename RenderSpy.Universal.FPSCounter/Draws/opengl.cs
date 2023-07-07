
using System;
using System.Drawing;
using RenderSpy.Graphics.opengl;
using RenderSpy.Rendering.OpenGL;

namespace RenderSpy.Universal.FPS.Draws
{
    public class opengl
    {
        private IntPtr Context;
        private IntPtr hDc;

        private bool IsContextCreated = false;
        private int[] Viewport = new int[4];

        public Font FontData = new Font("Arial", 20);
     
        public void SetDevice(IntPtr hdc)
        {
            hDc = hdc;
        }

        [Obsolete]
        public void DrawText(string msg, System.Drawing.Color FontColor, System.Drawing.Point location)
        {
            try
            {
                // Get Current Context
                IntPtr OldContext = OldSDK.GetCurrentContext();
               
                // Get Old Viewport
                int[] CurrentViewport = new int[4];
                OldSDK.GetInteger(Target.VIEWPORT, CurrentViewport);

                // Initalize OpenGL Context
                if (!IsContextCreated)
                {
                    // Create Context
                    Context = OldSDK.wglCreateContext(hDc);
                    OldSDK.wglMakeCurrent(hDc, Context);

                    // Setup Context for 2D Drawings
                    OldSDK.MatrixMode(Graphics.opengl.MatrixMode.Projection);
                    OldSDK.LoadIdentity();

                    OldSDK.GetInteger(Target.VIEWPORT, Viewport);
                    OldSDK.glOrtho(0.0, Viewport[2], Viewport[3], 0.0, 1.0, -1.0);

                    OldSDK.MatrixMode(Graphics.opengl.MatrixMode.ModelView);
                    OldSDK.ClearColor(0, 0, 0, 1.0f);

                    IsContextCreated = true;
                }
                else
                    OldSDK.wglMakeCurrent(hDc, Context);

                if (Viewport[2] != CurrentViewport[2] && Viewport[3] != CurrentViewport[3])
                {
                    Viewport = CurrentViewport;
                    IsContextCreated = false;
                }

                PointF position = new PointF(location.X, location.Y);

                //Drawings.glRectangle(50, 50, 60, 60, Color.Red); //Draw Rectangle
                //Drawings.glRectangle(60, 60, 40, 40, Color.Blue, true); //Draw Rectangle
                //System.Drawing.Bitmap TextBitmap = Rendering.OpenGL.BitmapFont.DrawTextToBitmap(msg, FontData, FontColor);
                //Drawings.glBitmap(TextBitmap, position); // Draw 2d Bitmap Image

                Drawings.glText(msg,FontData,FontColor, position); // Draw Text

                // Set Current Context to Old One
                OldSDK.wglMakeCurrent(hDc, OldContext);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }



    }
}
