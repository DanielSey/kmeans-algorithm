using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace kmeans
{
    class Centroid
    {
        public double X { get; set; }
        public double Y { get; set; }

        private List<Point> closestPoints = new List<Point>();
        private List<Point> previousPoints = new List<Point>();

        public Centroid(double x, double y)
        {
            X = x;
            Y = y;
        }

        public List<Point> GetClosestPoints()
        {
            return closestPoints;
        }

        public void ClearPoints()
        {
            previousPoints = closestPoints;
            closestPoints.Clear();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddClosestPoint(Point kMeansPoint)
        {
            closestPoints.Add(kMeansPoint);
        }

        public void CalculateNewPosition()
        {
            double newX = 0;
            double newY = 0;

            for (int i = 0; i < closestPoints.Count; i++)
            {
                newX += closestPoints[i].X;
                newY += closestPoints[i].Y;
            }

            X = newX / closestPoints.Count;
            Y = newY / closestPoints.Count;
        }

        public bool NeedNewPosition()
        {
            if (previousPoints.Count != closestPoints.Count)
                return true;

            for (int i = 0; i < previousPoints.Count; i++)
            {
                if (previousPoints[i].X != closestPoints[i].X && previousPoints[i].Y != closestPoints[i].Y)
                    return true;
            }

            return false;
        }
    }
}
