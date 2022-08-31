global using System;
global using System.Collections;
namespace TrafficSimulator
{
    static class Program
    {
        static (double x, double y)[] pointCors;
        static (long from, long to)[] roadsDesc;
        static long[] roadCarCount;
        static long[] carPos;
        static (long start, long end)[] carsDest;

        static void Main(string[] args)
        {
            long[] input = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
            long points = input[0];
            long roads = input[1];
            long cars = input[2];
            pointCors = new (double, double)[points];
            roadsDesc = new (long, long)[points];
            roadCarCount = new long[roads];
            carsDest = new (long, long)[cars];
            carPos = new long[points];
            for(long i = 0;i < points; i++)
            {
                double[] input2 = Console.ReadLine().Split(' ').Select(double.Parse).ToArray();
                pointCors[i].x = input2[0];
                pointCors[i].y = input2[1];
            }
            for (int i = 0; i < roads; i++)
            {
                long[] input2 = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
                roadsDesc[i].from = input2[0];
                roadsDesc[i].to = input2[1];
            }
            for (int i = 0; i < cars; i++)
            {
                long[] input2 = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
                carsDest[i].start = input2[0];
                carsDest[i].end = input2[1];
            }

        }
    }
}
