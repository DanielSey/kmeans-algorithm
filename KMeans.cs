using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmeans
{
    class KMeans
    {
        private int maxClusters;
        private int size;
        private List<Point> points;
        private List<PointsDistances> distances;

        public KMeans(int maxClusters, int size, List<Point> points)
        {
            this.maxClusters = maxClusters;
            this.size = size;
            this.points = points;
            CreateDistanceMatrix();
        }

        private void CreateDistanceMatrix()
        {
            distances = new List<PointsDistances>();

            for (int i = 0; i < size; i++)
            {
                for (int j = i; j < size; j++) // j = i
                {
                    double p_distance = Distance(points[i].X, points[i].Y, points[j].X, points[j].Y);

                    if (p_distance != 0)
                    {
                        distances.Add(new PointsDistances(points[i], points[j], p_distance));
                    }
                }
            }
        }

        public void Clustering()
        {
            int cluster = 1;
            int id = points.Count;
            int stopCount = size;

            while (stopCount > maxClusters) {
                // find minimal distance
                PointsDistances minDistance = FindMin();
                //Console.WriteLine("Found min. distance: " + minDistance.Distance + Environment.NewLine);  // delete after

                // create new point 
                int x = (minDistance.Point.X + minDistance.Point2.X) / 2;
                int y = (minDistance.Point.Y + minDistance.Point2.Y) / 2;
                Point newPoint = new Point(++id, x, y, cluster, false, true);

                int a = minDistance.Point.Cluster;
                int b = minDistance.Point2.Cluster;

                // assign new cluster
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].Id == minDistance.Point.Id || points[i].Id == minDistance.Point2.Id)
                    {
                        //Console.WriteLine("assign new cluster (from: "+points[i].Cluster+" to: "+cluster+"), id: " + points[i].Id);
                        points[i].Cluster = cluster;
                    }
                }

                // merge clusters
                if (minDistance.Point.NewPoint || minDistance.Point2.NewPoint)
                {
                    //Console.WriteLine("a: " + a);
                    //Console.WriteLine("b: " + b);
                    for (int i = 0; i < points.Count; i++)
                    {
                        if ((points[i].Cluster == a || points[i].Cluster == b) && points[i].Cluster != 0)
                        {
                            //Console.WriteLine("update cluster (from: " + points[i].Cluster + " to: " + cluster + "), id: " + points[i].Id);
                            points[i].Cluster = cluster;
                        }
                    }
                }
                //Console.WriteLine();

                // remove min distance points
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].Id == minDistance.Point.Id || points[i].Id == minDistance.Point2.Id)
                    {
                        points[i].Deleted = true;
                    }
                }

                // add new point into list
                points.Add(newPoint);

                // compute new distances
                //PrintPoints(); // delete after
                distances.Clear();

                for (int i = 0; i < points.Count; i++)
                {
                    for (int j = i; j < points.Count; j++)
                    {
                        if (!points[i].Deleted && !points[j].Deleted) // skip deleted values
                        {
                            double p_distance = Distance(points[i].X, points[i].Y, points[j].X, points[j].Y);

                            if (p_distance != 0) // skip zero values
                            {
                                distances.Add(new PointsDistances(points[i], points[j], p_distance));
                            }
                        }
                    }
                }

                //PrintDistanceMatrix(); // delete after
                cluster++;
                stopCount--;
            }

            PrintClusters();
        }

        private PointsDistances FindMin()
        {
            double min = 0;
            PointsDistances pointsDistances = null;

            for (int i = 0; i < distances.Count; i++)
            {
                if (i == 0)
                {
                    min = distances[i].Distance;
                    pointsDistances = distances[i];
                    continue;
                }

                if (distances[i].Distance < min)
                {
                    min = distances[i].Distance;
                    pointsDistances = distances[i];
                }
            }

            //foreach (var val in distances)
            //{
            //    if (val.Distance < min)
            //    {
            //        min = val.Distance;
            //    }
            //}

            return pointsDistances;
        }
        private double Distance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
            //return Math.Sqrt(((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2)));
        }

        public void PrintDistanceMatrix()
        {
            Console.WriteLine("----- Distance matrix - reduced (count: " + distances.Count + ") -----");

            for (int i = 0; i < distances.Count; i++)
            {
                Console.WriteLine("Points " + distances[i].Point.Id + ", " + distances[i].Point2.Id + ": distance: " + Math.Round(distances[i].Distance, 0));
            }

            Console.WriteLine();
        }

        public void PrintPoints()
        {
            Console.WriteLine("----- List of points (count: " + points.Count + ") -----");

            for (int i = 0; i < points.Count; i++)
            {
                Console.WriteLine("Point " + points[i].Id + ", X: " + points[i].X + ", Y: " + points[i].Y + ", cluster: " + points[i].Cluster + ", deleted: " + points[i].Deleted + ", newPoint: " + points[i].NewPoint);
            }

            Console.WriteLine();
        }

        public void PrintClusters()
        {
            Console.WriteLine("----- Clusters (count: " + maxClusters + ") -----");

            //https://www.geeksforgeeks.org/counting-frequencies-of-array-elements/

            Dictionary<int, int> mp = new Dictionary<int, int>();

            // Traverse through array elements and
            // count frequencies

            for (int i = 0; i < size; i++)
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

            // To print elements according to first
            // occurrence, traverse array one more time
            // print frequencies of elements and mark
            // frequencies as -1 so that same element
            // is not printed multiple times.
            for (int i = 0; i < size; i++)
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

        public void TestDistance() // just debug method
        {
            Console.Write("enter x1: ");
            int x1 = Convert.ToInt32(Console.ReadLine());
            Console.Write("enter x2: ");
            int x2 = Convert.ToInt32(Console.ReadLine());
            Console.Write("enter y1: ");
            int y1 = Convert.ToInt32(Console.ReadLine());
            Console.Write("enter y2: ");
            int y2 = Convert.ToInt32(Console.ReadLine());
            double distance = Distance(x1, y1, x2, y2);
            Console.WriteLine("distance: " + Math.Round(distance, 0));
        }
    }
}
