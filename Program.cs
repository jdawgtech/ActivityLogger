using System;
using System.Collections.Generic;
using System.IO;

namespace ActivityLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Activity Logger!");

            var today = DateTime.Today.ToString("yyyyMMdd");

            string filePath = $"{today}.csv";

            if (!File.Exists(filePath))
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("activity name, started, stopped, duration, time spent seconds");
                }
            }

            List<Activity> activities = new List<Activity>();

            while (true)
            {
                Console.Write("Enter activity: ");
                string activity = Console.ReadLine();

                DateTime currentTime = DateTime.Now;

                var lastActivity = activities.LastOrDefault();
                if (lastActivity != null)
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        TimeSpan timeDiff = currentTime - lastActivity.Started;
                        lastActivity.Stopped = currentTime;
                        writer.WriteLine($", {lastActivity.Stopped.ToShortTimeString()}, {lastActivity.Diff.ToString("hh\\:mm\\:ss")}, {lastActivity.Diff.Seconds}");
                    }
                }

                if (activity == "quit")
                {
                    return;
                }

                var newActivity = new Activity()
                {
                    Name = activity,
                    Started = currentTime
                };
                activities.Add(newActivity);

                using (StreamWriter writer = new StreamWriter(filePath, true))
                {

                    writer.Write($"{newActivity.Name}, {newActivity.Started.ToShortTimeString()}");
                }

            }
        }
    }
    class Activity
    {
        public string Name { get; set; }
        public DateTime Started { get; set; }
        public DateTime Stopped { get; set; }

        public TimeSpan Diff
        {
            get
            {
                return Stopped - Started;
            }
        }

        public override string ToString()
        {
            return $"{Name},{Started.TimeOfDay},{Stopped.TimeOfDay},{Diff.TotalSeconds}";
        }
    }
}
