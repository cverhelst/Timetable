using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Program
    {
        static void Main(string[] args)
        {
            Logger.Reset();
            TimetableFitter fitter = new TimetableFitter();
            fitter.NormalFitCourses(fitter.generateDefaultCourses(), fitter.generateDefaultTimeTable());
            List<Timetable> tables = fitter.GeneratedTables.ToList();
            List<Timetable> uniques = fitter.UniquelyGeneratedTables.ToList();
            Console.BufferWidth = 250;
            int index = 1;
            foreach (Timetable table in uniques)
            {
                String output = index + " " + table.ToString();
                Console.Out.Write(output);
                Logger.Log(output);
                index++;
            }

            Console.Out.WriteLine("Generated {0} timetables", tables.Count);
            Console.Out.WriteLine("Of which {0} were unique", uniques.Count());
            Console.Out.WriteLine("Please press enter to continue");
            Console.In.ReadLine();
        }

        static public List<Timetable> GenerateTables(String method)
        {
            TimetableFitter timeFitter = new TimetableFitter();

            switch(method){
                case "Pushed":
                    timeFitter.GeneratedTables.Clear();
                    timeFitter.PushedFitCourses(timeFitter.generateDefaultCourses(), timeFitter.generateDefaultTimeTable(), 30);
                    break;
                case "Squeezed":
                    timeFitter.GeneratedTables.Clear();
                    timeFitter.SqueezedFitCourses(timeFitter.generateDefaultCourses(), timeFitter.generateDefaultTimeTable(), 30);
                    break;
                default:
                    timeFitter.GeneratedTables.Clear();
                    timeFitter.NormalFitCourses(timeFitter.generateDefaultCourses(), timeFitter.generateDefaultTimeTable());
                    break;
            }
            return timeFitter.GeneratedTables.ToList();
        }
    }
}
