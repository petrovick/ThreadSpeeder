using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using ThreadSpeeder.Example.Application;
using ThreadSpeeder.Example.Business;

namespace ThreadSpeeder.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Process proc = Process.GetCurrentProcess();
            var memoryBeforeCreatingObjects = proc.PrivateMemorySize64;

            #region Make objects for testing, don't focus on that :)
            var timeToMakeObjectsBefore = DateTime.Now;
            List<User> users = new List<User>();
            for (int i = 0; i < 50; i++)
            {
                users.Add(new User()
                {
                    Address = new Address()
                    {
                        Street = "Fake address " + i,
                    },
                    Age = (byte)(i % 39),
                    Email = "fakeEmail" + i + " @email.com",
                    IdUser = i,
                    Name = "Fake name " + i,
                    Salary = i
                });
            }
            var timeToMakeObjectsAfter = DateTime.Now;
            var secondsToMakeObjects = timeToMakeObjectsAfter.Subtract(timeToMakeObjectsBefore);
            #endregion

            var memoryAfterCreatingObjects = proc.PrivateMemorySize64;


            PerformanceCounter cpuCounter;
            PerformanceCounter ramCounter;

            cpuCounter = new PerformanceCounter();

            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            #region Thread Speeder
            var dateBeforeProcessWithThreadSpeeder = DateTime.Now;
            var cpuCounterThreadSpeederStart = cpuCounter.NextValue();
            var ramCounterThreadSpeederStart = ramCounter.NextValue();
            new UserApplication().ProcessWithThreadSpeeder(users);
            var cpuCounterThreadSpeederFinish = cpuCounter.NextValue();
            var ramCounterThreadSpeederFinish = ramCounter.NextValue();
            var dateAfterProcessWithThreadSpeeder = DateTime.Now;
            var secondsWithThreadSpeeder = dateAfterProcessWithThreadSpeeder.Subtract(dateBeforeProcessWithThreadSpeeder);
            #endregion


            #region Without Thread Speeder
            var dateBeforeProcessWithoutThreadSpeeder = DateTime.Now;
            var cpuCounterWithoutThreadSpeederStart = cpuCounter.NextValue();
            var ramCounterWithoutThreadSpeederStart = ramCounter.NextValue();
            new UserApplication().ProcessWithoutThreadSpeeder(users);
            var cpuCounterWithoutThreadSpeederFinish = cpuCounter.NextValue();
            var ramCounterWithoutThreadSpeederFinish = ramCounter.NextValue();
            var dateAfterProcessWithoutThreadSpeeder = DateTime.Now;
            var secondsWithoutThreadSpeeder = dateAfterProcessWithoutThreadSpeeder.Subtract(dateBeforeProcessWithoutThreadSpeeder);
            #endregion

            var processorname = "";
            ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject mo in mos.Get())
            {
                processorname = mo["Name"].ToString();
            }


            Console.WriteLine(String.Format("|{0,20}-{1,50}-{2,15}-{3,20}-{4,15}-{5,16}-{6,15}|", "--------------------", "--------------------------------------------------", "---------------", "--------------------", "---------------", "---------------", "---------------"));
            Console.WriteLine(String.Format("|{0,20}|{1,50}|{2,15}|{3,20}|{4,15}|{5,16}|{6,15}|", "Descrição", "CPU", "Total Memory", "CPU Usage(%)", "RAM(Mbs)", "Time", "Nº Total List"));
            Console.WriteLine(String.Format("|{0,20}|{1,50}|{2,15}|{3,20}|{4,15}|{5,16}|{6,15}|", "No ThreadSpeeder", processorname, "16GB", (cpuCounterWithoutThreadSpeederFinish - cpuCounterWithoutThreadSpeederStart), (ramCounterWithoutThreadSpeederFinish - ramCounterWithoutThreadSpeederStart), secondsWithoutThreadSpeeder.ToString(), users.Count));
            Console.WriteLine(String.Format("|{0,20}|{1,50}|{2,15}|{3,20}|{4,15}|{5,16}|{6,15}|", "ThreadSpeeder", processorname, "16GB", (cpuCounterThreadSpeederFinish - cpuCounterThreadSpeederStart), (ramCounterThreadSpeederFinish - ramCounterThreadSpeederStart), secondsWithThreadSpeeder.ToString(), users.Count));
            Console.WriteLine(String.Format("|{0,20}-{1,50}-{2,15}-{3,20}-{4,15}-{5,16}-{6,15}|", "--------------------", "--------------------------------------------------", "---------------", "--------------------", "---------------", "---------------", "---------------"));

            Console.WriteLine("");
            Console.ReadLine();



        }


    }
}
