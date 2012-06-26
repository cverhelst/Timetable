using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            TimetableFitter timeFitter = new TimetableFitter();
            timeFitter.FitCourses(timeFitter.generateDefaultCourses(), timeFitter.generateDefaultTimeTable());
            //Console.Out.Write(timeFitter.GeneratedTables.ToString());
            Console.In.ReadLine();
        }
    }
}
