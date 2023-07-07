using System;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct3D11;


namespace RenderSpy.Universal.FPS.Draws
{
    public class d3d11
    {
        public d3d11() {
          
        }

        SharpDX.DXGI.SwapChain3 swapChain;
        public System.Drawing.Font FontData = new System.Drawing.Font("Arial", 20);


        public void SetDevice(IntPtr SwapChainPtr)
        {
            swapChain = (SharpDX.DXGI.SwapChain3)SwapChainPtr;
        }

         SharpDX.Direct3D11.Device device;
         DeviceContext _deviceContext;
        Texture2D _renderTarget;
        RenderTargetView _renderTargetView;

        SpriteTextRenderer.SharpDX.SpriteRenderer sprite;
        SpriteTextRenderer.SharpDX.TextBlockRenderer textBlock;

        public void ExtractResources() {
            try
            {
                RenderSpy.Globals.SharpDXSprite SpriteLib = RenderSpy.Globals.Helpers.GetSharpDXSprite(RenderSpy.Globals.ArchSpriteLib.auto);
                if (System.IO.File.Exists(SpriteLib.LibName) == false) { System.IO.File.WriteAllBytes(SpriteLib.LibName, SpriteLib.LibBytes); }
            }
            finally { }
        }

        public void DrawText(string msg, System.Drawing.Color FontColor, System.Drawing.Point location)
        {
            if (!init)
            {
                InitText(swapChain);
            }

            try
            {
                int width = swapChain.Description.ModeDescription.Width;
                int height = swapChain.Description.ModeDescription.Height;
                _deviceContext.ClearRenderTargetView(_renderTargetView, Color.DarkBlue);

                //textBlock.DrawString("ABCDEFGHIJKLMNOPQRSTUVWXYZ" + Environment.NewLine + "abcdefghijklmnopqrstuvwxyz", Vector2.Zero, new Color4(1.0f, 1.0f, 0.0f, 1.0f));
                //textBlock.DrawString("SDX SpriteTextRenderer sample" + Environment.NewLine + "(using SharpDX)", new System.Drawing.RectangleF(0, 0, width, height),
                //SpriteTextRenderer.TextAlignment.Right | SpriteTextRenderer.TextAlignment.Bottom, new Color4(1.0f, 1.0f, 0.0f, 1.0f));
                
                textBlock.DrawString(msg, new System.Drawing.RectangleF(0, 0, width, height),     SpriteTextRenderer.TextAlignment.Left | SpriteTextRenderer.TextAlignment.Top, new Color4(1.0f, 1.0f, 1.0f, 1.0f));

                sprite.Flush();
            }
            catch (Exception ex) { Console.WriteLine("Draw Error: " + ex.Message); Console.ReadKey(); }
            }


        public void UpdateFont()
        {
           if (sprite !=null )
            textBlock = new SpriteTextRenderer.SharpDX.TextBlockRenderer(sprite, FontData.Name, SharpDX.DirectWrite.FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, SharpDX.DirectWrite.FontStretch.Normal, FontData.Size);
        }


        bool init = false;

        public bool DeferredContext
        {
            get
            {
                return _deviceContext.TypeInfo == DeviceContextType.Deferred;
            }
        }

        void InitText(SharpDX.DXGI.SwapChain3 tempSwapChain)
        {
            try {

                init = true;
                device = tempSwapChain.GetDevice<Device>();
                _renderTarget = tempSwapChain.GetBackBuffer<Texture2D>(0);

                try
                {
                    _deviceContext = new DeviceContext(device);
                }
                catch (SharpDXException)
                {
                    _deviceContext = device.ImmediateContext;
                }

                _renderTargetView = new RenderTargetView(device, _renderTarget);
             
                if (DeferredContext)
                {
                    
                    _deviceContext.Rasterizer.SetViewport(new ViewportF(0, 0, _renderTarget.Description.Width, _renderTarget.Description.Height, 0, 1));
                    //_deviceContext.Rasterizer.SetViewports(new SharpDX.Mathematics.Interop.RawViewportF[]
                    //{
                    //    new SharpDX.Mathematics.Interop.RawViewportF()
                    //    {
                    //        X = 0,
                    //        Y = 0,
                    //        Width = _renderTarget.Description.Width,
                    //        Height = _renderTarget.Description.Height,
                    //        MinDepth = 0,
                    //        MaxDepth = 1
                    //    }
                    //});
                   
                    _deviceContext.OutputMerger.SetTargets(_renderTargetView);
                  
                }

                sprite = new SpriteTextRenderer.SharpDX.SpriteRenderer(device);
                sprite.ScreenSize = new SpriteTextRenderer.STRVector() { X = swapChain.Description.ModeDescription.Width, Y = swapChain.Description.ModeDescription.Height };

                if (sprite.FailedViewPort == true) { sprite.viewport = new SpriteTextRenderer.STRViewport { Width = _renderTarget.Description.Width, Height = _renderTarget.Description.Height };  };

                UpdateFont();
            }
            catch ( Exception ex) {
            Console.WriteLine("InitText Error: " + ex.Message);
                Console.WriteLine(ex.StackTrace.ToString());
            }
            
        }

    }
}
