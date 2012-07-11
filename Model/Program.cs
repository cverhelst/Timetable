using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Reset();
            TimetableFitter timeFitter = new TimetableFitter();
            //timeFitter.FitCourses(timeFitter.generateDefaultCourses(), timeFitter.generateDefaultTimeTable());
            timeFitter.PushedFitCourses(timeFitter.generateDefaultCourses(), timeFitter.generateDefaultTimeTable(), 30);
            //timeFitter.SqueezedFitCourses(timeFitter.generateDefaultCourses(), timeFitter.generateDefaultTimeTable(), 30);
            Console.BufferWidth = 250;
            IEnumerable<Timetable> uniques = timeFitter.GeneratedTables.Distinct(new TimetableBookedTimeEquality()).Select( t => t.RemoveAllFreeTime()).OrderBy(x => x.ToString());
            int index = 1;
            foreach (Timetable table in uniques)
            {
                String output = index + " " + table.ToString();
                Console.Out.Write(output);
                Logger.Log(output);
                index++;
            }

            Console.Out.WriteLine("Generated {0} timetables", timeFitter.GeneratedTables.Count);
            Console.Out.WriteLine("Of which {0} were unique", uniques.Count());
            Console.Out.WriteLine("Please press enter to continue");
            Console.In.ReadLine();
        }
    }
}
