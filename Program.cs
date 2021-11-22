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
            int numberOfPoints = 10000000;
            int maxCoord = 3000;
            Generator _generator = new Generator(numberOfPoints, maxCoord);
            List<Point> points = _generator.Generate();
            Console.WriteLine("\tDone!");

            // Clustering
            Console.Write("Start clustering...");
            Stopwatch sw = new Stopwatch();
            sw.Start();

            List<Centroid> centroids = new List<Centroid>();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            // generate random centroids
            int numberOfClusters = 3;
            for (int i = 0; i < numberOfClusters; i++)
            {
                double x = rnd.NextDouble() * ((maxCoord + 1) - 1) + 1;
                double y = rnd.NextDouble() * ((maxCoord + 1) - 1) + 1;

                centroids.Add(new Centroid(x, y));
            }

            int numberOfThreads = 12; // ryzen powa :-D
            KMeans _kMeans = new KMeans(points, centroids, maxCoord, numberOfThreads);
            _kMeans.RunKMeans();

            sw.Stop();
            Console.WriteLine("\tDone! Total time: " + sw.Elapsed);
        }
    }
}
