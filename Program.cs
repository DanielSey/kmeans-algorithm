using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace kmeans
{
    class Program
    {
        static void Main(string[] args)
        {
            // generating
            Console.Write("Generating data... ");
            int numberOfPoints = 20;
            int maxCoord = 100;
            Generator _generator = new Generator(numberOfPoints, maxCoord);
            List<Point> points = _generator.Generate();
            Console.WriteLine("\tDone!");

            // clustering
            Console.Write("Start clustering...");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            int numberOfClusters = 3;
            KMeans _kMeans = new KMeans(numberOfClusters, points, maxCoord);
            //List<Point> lastCentroids = _kMeans.GetLastCentroids();
            //new Visualization(points, maxCoord, lastCentroids).CreateImage("generated");

            _kMeans.Clustering();

            sw.Stop();
            Console.WriteLine("\tDone! Total time: " + sw.Elapsed + Environment.NewLine);

            _kMeans.PrintClusters();
            //lastCentroids = _kMeans.GetLastCentroids();
            //new Visualization(points, maxCoord, lastCentroids, true).CreateImage("final");
        }
    }
}
