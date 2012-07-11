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
            Console.BufferWidth = 250;
            IEnumerable<Timetable> uniques = timeFitter.GeneratedTables.Distinct(new TimetableBookedTimeEquality()).Select( t => t.RemoveAllFreeTime());
            foreach (Timetable table in uniques)
            {
                Console.Out.Write(table.ToString());
                Logger.Log(table.ToString());
            }

            Console.Out.WriteLine("Generated {0} timetables", timeFitter.GeneratedTables.Count);
            Console.Out.WriteLine("Of which {0} were unique", uniques.Count());
            Console.Out.WriteLine("Please press enter to continue");
            Console.In.ReadLine();
        }
    }
}
