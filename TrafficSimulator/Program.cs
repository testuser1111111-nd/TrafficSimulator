global using System;
global using System.Collections;
namespace TrafficSimulator
{
    static class Program
    {
        static long points;
        static long roads;
        static long cars;
        static double roadevaltime;
        static PriorityQueue<long, double> priorityQueue;
        static long[,] nextMove;
        static Queue<long>[] roadque;
        static double currenttime;
        static (double x, double y)[] pointCors;
        static (long from, long to)[] roadsDesc;
        static (double a, double b)[] roadsTime;
        static long[] roadCarCount;
        static long[] carPos;
        static (long startpoint,double starttime, long end)[] carsDesc;

        static void Main(string[] args)
        {
            Initialize();
            while(priorityQueue.Count > 0)
            {

            }
        }
        static void Initialize()
        {
            long[] input = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
            points = input[0];
            roads = input[1];
            cars = input[2];
            currenttime = 0;
            priorityQueue = new PriorityQueue<long, double>();
            priorityQueue.Enqueue(-1, -1);
            roadevaltime = double.Parse(Console.ReadLine());
            roadsTime = new (double a, double b)[roads];
            pointCors = new (double, double)[points];
            roadsDesc = new (long, long)[roads];
            roadCarCount = new long[roads];
            carsDesc = new (long, double, long)[cars];
            carPos = new long[points];
            nextMove = new long[points, points];
            for (int i = 0; i < points; i++)
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
            for(int i = 0;i < roads; i++)
            {
                double[] input2 = Console.ReadLine().Split(' ').Select(double.Parse).ToArray();
                roadsTime[i].a = input2[0];
                roadsTime[i].b = input2[1];
            }
            for (int i = 0; i < cars; i++)
            {
                long[] input2 = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
                carsDesc[i].startpoint = input2[0];
                carsDesc[i].starttime = (double)input2[1];
                carsDesc[i].end = input2[2];
                priorityQueue.Enqueue(i, carsDesc[i].starttime);
            }
        }
    }
}
