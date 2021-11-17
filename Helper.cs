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
                Console.WriteLine("Point " + points[i].Id + ", X: " + points[i].X + ", Y: " + points[i].Y + ", cluster: " + points[i].Cluster + ", deleted: " + points[i].Deleted + ", newPoint: " + points[i].NewPoint);
            }

            Console.WriteLine();
        }
    }
}
