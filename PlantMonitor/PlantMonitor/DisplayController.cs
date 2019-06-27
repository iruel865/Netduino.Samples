using System;
using Microsoft.SPOT;
using Netduino.Foundation.RTCs;
using Netduino.Foundation.Displays;

namespace PlantMonitor
{
    public class DisplayController
    {        
        protected GraphicsLibrary graphicsLibrary;

        public DisplayController()
        {
            var display = new SSD1306(0x3C, 400, SSD1306.DisplayType.OLED128x32);
            graphicsLibrary = new GraphicsLibrary(display);
            graphicsLibrary.Clear();
        }

        public void DrawText(string text)
        {
            graphicsLibrary.Clear();
            graphicsLibrary.DrawRectangle(0, 0, 128, 32);
            graphicsLibrary.CurrentFont = new Font8x12();

            int x = (128 - (text.Length * 8)) / 2;

            graphicsLibrary.DrawText(x, 12, text);
            graphicsLibrary.Show();
        }
    }
}