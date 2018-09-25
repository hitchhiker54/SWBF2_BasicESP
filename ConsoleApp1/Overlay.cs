using System;
using System.Diagnostics;
using System.Threading;
using Yato.DirectXOverlay;
using SharpDX;

namespace BasicESP
{
    public class Overlay
    {
        private IntPtr handle;
        private Process process = null;

        private Thread updateThread = null;
        private Thread gameCheckThread = null;
        private GameManager gameManager;

        private OverlayWindow overlay;
        private Direct2DRenderer d2d;
        private Direct2DBrush clearBrush;

        private bool IsGameRunning = true;

        private bool OPTIONS_AA = true;
        private bool OPTIONS_VSync = true;
        private bool OPTIONS_ShowFPS = false;

        public Overlay(Process gameProcess)
        {
            process = gameProcess;

            // check the game window exists then create the overlay
            while (true)
            {
                handle = NativeMethods.FindWindow(null, "STAR WARS Battlefront II");

                if (handle != IntPtr.Zero)
                {
                    break;
                }
            }

            // check if game running. timed at 2-5ms per call so runs in own thread
            gameCheckThread = new Thread(new ParameterizedThreadStart(GameCheck));
            gameCheckThread.Start();

            // Starting the ESP before the game leaves invalid process info so we'll wait a second to let the game check thread fix that
            if (process.MainWindowHandle == IntPtr.Zero)
            {
                Thread.Sleep(1000);
            }

            // set up the remote process memory class
            RPM.OpenProcess(process.Id);

            // setup the overlay
            var rendererOptions = new Direct2DRendererOptions()
            {
                AntiAliasing = OPTIONS_AA,
                Hwnd = IntPtr.Zero,
                MeasureFps = OPTIONS_ShowFPS,
                VSync = OPTIONS_VSync
            };

            OverlayManager manager = new OverlayManager(handle, rendererOptions);

            overlay = manager.Window;
            d2d = manager.Graphics;
            clearBrush = d2d.CreateBrush(0xF5, 0xF5, 0xF5, 0);  // our transparent colour

            // start the update thread
            updateThread = new Thread(new ParameterizedThreadStart(Update));
            updateThread.Start();
        }
       
        private void GameCheck(object sender)
        {
            while (IsGameRunning)
            {
                Process[] pList = Process.GetProcessesByName("starwarsbattlefrontii");
                process = pList.Length > 0 ? pList[0] : null;
                if (process == null)
                {
                    IsGameRunning = false;
                }

                Thread.Sleep(100);
            }
        }

        private void Update(object sender)
        {
            // set up our colours for drawing
            var blackBrush = d2d.CreateBrush(0, 0, 0, 255);
            var redBrush = d2d.CreateBrush(153, 0, 0, 128);
            var yellowBrush = d2d.CreateBrush(153, 153, 0, 128);
            var orangeBrush = d2d.CreateBrush(255, 200, 0, 255);
            var darkOrangeBrush = d2d.CreateBrush(255, 100, 0, 255);
            var greenBrush = d2d.CreateBrush(0, 255, 0, 255);
            var blueBrush = d2d.CreateBrush(0, 0, 255, 255);
            var whiteBrush = d2d.CreateBrush(250, 250, 250, 128);
            var vehicleBrush = d2d.CreateBrush(243, 243, 255, 128);
            var heroVisBrush = d2d.CreateBrush(240, 0, 255, 255);
            var heroBrush = d2d.CreateBrush(120, 0, 120, 255);

            // and our font
            var font = d2d.CreateFont("Tahoma", 9);

            Direct2DBrush brush;

            Console.WriteLine("Initialising...");

            // initialise the GameManager class that handles the player data
            gameManager = new GameManager(process, new Rectangle(0, 0, overlay.Width, overlay.Height));

            Console.WriteLine("Ready.");

            // main loop
            while (IsGameRunning)
            {
                if (gameManager.UpdateFrame(new Rectangle(0, 0, overlay.Width, overlay.Height)))
                {
                    d2d.BeginScene();
                    d2d.ClearScene(clearBrush);

                    if (OPTIONS_ShowFPS)
                    {
                        d2d.DrawTextWithBackground($"FPS: {d2d.FPS}", 20, 20, font, greenBrush, blackBrush);
                    }

                    foreach (Player player in gameManager.AllPlayers)
                    {
                        if (!player.IsDead)
                        {
                            if (player.IsVisible)
                            {
                                if (player.MaxHealth < 400) brush = yellowBrush;
                                else brush = heroVisBrush;
                            }
                            else
                            {
                                if (player.MaxHealth < 400) brush = redBrush;
                                else brush = heroBrush;
                            }

                            if (!player.InVehicle)
                            {
                                /*if (!player.IsVisible)*/ DrawAABB(player.TransformAABB, brush);
                            }
                            else
                            {
                                brush = vehicleBrush;

                                DrawAABB(player.TransformAABB, brush);
                            }

                            var name = player.Name;
                            var dist = $"{(int)player.Distance}m";

                            Vector3 textPos = new Vector3(player.Position.X, player.Position.Y, player.Position.Z);
                            if (gameManager.WorldToScreen(textPos, out textPos))
                            {
                                var textPosX = textPos.X - ((name.Length * font.FontSize) / 4);
                                d2d.DrawText(name, textPosX - 1, textPos.Y - 1, font, blackBrush);
                                d2d.DrawText(name, textPosX, textPos.Y, font, whiteBrush);

                                textPosX = textPos.X - ((dist.Length * font.FontSize) / 4);
                                var textPosY = textPos.Y + font.FontSize;

                                d2d.DrawText(dist, textPosX - 1, textPosY - 1, font, blackBrush);
                                d2d.DrawText(dist, textPosX, textPosY, font, whiteBrush);
                            }
                        }
                    }

                    d2d.EndScene();
                }
            }

            // clean up if the game has closed
            RPM.CloseProcess();
            Environment.Exit(0);
        }

        private void DrawAABB(Frostbite.TransformAABBStruct TransformAABB, Direct2DBrush brush)
        {
            Vector3 pos = TransformAABB.Matrix.TranslationVector;

            Vector3 min = new Vector3(TransformAABB.AABB.Min.X, TransformAABB.AABB.Min.Y, TransformAABB.AABB.Min.Z);
            Vector3 max = new Vector3(TransformAABB.AABB.Max.X, TransformAABB.AABB.Max.Y, TransformAABB.AABB.Max.Z);

            Vector3 crnr2 = pos + gameManager.MultiplyMat(new Vector3(max.X, min.Y, min.Z), TransformAABB.Matrix);
            Vector3 crnr3 = pos + gameManager.MultiplyMat(new Vector3(max.X, min.Y, max.Z), TransformAABB.Matrix);
            Vector3 crnr4 = pos + gameManager.MultiplyMat(new Vector3(min.X, min.Y, max.Z), TransformAABB.Matrix);
            Vector3 crnr5 = pos + gameManager.MultiplyMat(new Vector3(min.X, max.Y, max.Z), TransformAABB.Matrix);
            Vector3 crnr6 = pos + gameManager.MultiplyMat(new Vector3(min.X, max.Y, min.Z), TransformAABB.Matrix);
            Vector3 crnr7 = pos + gameManager.MultiplyMat(new Vector3(max.X, max.Y, min.Z), TransformAABB.Matrix);

            min = pos + gameManager.MultiplyMat(min, TransformAABB.Matrix);
            max = pos + gameManager.MultiplyMat(max, TransformAABB.Matrix);

            if (!gameManager.WorldToScreen(min, out min) || !gameManager.WorldToScreen(max, out max)
                || !gameManager.WorldToScreen(crnr2, out crnr2) || !gameManager.WorldToScreen(crnr3, out crnr3)
                || !gameManager.WorldToScreen(crnr4, out crnr4) || !gameManager.WorldToScreen(crnr5, out crnr5)
                || !gameManager.WorldToScreen(crnr6, out crnr6) || !gameManager.WorldToScreen(crnr7, out crnr7))
                return;

            d2d.DrawLine(min.X, min.Y, crnr2.X, crnr2.Y, 1, brush);
            d2d.DrawLine(min.X, min.Y, crnr4.X, crnr4.Y, 1, brush);
            d2d.DrawLine(min.X, min.Y, crnr6.X, crnr6.Y, 1, brush);

            d2d.DrawLine(max.X, max.Y, crnr5.X, crnr5.Y, 1, brush);
            d2d.DrawLine(max.X, max.Y, crnr7.X, crnr7.Y, 1, brush);
            d2d.DrawLine(max.X, max.Y, crnr3.X, crnr3.Y, 1, brush);

            d2d.DrawLine(crnr2.X, crnr2.Y, crnr7.X, crnr7.Y, 1, brush);
            d2d.DrawLine(crnr2.X, crnr2.Y, crnr3.X, crnr3.Y, 1, brush);

            d2d.DrawLine(crnr4.X, crnr4.Y, crnr5.X, crnr5.Y, 1, brush);
            d2d.DrawLine(crnr4.X, crnr4.Y, crnr3.X, crnr3.Y, 1, brush);

            d2d.DrawLine(crnr6.X, crnr6.Y, crnr5.X, crnr5.Y, 1, brush);
            d2d.DrawLine(crnr6.X, crnr6.Y, crnr7.X, crnr7.Y, 1, brush);
        }
    }
}
