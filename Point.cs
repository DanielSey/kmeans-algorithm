using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmeans
{
    class Point
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Cluster { get; set; }
        public bool Centroid { get; set; }
        public bool NewPoint { get; set; }

        public Point (int id, int x, int y, int cluster = 0, bool centroid = false, bool newPoint = false)
        {
            Id = id;
            X = x;
            Y = y;
            Cluster = cluster;
            Centroid = centroid;
            NewPoint = newPoint;
        }
    }
}
