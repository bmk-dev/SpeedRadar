using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using Rage;
using Rage.Attributes;
using Rage.Native;

[assembly: Rage.Attributes.Plugin("SpeedRadar", Description = "Displays target vehicle speed.", Author = "Straysify")]

namespace GTAVSpeedRadar
{
    public class SpeedRadar
    {
        public static int checkpoint;
        public static Vector3 radarPos;
        public static bool radarActivated = false;
        public static string targetModel = "";
        public static int targetSpeed;
        public static string speedUnits = "KMH";
        public static int xOffset = 0;
        public static int yOffset = 0;
        public static int zOffset = 0;
        public static INIFile settings;
        public static void Main()
        {
            settings = new INIFile("Plugins\\SpeedRadar.ini");
            speedUnits = settings.Read("SETTINGS", "RadarUnits");
            string _tKey = settings.Read("SETTINGS", "ToggleRadar");
            string _rKey = settings.Read("SETTINGS", "PositionReset");
            string _yPKey = settings.Read("SETTINGS", "PositionForward");
            string _yMKey = settings.Read("SETTINGS", "PositionBackward");
            string _xPKey = settings.Read("SETTINGS", "PositionRight");
            string _xMKey = settings.Read("SETTINGS", "PositionLeft");
            string _zPKey = settings.Read("SETTINGS", "PositionUp");
            string _zMKey = settings.Read("SETTINGS", "PositionDown");
            KeysConverter c = new KeysConverter();
            Keys yPKey = (Keys)c.ConvertFromString(_yPKey);
            Keys yMKey = (Keys)c.ConvertFromString(_yMKey);
            Keys xPKey = (Keys)c.ConvertFromString(_xPKey);
            Keys xMKey = (Keys)c.ConvertFromString(_xMKey);
            Keys zPKey = (Keys)c.ConvertFromString(_zPKey);
            Keys zMKey = (Keys)c.ConvertFromString(_zMKey);
            Keys tKey = (Keys)c.ConvertFromString(_tKey);
            Keys rKey = (Keys)c.ConvertFromString(_rKey);
            if (!speedUnits.Equals("MPH") && !speedUnits.Equals("KMH"))
            {
                speedUnits = "KMH";
            }
            Game.FrameRender += Game_FrameRender;
            while (true)
            {
                if (radarActivated && Game.LocalPlayer.Character.IsInAnyVehicle(false))
                {
                    foreach (Vehicle v in World.GetAllVehicles())
                    {
                        if (v != null && v.Exists() && v != Game.LocalPlayer.Character.CurrentVehicle && DistanceTo(radarPos, v.Position) <= 7)
                        {
                            targetModel = v.Model.Name;
                            if (speedUnits.Equals("KMH"))
                                targetSpeed = MathHelper.ConvertMetersPerSecondToKilometersPerHourRounded(v.Speed);
                            else
                                targetSpeed = (int)MathHelper.ConvertMetersPerSecondToMilesPerHour(v.Speed);
                        }
                    }

                }
                if(Game.IsKeyDown(tKey)) {
                    if (radarActivated && Game.LocalPlayer.Character.IsInAnyVehicle(false))
                    {
                        radarActivated = false;
                        Rage.Native.NativeFunction.CallByName<uint>("DELETE_CHECKPOINT", checkpoint);
                    }
                    else
                        radarActivated = true;
                    radarPos = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0, 8, -1));
                }
                if (radarActivated && Game.LocalPlayer.Character.IsInAnyVehicle(false))
                {
                    radarPos = Game.LocalPlayer.Character.GetOffsetPosition(new Vector3(0 + xOffset + 0.5f, 8 + yOffset, -1 + zOffset));
                    if (Game.IsKeyDown(rKey))
                    {
                        xOffset = 0;
                        yOffset = 0;
                    }
                    if (Game.IsKeyDown(yPKey))
                        yOffset++;
                    if (Game.IsKeyDown(yMKey))
                        yOffset--;
                    if (Game.IsKeyDown(xPKey))
                        xOffset++;
                    if (Game.IsKeyDown(xMKey))
                        xOffset--;
                    if (Game.IsKeyDown(zPKey))
                        zOffset++;
                    if (Game.IsKeyDown(zMKey))
                        zOffset--;
                    Rage.Native.NativeFunction.CallByName<uint>("DELETE_CHECKPOINT", checkpoint);
                    checkpoint = Rage.Native.NativeFunction.CallByName<int>("CREATE_CHECKPOINT", 45, radarPos.X, radarPos.Y, radarPos.Z, radarPos.X, radarPos.Y, radarPos.Z, 2.5F, 255, 0, 0, 255, 0);
                    Rage.Native.NativeFunction.CallByName<int>("SET_CHECKPOINT_CYLINDER_HEIGHT", checkpoint, 2F, 2F, 2F);
                }
                GameFiber.Yield();
            }
        }

        public static void Game_FrameRender(object sender, GraphicsEventArgs e)
        {
            if (radarActivated && Game.LocalPlayer.Character.IsInAnyVehicle(false))
            {
                Graphics.DrawRectangle(1, 250, 150, 50, Color.FromArgb(200, Color.Black));
                Graphics.DrawText("MODEL: " + targetModel, 3, 253, 0.4F, Color.White, 8, false);
                Graphics.DrawText("SPEED: " + targetSpeed + speedUnits, 3, 273, 0.4F, Color.White, 8, false);
            }
        }

        public static float DistanceTo(Vector3 p1, Vector3 p2)
        {
            return (p2 - p1).Length();
        }
    }
}
