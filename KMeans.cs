using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmeans
{
    class KMeans
    {
        private int numberOfClusters;
        private List<Point> points;
        private List<Point> centroids;

        public KMeans(int numberOfClusters, List<Point> points, int maxCoord)
        {
            this.numberOfClusters = numberOfClusters;
            this.points = points;
            SetCentroids();
            //Helper.PrintPoints(points);
        }

        private void SetCentroids()
        {
            int cluster = 1;
            centroids = new List<Point>();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            List<int> selected = new List<int>();

            while (selected.Count < numberOfClusters)
            {
                int index = rnd.Next(points.Count);

                if (!selected.Contains(index))
                {
                    //Console.WriteLine("set centroid: " + cluster);
                    selected.Add(index);
                    points[index].Centroid = true;
                    points[index].Cluster = cluster;
                    centroids.Add(points[index]);
                    cluster++;
                }
            }
        }

        public void Clustering()
        {
            int id = points.Count;
            int stop = 0;
            const int MAX = 10;

            while (stop < MAX)
            {
                Console.WriteLine("iteration: " + stop);

                // assign new cluster for all points
                for (int i = 0; i < points.Count; i++)
                {
                    if (!points[i].Centroid) // skip centroids
                    {
                        // find closest centroid
                        Point closestCentroid = FindClosestCentroid(points[i]);

                        // assign new cluster
                        points[i].Cluster = closestCentroid.Cluster;
                        //Console.WriteLine("point "+(i+1)+" Cluster: " + points[i].Cluster);
                    }
                }
                //Console.WriteLine("assign clusters");
                //Helper.PrintPoints(points);

                // remove actual centroids
                if (stop == 0) // run only once (need reset, then are created new points by x,y mean)
                {
                    for (int i = 0; i < centroids.Count; i++)
                    {
                        centroids[i].Centroid = false;
                    }
                }
                if ((stop + 1) == MAX) // last loop, skip deleting centroids list into List of points for visualization
                {
                    break;
                }
                centroids.Clear();

                // compute mean of coords of points per cluster to create new centroids
                for (int i = 0; i < numberOfClusters; i++)
                {
                    int x = 0;
                    int y = 0;
                    int count = 0;
                    int cluster = i + 1;

                    for (int j = 0; j < points.Count; j++)
                    {
                        if (cluster == points[j].Cluster)
                        {
                            x += points[j].X;
                            y += points[j].Y;
                            count++;
                        }
                    }

                    if (count != 0)
                    {
                        //Console.WriteLine("wtf");
                        x /= count;
                        y /= count;
                    }

                    centroids.Add(new Point(++id, x, y, cluster, true, true));

                    //Console.WriteLine("set new centroid");
                }
                //break;
                stop++;
            }
        }

        private Point FindClosestCentroid(Point point)
        {
            List<PointsDistances> distances = new List<PointsDistances>();
            
            for (int i = 0; i < centroids.Count; i++)
            {
                distances.Add(new PointsDistances(point, centroids[i], Distance(point.X, point.Y, centroids[i].X, centroids[i].Y)));
            }

            // find min distance with index
            double min = distances[0].Distance;
            Point minPoint = distances[0].Point2;
            //int minIndex = 0;

            for (int i = 1; i < distances.Count; ++i)
            {
                if (distances[i].Distance < min)
                {
                    min = distances[i].Distance;
                    minPoint = distances[i].Point2;
                    //minIndex = i;
                }
            }

            //for (int i = 0; i < distances.Count; i++) // print
            //{
            //    Console.WriteLine("distances: " + distances[i].Distance);
            //}
            //Console.WriteLine("min: " + min + ", point: " + minPoint.Id);

            return minPoint;
        }

        public List<Point> GetLastCentroids()
        {
            return centroids;
        }

        private double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }

        public void PrintClusters()
        {
            Console.WriteLine("----- Clusters (count: " + numberOfClusters + ") -----");

            //https://www.geeksforgeeks.org/counting-frequencies-of-array-elements/

            Dictionary<int, int> mp = new Dictionary<int, int>();

            for (int i = 0; i < points.Count; i++)
            {
                if (mp.ContainsKey(points[i].Cluster))
                {
                    var val = mp[points[i].Cluster];
                    mp.Remove(points[i].Cluster);
                    mp.Add(points[i].Cluster, val + 1);
                }
                else
                {
                    mp.Add(points[i].Cluster, 1);
                }
            }

            for (int i = 0; i < points.Count; i++)
            {
                if (mp.ContainsKey(points[i].Cluster) && mp[points[i].Cluster] != -1)
                {
                    Console.WriteLine("Cluster " + points[i].Cluster + ": " + mp[points[i].Cluster] + "x");
                    mp.Remove(points[i].Cluster);
                    mp.Add(points[i].Cluster, -1);
                }
            }

            Console.WriteLine();
        }
    }
}
