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

        static void BeolvasExcluded()
        {
            try
            {
                StreamReader file = new StreamReader("excluded.csv");
                try
                {
                    while (!file.EndOfStream)
                    {
                        excluded.Add(file.ReadLine());
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
     
        static void Main(string[] args)
        {
            BeolvasExcluded();
            Console.WriteLine("\nVége...");
            Console.ReadLine();
        }
    }
}
