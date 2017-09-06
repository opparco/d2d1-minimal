using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DXGI;

namespace WindowsFormsApp1
{
    class Viewer : IDisposable
    {
        Control control;

        SharpDX.Direct2D1.Factory d2dFactory;

        RenderTarget renderTarget;
        SolidColorBrush solidColorBrush;

        int CreateDeviceIndependentResources()
        {
            d2dFactory = new SharpDX.Direct2D1.Factory();

            return 0;
        }

        int CreateDeviceResources(ref Size2 size)
        {
            if (renderTarget == null)
            {
                renderTarget = new WindowRenderTarget(d2dFactory, new RenderTargetProperties(), new HwndRenderTargetProperties { Hwnd = control.Handle, PixelSize = size });

                solidColorBrush = new SolidColorBrush(renderTarget, Color.White);
            }
            return 0;
        }

        int DiscardDeviceResources()
        {
            if (solidColorBrush != null)
            {
                solidColorBrush.Dispose();
                solidColorBrush = null;
            }
            if (renderTarget != null)
            {
                renderTarget.Dispose();
                renderTarget = null;
            }
            return 0;
        }

        public bool InitializeGraphics(Control control)
        {
            this.control = control;

            CreateDeviceIndependentResources();

            return true;
        }

        public void Update()
        {

        }

        public void Render()
        {
            Size2 size = new Size2(control.ClientSize.Width, control.ClientSize.Height);

            CreateDeviceResources(ref size);

            renderTarget.BeginDraw();
            renderTarget.Clear(Color.Black);
            renderTarget.DrawRectangle(new RectangleF(16, 16, 64, 64), solidColorBrush);
            try
            {
                renderTarget.EndDraw();
            }
            catch (SharpDXException ex) when ((uint)ex.HResult == 0x8899000C) // D2DERR_RECREATE_TARGET
            {
                // device has been lost!
                DiscardDeviceResources();
            }
        }

        public void Dispose()
        {
            Console.WriteLine("viewer Dispose now!");

            solidColorBrush?.Dispose();
            renderTarget?.Dispose();

            d2dFactory?.Dispose();
        }
    }
}
