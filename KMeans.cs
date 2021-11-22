using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace kmeans
{
    class KMeans
    {
        List<Point> points;
        List<Centroid> centroids;
        private int maxCoord;
        private int numberOfThreads;

        public KMeans(List<Point> points, List<Centroid> centroids, int maxCoord, int numberOfThreads)
        {
            this.points = points;
            this.centroids = centroids;
            this.maxCoord = maxCoord;
            this.numberOfThreads = numberOfThreads;
        }

        public void RunKMeans()
        {
            new Visualization(points, centroids, maxCoord).CreateImage("generated"); // with a lot of data this takes a lot time

            GetCentroidPoints();

            while (CentroidsChanged())
            {
                CalculateNewCentroids();
                GetCentroidPoints();
            }

            new Visualization(points, centroids, maxCoord, true).CreateImage("final"); // with a lot of data this takes a lot time
        }
        private void CalculateNewCentroids()
        {
            for (int i = 0; i < centroids.Count; i++)
            {
                centroids[i].CalculateNewPosition();
                centroids[i].ClearPoints();
            }
        }
        private bool CentroidsChanged()
        {
            bool changed = false;
            for (int i = 0; i < centroids.Count; i++)
            {
                changed = centroids[i].NeedNewPosition();
            }

            return changed;
        }
        private void GetCentroidPoints()
        {
            int[] indexes = new int[numberOfThreads + 1];

            for (int i = 0; i < numberOfThreads; i++)
            {
                indexes[i] = points.Count / numberOfThreads * i;
            }

            indexes[numberOfThreads] = points.Count;
            Thread[] threads = new Thread[numberOfThreads];

            for (int i = 0; i < numberOfThreads; i++)
            {
                int from = indexes[i];
                int to = indexes[i + 1];

                threads[i] = new Thread(() => ComputeThread(from, to));
                threads[i].Start();
            }
            for (int i = 0; i < numberOfThreads; i++)
            {
                threads[i].Join();
            }
        }
        private void ComputeThread(int from, int to)
        {
            for (int i = from; i < to; i++)
            {
                int closestCentroidIndex = 0;
                double shortestDistance = Math.Sqrt(Math.Pow(points[i].X - centroids[0].X, 2) + Math.Pow(points[i].Y - centroids[0].Y, 2));

                for (int j = 0; j < centroids.Count; j++)
                {
                    double distance = Math.Sqrt(Math.Pow(points[i].X - centroids[j].X, 2) + Math.Pow(points[i].Y - centroids[j].Y, 2));

                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        closestCentroidIndex = j;
                    }
                }

                centroids[closestCentroidIndex].AddClosestPoint(points[i]);
            }
        }
    }
}
