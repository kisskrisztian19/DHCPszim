using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DHCPSzim
{
    class Program
    {

        static List<string> excluded = new List<string>();
        static Dictionary<string, string> dhcp = new Dictionary<string, string>();
        static Dictionary<string, string> reserved = new Dictionary<string, string>();
        static List<string> commands = new List<string>();
        static void BeolvasList(List<string> l, string filename)
        {
            try
            {
                StreamReader file = new StreamReader(filename);
                try
                {
                    while (!file.EndOfStream)
                    {
                        l.Add(file.ReadLine());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    file.Close();
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static string CimEggyelNo(string cim)
        {
            /*
             * cim = "192.168.10.100"
             * return "192.168.10.101"
             * Szétvágni a pontok mentén (split)
             * Utolsót intté konvertálni aztán ++
             * 255-öt ne lépjük túl
             * Összefűzni string-é
             */
            string[] ip = cim.Split('.');
            int utolso = int.Parse(ip[3]);
            if (utolso < 255)
            {
                utolso++;
                ip[3] = utolso.ToString();
            }
            return $"{ip[0]}.{ip[1]}.{ip[2]}.{ip[3]}";
        }

        static void Feladat(string parancs)
        {
            /*
             * parancs = "request;MAC-cím"
             * először csak "request" paranccsal foglalkozunk
             * 
             * Megnézzük, hogy "request"-e (x)
             * Ki kell szedni a MAC címet a parancsból
             */
            if (parancs.Contains("request"))
            {
                string[] a = parancs.Split(';');
                string mac = a[1];
                if (dhcp.ContainsKey(mac))
                {
                    Console.WriteLine($"DHCP: {mac} -> {dhcp[mac]}"); //A dhcp-nek nem értéket adunk, hanem kulcsot, amihez kiadja a value-t
                }
                else
                {
                    if (reserved.ContainsKey(mac))
                    {
                        Console.WriteLine($"RESERVED: {mac} -> {reserved[mac]}");
                        dhcp.Add(mac, reserved[mac]);
                    }
                    else
                    {
                        string indulo = "192.168.10.100";
                        int okt4 = 100;

                        while (okt4 < 200 && (dhcp.ContainsValue(indulo) || reserved.ContainsValue(indulo) || excluded.Contains(indulo)))
                        {
                            okt4++;
                            indulo = CimEggyelNo(indulo);
                        }
                        if (okt4 < 200)
                        {
                            Console.WriteLine($"Kiosztott: {mac} -> {indulo}");
                            dhcp.Add(mac, indulo);
                        }
                        else
                        {
                            Console.WriteLine($"{mac} nincs IP!");
                        }
                    }
                }
            }
            else Console.WriteLine("Nem oké");

        }

        static void Feladatok()
        {
            foreach (var command in commands)
            {
                Feladat(command);
            }
        }

        static void BeolvasDictionary(Dictionary<string, string> d, string filenev)
        {
            try
            {
                StreamReader file = new StreamReader(filenev);
                while (!file.EndOfStream)
                {
                    string[] adatok = file.ReadLine().Split(';');
                    d.Add(adatok[0], adatok[1]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
     
        static void Main(string[] args)
        {
            BeolvasList(excluded, "excluded.csv");
            BeolvasList(commands, "test.csv");
            BeolvasDictionary(dhcp, "dhcp.csv");
            BeolvasDictionary(reserved, "reserved.csv");
            Feladatok();
            Console.WriteLine("\nVége...");
            Console.ReadLine();
        }
    }
}
