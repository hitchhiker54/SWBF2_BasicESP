using SharpDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BasicESP
{
    class GameManager
    {
        private ulong pGameContext;
        private ulong pGameRenderer;
        private ulong pFirstTypeInfo;
        private ulong pPlayerManager;
        private ulong pGameRenderView;
        private Frostbite.ClientGameContext gameContext;
        private Frostbite.ClientPlayerManager playerManager;
        private Frostbite.GameRenderer gameRenderer;
        private Frostbite.RenderView gameRenderview;
        private Matrix ViewProjection;
        private ulong maxPlayers = 64;
        private Rectangle ScreenSize;

        public Player LocalPlayer { get; set; }
        public List<Player> AllPlayers;

        public bool IsValidGame { get; set; }

        public GameManager(Process process, Rectangle screenSize)
        {
            SigScanSharp SigScan = new SigScanSharp(RPM.GetHandle());
            var t = process.MainModule;
            SigScan.SelectModule(process.MainModule);

            SigScan.AddPattern("GameRenderer", "48 8B 0D ? ? ? ? 48 85 C9 74 0B 48 8B 01 BA ? ? ? ? FF 50 68");
            SigScan.AddPattern("GameContext", "48 89 15 ? ? ? ? 48 89 CB");
            SigScan.AddPattern("FirstTypeInfo", "48 8B 05 ? ? ? ? 48 89 41 08 48 89 0D ? ? ? ?");

            var result = SigScan.FindPatterns(out long lTime);

            pGameContext = SigScan.FindOffset(result["GameContext"]);
            pGameRenderer = SigScan.FindOffset(result["GameRenderer"]);
            pFirstTypeInfo = SigScan.FindOffset(result["FirstTypeInfo"]);

            Console.WriteLine("GameRenderer  : 0x{0}", pGameRenderer.ToString("X"));
            Console.WriteLine("GameContext   : 0x{0}", pGameContext.ToString("X"));
            Console.WriteLine("FirstTypeInfo : 0x{0}", pFirstTypeInfo.ToString("X"));

            if (!RPM.IsValid(pGameContext)) { IsValidGame = false; return; }
            if (!RPM.IsValid(pGameRenderer)) { IsValidGame = false; return; }
            if (!RPM.IsValid(pFirstTypeInfo)) { IsValidGame = false; return; }

            pGameContext = RPM.Read<ulong>(pGameContext);
            pGameRenderer = RPM.Read<ulong>(pGameRenderer);

            gameContext = RPM.Read<Frostbite.ClientGameContext>(pGameContext);
            gameRenderer = RPM.Read<Frostbite.GameRenderer>(pGameRenderer);

            pPlayerManager = gameContext.m_ClientPlayerManager;
            pGameRenderView = gameRenderer.m_RenderView;

            if (!RPM.IsValid(gameContext.m_ClientPlayerManager)) { IsValidGame = false; return; }
            playerManager = RPM.Read<Frostbite.ClientPlayerManager>(gameContext.m_ClientPlayerManager);

            AllPlayers = new List<Player>();

            ScreenSize = screenSize;
        }

        public bool UpdateFrame(Rectangle screenSize)
        {
            ScreenSize = screenSize;
            IsValidGame = false;

            AllPlayers.Clear();

            if (!RPM.IsValid(gameContext.m_ClientPlayerManager)) { return false; }
            playerManager = RPM.Read<Frostbite.ClientPlayerManager>(gameContext.m_ClientPlayerManager);

            if (!RPM.IsValid(gameRenderer.m_RenderView)) { return false; }
            gameRenderview = RPM.Read<Frostbite.RenderView>(gameRenderer.m_RenderView);
            ViewProjection = gameRenderview.m_ViewProjection;

            if (!RPM.IsValid(playerManager.m_LocalPlayer)) { return false; }
            LocalPlayer = new Player(playerManager.m_LocalPlayer);

            if (LocalPlayer.IsValidPlayer)
            {
                if (!RPM.IsValid(playerManager.m_PlayerList)) { return false; }

                for (ulong i = 0; i < maxPlayers; i++)
                {
                    Player player = new Player(RPM.Read<ulong>(playerManager.m_PlayerList + (i * sizeof(ulong))));

                    if ((player.IsValidPlayer) && (player.Name != LocalPlayer.Name))
                    {
                        if ((player.TeamId != LocalPlayer.TeamId) && (player.TeamId != 0))
                        {
                            player.ScreenPosition = (Vector2)WorldToScreen(player.Position);
                            player.Distance = Vector3.Subtract(LocalPlayer.Position, player.Position).Length();

                            AllPlayers.Add(player);
                        }
                    }
                }
            }

            IsValidGame = true;
            return true;
        }

        public Vector3 WorldToScreen(Vector3 location)
        {
            Vector3 screen = new Vector3(0, 0, 0);

            float screenW =
                (ViewProjection.M14 * location.X)
                + (ViewProjection.M24 * location.Y)
                + (ViewProjection.M34 * location.Z + ViewProjection.M44);

            if (screenW < 0.0001f)
                return screen;

            float screenX =
                (ViewProjection.M11 * location.X)
                + (ViewProjection.M21 * location.Y)
                + (ViewProjection.M31 * location.Z + ViewProjection.M41);

            float screenY =
                (ViewProjection.M12 * location.X)
                + (ViewProjection.M22 * location.Y)
                + (ViewProjection.M32 * location.Z + ViewProjection.M42);


            screen.X = (ScreenSize.Width / 2) + (ScreenSize.Width / 2) * screenX / screenW;
            screen.Y = (ScreenSize.Height / 2) - (ScreenSize.Height / 2) * screenY / screenW;
            screen.Z = screenW;

            return screen;
        }

        public bool WorldToScreen(Vector3 playerOrigin, out Vector3 screen)
        {
            screen = new Vector3(0, 0, 0);

            float screenW =
                (ViewProjection.M14 * playerOrigin.X)
                + (ViewProjection.M24 * playerOrigin.Y)
                + (ViewProjection.M34 * playerOrigin.Z + ViewProjection.M44);

            if (screenW < 0.0001f)
                return false;

            float screenX =
                (ViewProjection.M11 * playerOrigin.X)
                + (ViewProjection.M21 * playerOrigin.Y)
                + (ViewProjection.M31 * playerOrigin.Z + ViewProjection.M41);

            float screenY =
                (ViewProjection.M12 * playerOrigin.X)
                + (ViewProjection.M22 * playerOrigin.Y)
                + (ViewProjection.M32 * playerOrigin.Z + ViewProjection.M42);


            screen.X = (ScreenSize.Width / 2) + (ScreenSize.Width / 2) * screenX / screenW;
            screen.Y = (ScreenSize.Height / 2) - (ScreenSize.Height / 2) * screenY / screenW;
            screen.Z = screenW;

            return true;
        }

        public Vector3 MultiplyMat(Vector3 vector, Matrix mat)
        {
            return new Vector3(mat.M11 * vector.X + mat.M21 * vector.Y + mat.M31 * vector.Z,
                               mat.M12 * vector.X + mat.M22 * vector.Y + mat.M32 * vector.Z,
                               mat.M13 * vector.X + mat.M23 * vector.Y + mat.M33 * vector.Z);
        }
    }
}
