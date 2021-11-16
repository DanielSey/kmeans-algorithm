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
            Stopwatch sw = new Stopwatch();

            // generating
            Console.Write("Generating data...");
            int size = 500;
            Generator _generator = new Generator(size);
            List<Point> points = _generator.Generate();
            //_generator.PrintGeneratedPoints();
            Console.WriteLine("\tDone!");


            // clustering
            Console.WriteLine("Start clustering..." + Environment.NewLine);
            sw.Start();

            int clusters = 5;
            KMeans _kMeans = new KMeans(clusters, size, points);
            //_kMeans.PrintDistanceMatrix();
            _kMeans.Clustering();

            sw.Stop();
            Console.WriteLine("Total time: {0}", sw.Elapsed);
        }
    }
}
