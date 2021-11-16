using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmeans
{
    class Generator
    {
        private int size;
        private List<Point> points;

        public Generator(int size)
        {
            this.size = size;
        }

        public List<Point> Generate()
        {
            points = new List<Point>();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < size; i++)
            {
                int x = rnd.Next(1, 101); // random between 1 and 100
                int y = rnd.Next(1, 101);
                points.Add(new Point((i + 1), x, y));
            }

            return points;
        }

        public void PrintGeneratedPoints()
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
