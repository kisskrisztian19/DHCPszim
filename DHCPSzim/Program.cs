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
            foreach (var d in commands)
            {
                Console.WriteLine(d);
            }
            Console.WriteLine("\nVége...");
            Console.ReadLine();
        }
    }
}
