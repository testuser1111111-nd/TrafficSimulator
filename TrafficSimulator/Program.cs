global using System;
global using System.Collections;
using System.Net;
using System.Text.RegularExpressions;

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
        static double[] currentCosts;
        static List<(long,long)>[] roadsDesc2;
        static (long from, long to)[] roadsDesc;
        static (double a, double b)[] roadsTime;
        static long[] roadCarCount;
        static long[] carPos;
        static (long startpoint,double starttime, long end)[] carsDesc;
        static double[] carScore;
        static bool[] carstarted;
        static void Main(string[] args)
        {
            Initialize();
            while(priorityQueue.Count > 1)
            {
                long @event;
                double priority;
                priorityQueue.TryDequeue(out @event,out priority);
                if(@event == -1)
                {
                    double nexttime;
                    priorityQueue.TryPeek(out _, out nexttime);
                    UpdateNext();
                    priorityQueue.Enqueue(-1, priority + Math.Max(1,Math.Ceiling((nexttime-priority)/roadevaltime))*roadevaltime);
                }
                else
                {
                    if (carstarted[@event])
                    {
                        var prevroute = carPos[@event];
                        var currentpoint = roadsDesc[prevroute].to;
                        roadCarCount[prevroute]--;
                        if (currentpoint == carsDesc[@event].end)
                        {
                            carScore[@event] = priority - carsDesc[@event].starttime;
                        }
                        else
                        {
                            var nextroute = nextMove[currentpoint, carsDesc[@event].end];
                            roadCarCount[nextroute]++;
                            carPos[@event] = nextroute;
                            priorityQueue.Enqueue(@event, priority + roadsTime[nextroute].a * roadCarCount[nextroute] + roadsTime[nextroute].b);
                        }
                    }
                    else
                    {
                        var startpoint = carsDesc[@event].startpoint;
                        var endpoint = carsDesc[@event].end;
                        var nextroute = nextMove[startpoint, endpoint];
                        roadCarCount[nextroute]++;
                        carPos[@event] = nextroute;
                        carstarted[@event] = true;
                        priorityQueue.Enqueue(@event, priority + roadsTime[nextroute].a * roadCarCount[nextroute] + roadsTime[nextroute].b);
                    }
                }

            }
            List<string[]> exportdata = new List<string[]>();
            foreach(var score in carScore)
            {
                string[] a = new string[1];
                a[0] = score.ToString();
                exportdata.Add(a);
            }
            ExportCSV(exportdata);
        }
        static void Initialize()
        {
            long[] input = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
            points = input[0];
            roads = input[1];
            cars = input[2];
            priorityQueue = new PriorityQueue<long, double>();
            priorityQueue.Enqueue(-1, -1);
            roadevaltime = double.Parse(Console.ReadLine());
            roadsTime = new (double a, double b)[roads];
            roadsDesc = new (long, long)[roads];
            roadCarCount = new long[roads];
            roadsDesc2 = new List<(long,long)>[points];
            carScore = new double[cars];
            for(int i = 0;i < points; i++)
            {
                roadsDesc2[i] = new List<(long, long)>();
            }
            carsDesc = new (long, double, long)[cars];
            carPos = new long[cars];
            for(int i = 0;i < cars; i++)
            {
                carPos[i] = -1;
            }
            carstarted = new bool[cars];
            nextMove = new long[points, points];
            for (int i = 0; i < roads; i++)
            {
                long[] input2 = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
                roadsDesc[i].from = input2[0];
                roadsDesc[i].to = input2[1];
                roadsDesc2[roadsDesc[i].from].Add((roadsDesc[i].to, i));
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
        static void UpdateNext()
        {
            currentCosts = new double[roads];
            for (int i = 0; i < roads; i++)
            {
                currentCosts[i] = roadsTime[i].a * roadCarCount[i] + roadsTime[i].b;
            }
            for(int i = 0;i < points; i++)
            {
                long[] nextpaths = new long[points];
                double[] costs = new double[points];
                nextpaths[i] = -1;
                PriorityQueue<long, double> pq = new PriorityQueue<long, double>();
                for(int j = 0;j < points; j++)
                {
                    if (i != j)
                    {
                        pq.Enqueue(j, double.MaxValue);
                    }
                    else
                    {
                        pq.Enqueue(j, 0);
                    }
                }
                while(pq.Count > 0)
                {
                    var p = pq.Dequeue();
                    foreach(var next in roadsDesc2[p])
                    {
                        var q = next.Item1;
                        var road = next.Item2;
                        var alt = costs[p] + currentCosts[road];
                        if (costs[q] > alt)
                        {
                            costs[q] = alt;
                            pq.Enqueue(q, alt);
                            if(p == i)
                            {
                                nextpaths[q] = road;
                            }
                            else
                            {
                                nextpaths[q] = nextpaths[p];
                            }
                        }
                    }
                }
                for(int j = 0;j < points; j++)
                {
                    nextMove[i, j] = nextpaths[j];
                }
            }
        }
        
        static string ExportCSV(IEnumerable<string[]>? Keys, string PathOfKey = "Keys.csv", string BackupName = "backup.csv")
        {
            try
            {
                string temp = string.Empty;
                if (Keys == null || Keys.Count() == 0)
                {
                    //Console.WriteLine("No key in list. deleting the keyfile contents");
                }
                else
                {
                    foreach (string[] Key in Keys)
                    {
                        foreach (string key in Key)
                        {
                            var rgxquote = new Regex("\"");
                            rgxquote.Replace(key, "\"\"");
                            temp += "\"" + key + "\"" + ',';
                        }
                        temp = temp.Substring(0, temp.Length - 1);
                        temp += '\n';
                    }
                }
                var keys = File.Open(PathOfKey, FileMode.OpenOrCreate);
                var bkup = File.Open(BackupName, FileMode.OpenOrCreate);
                keys.Dispose();
                bkup.Dispose();
                File.Copy(PathOfKey, BackupName, true);
                File.WriteAllText(PathOfKey, temp);
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
