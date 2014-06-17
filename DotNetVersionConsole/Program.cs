using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Win32;


namespace DotNetVersionConsole
{

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The following .NET Framework versions are installed:\n");
            Console.WriteLine(String.Format("|{0,12}|{1,16}|{2,5}|{3,7}|{4,14}|", "Name", "Version", "SP", "Release", "4.5.x Version"));
            Console.WriteLine(new string('-', 60));
            GetVersionFromRegistry();
            Console.WriteLine(new string('-', 60));
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static int Get45or451FromRegistry()
        {
            using (RegistryKey ndpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
            {
                int releaseKey = (int)ndpKey.GetValue("Release");
                {
                    return releaseKey;
                }
            }
        }

        private static void GetVersionFromRegistry()
        {
        using (RegistryKey ndpKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))        
            {
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {
                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        string name = (string)versionKey.GetValue("Version", "");
                        string sp = versionKey.GetValue("SP", "").ToString();
                        string install = versionKey.GetValue("Install", "").ToString();
                        string releaseKey = "";
                        string net45xVer = "";
                        if (name.StartsWith("4.5"))
                        {
                            int releaseKeyNumber = Get45or451FromRegistry();
                            releaseKey = Get45or451FromRegistry().ToString();
                            if (releaseKeyNumber == 378675 | releaseKeyNumber == 378675)
                                net45xVer = "4.5.1";
                            if (releaseKeyNumber == 379893)
                                net45xVer = "4.5.2";
                            if (releaseKeyNumber > 379893)
                                net45xVer = "4.5.2+";
                        }
                                                    
                        if (install == "") //no install info, must be later.                            
                            Console.WriteLine(String.Format("|{0,12}|{1,16}|{2,5}|{3,7}|{4,14}|", versionKeyName, name, sp, releaseKey, net45xVer));
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                Console.WriteLine(String.Format("|{0,12}|{1,16}|{2,5}|{3,7}|{4,14}|", versionKeyName, name, sp, releaseKey, net45xVer));                             
                            }
                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();
                            install = subKey.GetValue("Install", "").ToString();
                            if (name.StartsWith("4.5"))
                            {
                                releaseKey = Get45or451FromRegistry().ToString();
                                int releaseKeyNumber = Get45or451FromRegistry();
                                if (releaseKeyNumber == 378675 | releaseKeyNumber == 378758)
                                    net45xVer = "4.5.1";
                                if (releaseKeyNumber == 379893)
                                    net45xVer = "4.5.2";
                                if (releaseKeyNumber > 379893)
                                    net45xVer = "4.5.2+";
                            }
                            if (install == "") //no install info, must be later.
                                Console.WriteLine(String.Format("|{0,12}|{1,16}|{2,5}|{3,7}|{4,14}|", versionKeyName, name, sp, releaseKey, net45xVer));
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    Console.WriteLine(String.Format("|{0,12}|{1,16}|{2,5}|{3,7}|{4,14}|", subKeyName, name, sp, releaseKey, net45xVer));
                                }
                                else if (install == "1")
                                {
                                    Console.WriteLine(String.Format("|{0,12}|{1,16}|{2,5}|{3,7}|{4,14}|", subKeyName, name, sp, releaseKey, net45xVer));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
