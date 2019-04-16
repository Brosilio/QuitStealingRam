using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Microsoft.VisualBasic.Devices;
using System.Diagnostics;
using System.Management;

namespace QuitStealingRamDotNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("QSR.Net - .Net/C# port (more of a rewrite) of 0x3F's QuitStealingRam OOM protection");

            Config cfg = new Config("qsr.cfg");
            ComputerInfo ci;
            Process[] processes;

            while (true)
            {
                Thread.Sleep(cfg.IntervalMs);

                ci = new ComputerInfo();

                if((int)(ci.AvailablePhysicalMemory / 1024 / 1024) <= cfg.LowMemThreshold)
                {
                    Console.WriteLine("MURDER SPREE STARTED");
                    processes = Process.GetProcesses();

                    for(int i = 0; i < cfg.Killstreak; i++)
                    {
                        Process proc = processes.FirstOrDefault();

                        for(int p = 0; p < processes.Length; p++)
                        {
                            if (processes[p].PrivateMemorySize64 > proc.PrivateMemorySize64)
                                proc = processes[p];
                        }

                        bool kill = false;

                        if (!cfg.DeathTo)
                        {
                            foreach (string s in cfg.Shitlist)
                            {
                                if (s.ToLower().Trim().Contains(proc.ProcessName.Trim().ToLower()))
                                {
                                    kill = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            kill = true;
                        }

                        if(kill)
                        {
                            try
                            {
                                KillProc(proc, cfg.DoubleHomicide, cfg.Prolicide);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Couldn't kill. Error: " + ex.Message);
                            }
                        }
                    }

                    processes = null;
                }

                ci = null;
            }
        }

        static void KillProc(Process p, bool doubleHomicide, bool prolicide)
        {
            if(!doubleHomicide)
            {
                if(prolicide)
                    foreach (Process child in p.GetChildProcesses())
                        child.Kill();
                p.Kill();
                return;
            }


            // If, for whatever reason, this isn't being run on windows, remove all the following code and remove all references and uses of the doublehomicide bool.

            string args = string.Format("/PID {0} /F", p.Id);
            if (prolicide)
                args += " /T";

            Process.Start("taskkill.exe", args);
            p.Kill();
        }
    }

    public class Config
    {
        /// <summary>
        /// If available system RAM is less than this (in megabytes), the murder spree starts
        /// </summary>
        public int LowMemThreshold = 256;

        /// <summary>
        /// Interval between checks in miliseconds
        /// </summary>
        public int IntervalMs = 2000;

        /// <summary>
        /// Tries to kill the process in every possible way, disregarding all pleas for mercy
        /// </summary>
        public bool DoubleHomicide = true;

        /// <summary>
        /// The amount of processes to kill in descending order from most RAM used
        /// </summary>
        public int Killstreak = 1;

        /// <summary>
        /// If true, also murder all the children processes of the process being killed.
        /// </summary>
        public bool Prolicide = true;

        /// <summary>
        /// Only processes whose names contain something on this list will be killed
        /// This is a comma separated string in the config file
        /// </summary>
        public readonly string[] Shitlist = { "google chrome", "discord" };

        /// <summary>
        /// Always kills the process with the highest memory usage regardless of whether or not it's on the shitlist. #DeathTo
        /// </summary>
        public bool DeathTo = false;

        public Config(string configPath)
        {
            if (!File.Exists(configPath))
            {
                Console.WriteLine("A config file was not located or misplaced or something. Using the excellent defaults.");
                return;
            }
        }
    }

    public static class ProcessExtensions
    {
        public static IEnumerable<Process> GetChildProcesses(this Process process)
        {
            List<Process> children = new List<Process>();
            ManagementObjectSearcher mos = new ManagementObjectSearcher(String.Format("Select * From Win32_Process Where ParentProcessID={0}", process.Id));

            foreach (ManagementObject mo in mos.Get())
            {
                children.Add(Process.GetProcessById(Convert.ToInt32(mo["ProcessID"])));
            }

            return children;
        }
    }
}
