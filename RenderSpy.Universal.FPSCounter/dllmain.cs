
using RenderSpy.Globals;
using System;
using System.Threading;
using System.Windows.Forms;

namespace RenderSpy.Universal.FPS
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InjectionEntryPoint : Attribute
    {
        public bool CreateThread { get; set; } = true;
        public string BuildTarget { get; set; } = ".dll";
        public bool MergeLibs { get; set; } = false;

    }
    public class dllmain
    {

        private static bool Logger = false;
        public static IntPtr GameHandle = IntPtr.Zero;
        public static bool Show = true;

        [InjectionEntryPoint(MergeLibs = true, BuildTarget = ".dll")]
        public static void EntryPoint()
        {
            while (GameHandle.ToInt32() == 0)
            {
                GameHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;  // Get Main Game Window Handle
            }


            if (Logger == true) { RenderSpy.Globals.WinApi.AllocConsole(); }

         
       

            
            
            RenderSpy.Graphics.GraphicsType GraphicsT = RenderSpy.Graphics.Detector.GetCurrentGraphicsType();

            RenderSpy.Interfaces.IHook CurrentHook = null;

            LogConsole("Current Graphics: " + GraphicsT.ToString() + " LIB: " + RenderSpy.Graphics.Detector.GetLibByEnum(GraphicsT));

            switch (GraphicsT)
            {
                case RenderSpy.Graphics.GraphicsType.d3d9:

                    Graphics.d3d9.Present PresentHook_9 = new Graphics.d3d9.Present();
                    PresentHook_9.Install();
                    CurrentHook = PresentHook_9;

                    PresentHook_9.PresentEvent += (IntPtr device, IntPtr sourceRect, IntPtr destRect, IntPtr hDestWindowOverride, IntPtr dirtyRegion) =>
                    {
                        DrawFPS(GraphicsT, device);
                        return PresentHook_9.Present_orig(device, sourceRect, destRect, hDestWindowOverride, dirtyRegion);
                    };

                    break;
                case RenderSpy.Graphics.GraphicsType.d3d10:

                    Graphics.d3d10.Present PresentHook_10 = new Graphics.d3d10.Present();
                    PresentHook_10.Install();
                    CurrentHook = PresentHook_10;

                    PresentHook_10.PresentEvent += (swapChainPtr, syncInterval, flags) =>
                    {
                        DrawFPS(GraphicsT, swapChainPtr);
                        return PresentHook_10.Present_orig(swapChainPtr, syncInterval, flags);
                    };

                    break;
                case RenderSpy.Graphics.GraphicsType.d3d11:

                    Graphics.d3d11.Present PresentHook_11 = new Graphics.d3d11.Present();
                    PresentHook_11.Install();
                    CurrentHook = PresentHook_11;

                    PresentHook_11.PresentEvent += (swapChainPtr, syncInterval, flags) =>
                    {
                        DrawFPS(GraphicsT, swapChainPtr);
                        return PresentHook_11.Present_orig(swapChainPtr, syncInterval, flags);
                    };

                    d3d11_Drawer.ExtractResources();

                    break;
                case RenderSpy.Graphics.GraphicsType.d3d12:

                    break;
                case RenderSpy.Graphics.GraphicsType.opengl:

                    Graphics.opengl.wglSwapBuffers glSwapBuffersHook = new Graphics.opengl.wglSwapBuffers();
                    glSwapBuffersHook.Install();
                    CurrentHook = glSwapBuffersHook;

                    glSwapBuffersHook.wglSwapBuffersEvent += (IntPtr hdc) =>
                    {
                        DrawFPS(GraphicsT, hdc);
                        
                        return glSwapBuffersHook.wglSwapBuffers_orig(hdc); ;
                    };


                    break;
                case RenderSpy.Graphics.GraphicsType.vulkan:

                    break;
                default:

                    break;
            }


            bool Runtime = true;

            while (Runtime)  {
                Thread.Sleep(10);

                int ShowkeyState = WinApi.GetAsyncKeyState(Keys.F2);

                if (ShowkeyState == 1 || ShowkeyState == -32767)
                {
                    Show = !Show;
                }

                int EndkeyState = WinApi.GetAsyncKeyState(Keys.F3);

                if (EndkeyState == 1 || EndkeyState == -32767)
                {
                    Runtime = !Runtime;
                }

            }

            CurrentHook.Uninstall();


        }


        private static FramesPerSecond FrameCounter = new FramesPerSecond();

        private static RenderSpy.Universal.FPS.Draws.d3d9 d3d9_Drawer = new RenderSpy.Universal.FPS.Draws.d3d9();

        private static RenderSpy.Universal.FPS.Draws.d3d10 d3d10_Drawer = new RenderSpy.Universal.FPS.Draws.d3d10();

        private static RenderSpy.Universal.FPS.Draws.d3d11 d3d11_Drawer = new RenderSpy.Universal.FPS.Draws.d3d11();

        private static RenderSpy.Universal.FPS.Draws.opengl opengl_Drawer = new RenderSpy.Universal.FPS.Draws.opengl();

        private static System.Drawing.Color OverlayTextColor = System.Drawing.Color.BlueViolet;

        public static void DrawFPS(RenderSpy.Graphics.GraphicsType GraphicsType, IntPtr TargetGraphicsPtr)
        {
            if (Show == false) { return; }

            FrameCounter.Frame();

            string MessageDraw = String.Format("{0:N0} fps", FrameCounter.GetFPS()).ToUpper();

            switch (GraphicsType)
            {
                case RenderSpy.Graphics.GraphicsType.d3d9:

                    d3d9_Drawer.SetDevice(TargetGraphicsPtr);

                    d3d9_Drawer.DrawText(MessageDraw, OverlayTextColor, new System.Drawing.Point(0,0));

                    break;
                case RenderSpy.Graphics.GraphicsType.d3d10:

                    d3d10_Drawer.SetDevice(TargetGraphicsPtr);

                    d3d10_Drawer.DrawText(MessageDraw, OverlayTextColor, new System.Drawing.Point(0, 0));

                    break;
                case RenderSpy.Graphics.GraphicsType.d3d11:

                    d3d11_Drawer.SetDevice(TargetGraphicsPtr);

                    d3d11_Drawer.DrawText(MessageDraw, OverlayTextColor, new System.Drawing.Point(0, 0));

                    break;
                case RenderSpy.Graphics.GraphicsType.d3d12:

                    break;
                case RenderSpy.Graphics.GraphicsType.opengl:

                    opengl_Drawer.SetDevice(TargetGraphicsPtr);

                    opengl_Drawer.DrawText(MessageDraw, OverlayTextColor, new System.Drawing.Point(0, 0));

                    break;
                case RenderSpy.Graphics.GraphicsType.vulkan:

                    break;
                default:

                    break;
            }
        }

        public static void LogConsole(string msg)   {  if (Logger == true) { Console.WriteLine(msg); } }

    }
}
