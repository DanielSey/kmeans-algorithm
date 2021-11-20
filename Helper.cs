using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmeans
{
    static class Helper
    {
        public static void PrintPoints(List<Point> points)
        {
            Console.WriteLine("----- List of points (count: " + points.Count + ") -----");

            for (int i = 0; i < points.Count; i++)
            {
                Console.WriteLine("Point " + points[i].Id + ", X: " + points[i].X + ", Y: " + points[i].Y + ", cluster: " + points[i].Cluster + ", centroid: " + points[i].Centroid + ", newPoint: " + points[i].NewPoint);
            }

            Console.WriteLine();
        }
    }
}
