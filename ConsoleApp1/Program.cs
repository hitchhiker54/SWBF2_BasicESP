using System;
using System.Threading;
using System.Diagnostics;

/// <summary>
/// Basic ESP for Star Wars Battlefront 2 (2017)
/// 
/// Credits :
/// Specifically (in no particular order) for helpful posts + guides +  utils : IChooseYou, ShellBullet, txt231, ReUnioN, Kn4ck3r, stevemk14ebr, dude719, jd62, Coltonon, GenMJ, lolp1, RozenMaiden, huangfengye
/// For libraries used in this example code :   YatoDev https://www.unknowncheats.me/forum/c-/180781-direct2d-overlay-window.html
///                                             Striekcarl https://www.unknowncheats.me/forum/c-/215248-sigscansharp-fast-pattern-finder-individual-parallel.html
///                                             Alexandre Mutel http://sharpdx.org/
/// in general : Unknowncheats.me community
/// 
/// NuGet dependencies:
/// SharpDX (v4.01 at time of writing)
/// SharpDX.Direct2D1
/// SharpDX.DXGI
/// SharpDX.Mathematics
/// 
/// Features :
/// Soldier & vehicle ESP
/// Player names
/// Distance
/// 
/// Disclaimer:
/// This source is provided as a demonstration only. Use is prohibited by the games EULA; if you get banned it's your problem.
/// 
/// </summary>

namespace BasicESP
{
    class Program
    {
        static void Main(string[] args)
        {
            // Look for the game and wait if it is not running yet
            Console.WriteLine("Looking for game...");

            while (true)
            {
                if (GetProcessesByName("starwarsbattlefrontii", out Process process))
                {
                    // found the game, wait 1 second and start the overlay
                    Thread.Sleep(1000);
                    Overlay overlay = new Overlay(process);
                    break;
                }

                Thread.Sleep(100);
            }
        }

        private static bool GetProcessesByName(string pName, out Process process)
        {
            Process[] pList = Process.GetProcessesByName(pName);
            process = pList.Length > 0 ? pList[0] : null;
            return process != null;
        }
    }
}
