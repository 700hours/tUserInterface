using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria;

namespace tUserInterface.Extension
{
    public static class Helper
    {
        const uint MOUSEEVENTF_LEFTDOWN  = 0x0002;
        const uint MOUSEEVENTF_LEFTUP    = 0x0004;
        const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const uint MOUSEEVENTF_RIGHTUP   = 0x0010;
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, uint dwExtraInfo);
        //https://www.pinvoke.net/default.aspx/user32/GetKeyState.html
        internal enum VirtualKeyStates : int
        {
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
            VK_CANCEL = 0x03,
            VK_MBUTTON = 0x04,
        }
        static Stopwatch stopwatch => new Stopwatch();
        public static void LeftMouse()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Timer timer = new Timer(10);
            timer.AutoReset = false;
            timer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                timer.Dispose();
            };
            timer.Start();
        }
        public static void RightMouse()
        {
            mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        public static Texture2D MagicPixel()
        {
            return null;
            //  System.Drawing.Commons is unsupported
            //MemoryStream mem = new MemoryStream();
            //var bitmap = new System.Drawing.Bitmap(1, 1);
            //using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
            //{
            //    g.FillRectangle(System.Drawing.Brushes.White, 0, 0, 1, 1);
            //    bitmap.Save(mem, ImageFormat.Bmp);
            //}
            //var tex = Texture2D.FromStream(Main.graphics.GraphicsDevice, mem);
            //return tex;
        }
        public static Texture2D FromFile(this Texture2D texture, string path)
        {
            return null;
            //  System.Drawing.Commons is unsupported
            //MemoryStream mem = new MemoryStream();
            //var bitmap = System.Drawing.Bitmap.FromFile(path);
            //bitmap.Save(mem, ImageFormat.Png);
            //var tex = Texture2D.FromStream(Main.graphics.GraphicsDevice, mem);
            //return tex;
        }
    }
    public static class Element
    {
        public static int Width, Height;
        static Point mousePosition => Main.MouseScreen.ToPoint();
        static Point tRelative = new Point();
        static Point relative = new Point();
        
        static bool holdClick = false;
        static bool
            holdW, holdE, holdN, holdS;
        static bool hold = false;

        #region Rectangle


        [Obsolete("Not implemented.")]
        public static void Snap(this Rectangle element)
        {
        }
        public static Rectangle Drag(Rectangle element)
        {
            Point point = mousePosition;
            if (Main.mouseLeft)
            {
                if (element.Contains(point) || holdClick)
                {
                    holdClick = true;
                    return new Rectangle(mousePosition.X - relative.X, mousePosition.Y - relative.Y, element.Width, element.Height);
                }
            }
            else
            {
                holdClick = false;
                relative = RelativeMouse(element, mousePosition);
            }
            return element;
        }
        public static bool Resize(this Rectangle element)
        {
            Point point = RelativeMouse(element, mousePosition);
            Point surfaceMouse = mousePosition;
            
            Rectangle Hold = new Rectangle(element.Left - 15, element.Top - 15, element.Width + 30, element.Height + 30);
            if (Main.mouseLeft)
                hold = false;
            if (Hold.Contains(point))
                hold = true;
            if (hold)
            {
                if (mousePosition.X <= 4 && mousePosition.X >= -4 || holdW)
                {
                    //Mouse.SetCursor(Cursors.SizeWE);
                    if (Main.mouseLeft)
                    {
                        int resize = (int)(surfaceMouse.X - element.Left);
                        element = new Rectangle(element.Left + resize, element.Top, element.Width, element.Height);
                        element.Width -= resize;
                        holdW = true;
                    }
                    if (!Main.mouseLeft/* || element.Width != element.ActualWidth*/)
                    {
                        if (!Main.mouseLeft)
                            holdW = false;
                    }
                    return true;
                }
                else if (mousePosition.Y <= 4 && mousePosition.Y >= -4 || holdN)
                {
                    //Mouse.SetCursor(Cursors.SizeNS);
                    if (Main.mouseLeft)
                    {
                        int resize = (int)(surfaceMouse.Y - element.Top);
                        element = new Rectangle(element.Left, element.Top + resize, 0, 0);
                        element.Height -= resize;
                        holdN = true;
                    }
                    if (!Main.mouseLeft/* || element.Width != element.ActualWidth*/)
                    {
                        if (!Main.mouseLeft)
                            holdN = false;
                    }
                    return true;
                }
                else if (mousePosition.X >= element.Width - 4 && mousePosition.X <= element.Width + 4 || holdE)
                {
                    //Mouse.SetCursor(Cursors.SizeWE);
                    if (Main.mouseLeft)
                    {
                        element.Width = element.Width + (surfaceMouse.X - tRelative.X - element.Left);
                        holdE = true;
                    }
                    if (!Main.mouseLeft/* || element.Width != element.ActualWidth*/)
                    {
                        if (!Main.mouseLeft)
                            holdE = false;
                        tRelative = RelativeMouse(element, mousePosition);
                    }
                    return true;
                }
                else if (mousePosition.Y >= element.Height - 4 && mousePosition.Y <= element.Height + 4 || holdS)
                {
                    //Mouse.SetCursor(Cursors.SizeNS);
                    if (Main.mouseLeft)
                    {
                        element.Height = element.Height + (surfaceMouse.Y - tRelative.Y - element.Top);
                        holdS = true;
                    }
                    if (!Main.mouseLeft/* || element.Height != element.ActualHeight*/)
                    {
                        if (!Main.mouseLeft)
                            holdS = false;
                        tRelative = RelativeMouse(element, mousePosition);
                    }
                    return true;
                }
            }
            return false;
        }
        [Obsolete("Not implemented.")]
        public static void Blur(this Rectangle element, double radius)
        {
            //BlurEffect effect = new BlurEffect();
            //effect.Radius = radius;
            //element.Effect = effect;
        }
        public static Point RelativeMouse(Rectangle element, Point mouse)
        {
            int x = mouse.X - element.Left;
            int y = mouse.Y - element.Top;
            return new Point(x, y);
        }
        public static Point RelativeMouse(this Rectangle element, Point mouse, int Width, int Height)
        {
            int x = Width - element.Left;
            int y = Height - element.Top;
            mouse.X -= x;
            mouse.Y -= y;
            return new Point(mouse.X, mouse.Y);
        }

        public static Rectangle Bounds(this Rectangle element)
        {
            return new Rectangle(element.Left, element.Top, element.Width, element.Height);
        }

        #endregion
    }
}
