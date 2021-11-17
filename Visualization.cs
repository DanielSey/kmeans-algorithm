using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace kmeans
{
    class Visualization
    {
        private List<Point> points;
        private Bitmap bitmap;

        public Visualization(List<Point> points, int maxCoord)
        {
            this.points = points;
            bitmap = new Bitmap(maxCoord + 5, maxCoord + 5);
            FillBackground();
        }
        private void FillBackground()
        {
            for (var x = 0; x < bitmap.Width; x++)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, Color.Black);
                }
            }
        }

        public void CreateImage(string name)
        {
            List<int> finalClusters = CountClusters();
            Console.WriteLine(finalClusters.Count);
            if (finalClusters.Count <= 1)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    bitmap.SetPixel(points[j].X, points[j].Y, Color.White);
                }
            }
            else
            {
                for (int i = 0; i < finalClusters.Count; i++)
                {
                    Random rnd = new Random(Guid.NewGuid().GetHashCode());
                    Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

                    for (int j = 0; j < points.Count; j++)
                    {
                        if (finalClusters[i] == points[j].Cluster)
                        {
                            bitmap.SetPixel(points[j].X, points[j].Y, randomColor);
                        }
                    }
                }
            }

            bitmap.Save(name + ".jpg");
        }
        private List<int> CountClusters()
        {
            // https://www.geeksforgeeks.org/counting-frequencies-of-array-elements/

            List<int> finalClusters = new List<int>();
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
                    finalClusters.Add(points[i].Cluster); // for visualization
                    mp.Remove(points[i].Cluster);
                    mp.Add(points[i].Cluster, -1);
                }
            }

            return finalClusters;
        }
    }
}
