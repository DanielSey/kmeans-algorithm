using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmeans
{
    class KMeans
    {
        private int numberOfClusters;
        private int numberOfPoints;
        private int maxCoord;
        private List<Point> points;
        private List<PointsDistances> distances;

        public KMeans(int numberOfClusters, int numberOfPoints, List<Point> points, int maxCoord)
        {
            this.numberOfClusters = numberOfClusters;
            this.numberOfPoints = numberOfPoints;
            this.points = points;
            this.maxCoord = maxCoord;
            CreateDistanceMatrix();
        }

        private void CreateDistanceMatrix()
        {
            distances = new List<PointsDistances>();

            for (int i = 0; i < numberOfPoints; i++)
            {
                for (int j = i; j < numberOfPoints; j++) // j = i
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
            int stopCount = numberOfPoints;

            while (stopCount > numberOfClusters) {
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

            // remove all new points
            for (int i = points.Count - 1; i >= 0; i--)
            {
                if (points[i].NewPoint)
                {
                    points.RemoveAt(i);
                }
            }
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
