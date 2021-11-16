using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmeans
{
    class PointsDistances
    {
        public Point Point { get; set; }
        public Point Point2 { get; set; }
        public double Distance { get; set; }

        public PointsDistances(Point point, Point point2, double distance)
        {
            Point = point;
            Point2 = point2;
            Distance = distance;
        }
    }
}
