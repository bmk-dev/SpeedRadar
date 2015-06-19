using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVSpeedRadar
{
    class Graphics
    {
        public static void DrawText(string text, float xPos, float yPos, float scale, System.Drawing.Color color, int font, bool c)
        {
            System.Drawing.Size offset = new System.Drawing.Size();
            float x = (xPos + offset.Width) / 1280;
            float y = (yPos + offset.Height) / 720;
            Rage.Native.NativeFunction.CallByName<uint>("SET_TEXT_FONT", font);
            Rage.Native.NativeFunction.CallByName<uint>("SET_TEXT_SCALE", scale, scale);
            Rage.Native.NativeFunction.CallByName<uint>("SET_TEXT_COLOUR", (int)color.R, (int)color.G, (int)color.B, (int)color.A);
            Rage.Native.NativeFunction.CallByName<uint>("SET_TEXT_CENTRE", false);
            Rage.Native.NativeFunction.CallByName<uint>("_SET_TEXT_ENTRY", "STRING");
            Rage.Native.NativeFunction.CallByName<uint>("_ADD_TEXT_COMPONENT_STRING", text);
            Rage.Native.NativeFunction.CallByName<uint>("_DRAW_TEXT", x, y);
        }
        public static void DrawRectangle(float xPos, float yPos, float width, float height, System.Drawing.Color color)
        {
            System.Drawing.Size offset = new System.Drawing.Size();
            float w = width / 1280;
            float h = height / 720;
            float x = ((xPos + offset.Width) / 1280) + w * 0.5F;
            float y = ((yPos + offset.Height) / 720) + h * 0.5F;
            Rage.Native.NativeFunction.CallByName<uint>("DRAW_RECT", x, y, w, h, (int)color.R, (int)color.G, (int)color.B, (int)color.A);
        }
    }
}
