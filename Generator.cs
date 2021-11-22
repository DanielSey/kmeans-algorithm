using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmeans
{
    class Generator
    {
        private int numberOfPoints;
        private int maxCoord;
        private List<Point> points;

        public Generator(int numberOfPoints, int maxCoord)
        {
            this.numberOfPoints = numberOfPoints;
            this.maxCoord = maxCoord;
        }

        public List<Point> Generate()
        {
            points = new List<Point>();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            for (int i = 0; i < numberOfPoints; i++)
            {
                double x = rnd.NextDouble() * ((maxCoord + 1) - 1) + 1;
                double y = rnd.NextDouble() * ((maxCoord + 1) - 1) + 1;

                points.Add(new Point((i + 1), x, y));
            }

            return points;
        }
    }
}
