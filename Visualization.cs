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
        private List<Centroid> centroids;
        private Bitmap bitmap;
        private bool colorful;

        public Visualization(List<Point> points, List<Centroid> centroids, int maxCoord, bool colorful = false)
        {
            this.points = points;
            this.centroids = centroids;
            this.colorful = colorful;
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
            if (!colorful) // generated points.. all white
            {
                for (int i = 0; i < points.Count; i++)
                {
                    bitmap.SetPixel((int)points[i].X, (int)points[i].Y, Color.White);
                }
            }
            else // final clusters with colors
            {
                for (int i = 0; i < centroids.Count; i++)
                {
                    Colorful(centroids[i]);
                }
            }

            //higlight centroids
            for (int i = 0; i < centroids.Count; i++)
            {
                //bitmap.SetPixel((int)centroids[i].X, (int)centroids[i].Y, Color.Red);
                //bitmap.SetPixel((int)centroids[i].X + 1, (int)centroids[i].Y + 1, Color.Red);
                //bitmap.SetPixel((int)centroids[i].X + 1, (int)centroids[i].Y - 1, Color.Red);
                //bitmap.SetPixel((int)centroids[i].X - 1, (int)centroids[i].Y - 1, Color.Red);
                //bitmap.SetPixel((int)centroids[i].X - 1, (int)centroids[i].Y + 1, Color.Red);

                for (int a = 0; a < 20; a++)
                {
                    for (int b = 0; b < 20; b++)
                    {
                        int x = (int)centroids[i].X + a - 10;
                        int y = (int)centroids[i].Y + b - 10;

                        bitmap.SetPixel(x, y, Color.Red);
                    }
                }
            }

            bitmap.Save(name + ".jpg");
        }

        private void Colorful(Centroid centroid)
        {
            List<Point> centroidPoints = centroid.GetClosestPoints();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

            for (int i = 0; i < centroidPoints.Count; i++)
            {
                bitmap.SetPixel((int)centroidPoints[i].X, (int)centroidPoints[i].Y, randomColor);
            }
        }
    }
}
