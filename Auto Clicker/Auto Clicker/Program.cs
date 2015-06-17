using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auto_Clicker
{
    class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */
        private const int MOUSEEVENTF_RIGHTUP = 0x0010; /* right button up */
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; /* middle button down */
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040; /* middle button up */
        private const int MOUSEEVENTF_XDOWN = 0x0080; /* x button down */
        private const int MOUSEEVENTF_XUP = 0x0100; /* x button down */
        private const int MOUSEEVENTF_WHEEL = 0x0800; /* wheel button rolled */
        private const int MOUSEEVENTF_VIRTUALDESK = 0x4000; /* map to entire virtual desktop */
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000; /* absolute move */

        static int klikniecia = 0;
        static int przerwa = 0;
        static bool randomizacja = false;
        static bool zapetlenie = true;
        static CancellationTokenSource cts;

        static void Main(string[] args)
        {
            Console.WriteLine("Wybierz ilość kliknięć (0 dla nieskończoności):");
            klikniecia = int.Parse(Console.ReadLine());
            if (klikniecia != 0)
                zapetlenie = false;
            Console.WriteLine("Wybierz czas pomiędzy kolejnymi kliknięciami w ms:");
            przerwa = int.Parse(Console.ReadLine());
            Console.WriteLine("Czy włączyć nieregularność kliknięć? [t/n]");
            if (Console.Read() == 116)
                randomizacja = true;

            cts = new CancellationTokenSource();
            Task klikanie = new Task(() => Klikanie(klikniecia, przerwa, randomizacja, zapetlenie, cts.Token));
            klikanie.Start();
            Console.WriteLine("Wystartowano! By zakończyć, zaczekaj na koniec klikania lub naciśnij dowolny klawisz. . .");
            Console.ReadKey();
            if (cts != null)
                cts.Cancel();
            while (!klikanie.IsCompleted) { }


        }
        async static void Klikanie(int klikniecia, int przerwa, bool random, bool zapetlenie, CancellationToken cs)
        {
            if (zapetlenie)
                while (true)
                {
                    //Console.WriteLine(String.Format("{}, {}, {};", klikniecia, przerwa, zapetlenie));
                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                    await Task.Delay(przerwa);
                }
            else
            {
                for (int i = 0; i < klikniecia; i++)
                {
                    //Console.WriteLine(String.Format("{0}, {1}, {2};", i, przerwa, zapetlenie ));
                    mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                    await Task.Delay(przerwa);
                }
            }
        }

    }
}
