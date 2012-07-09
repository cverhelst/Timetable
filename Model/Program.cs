﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    class Program
    {
        static void Main(string[] args)
        {
            TimetableFitter timeFitter = new TimetableFitter();
            timeFitter.FitCourses(timeFitter.generateDefaultCourses(), timeFitter.generateDefaultTimeTable());
            Console.BufferWidth = 250;
            foreach (Timetable table in timeFitter.GeneratedTables)
            {
                Console.Out.Write(table.ToString());
                Logger.Log(table.ToString());
            }
            
            Console.Out.WriteLine("Please press enter to continue");
            Console.In.ReadLine();
        }
    }
}
