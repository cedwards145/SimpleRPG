using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleRPG
{
    /// <summary>
    /// Static class that performs clustering
    /// </summary>
    public static class Clusterer
    {
        /// <summary>
        /// Given a list of points, return an array of clusters, with each point in the list in exactly one of the clusters
        /// </summary>
        /// <param name="points">A list of points to be clustered</param>
        /// <returns>An array of clusters, represented as lists of Points</returns>
        public static List<Point>[] cluster(List<Point> points)
        {
            int minX = points[0].X;
            int maxX = points[0].X;
            int minY = points[0].Y;
            int maxY = points[0].Y;
            
            for (int index = 1; index < points.Count; index++)
            {
                minX = (points[index].X < minX ? points[index].X : minX);
                minY = (points[index].Y < minY ? points[index].Y : minY);
                maxX = (points[index].X > maxX ? points[index].X : maxX);
                maxY = (points[index].Y > maxY ? points[index].Y : maxY);
            }

            // Will contain the results from a run of the algorithm
            List<Point> previousFirstCluster;
            List<Point> previousSecondCluster;

            // Working lists during the algorithm
            List<Point> currentFirstCluster = new List<Point>();
            List<Point> currentSecondCluster = new List<Point>();

            Vector2 firstCenter = new Vector2();
            Vector2 secondCenter = new Vector2();

            Random random = Utilities.getRandom();

            do
            {

                // Initialize 2 cluster centers randomly
                firstCenter.X = (float)(minX + random.NextDouble() * (maxX - minX));
                firstCenter.Y = (float)(minY + random.NextDouble() * (maxY - minY));
                do
                {
                    secondCenter.X = (float)(minX + random.NextDouble() * (maxX - minX));
                    secondCenter.Y = (float)(minY + random.NextDouble() * (maxY - minY));
                } while (firstCenter.Equals(secondCenter));

                do
                {
                    previousFirstCluster = new List<Point>(currentFirstCluster);
                    previousSecondCluster = new List<Point>(currentSecondCluster);

                    currentFirstCluster.Clear();
                    currentSecondCluster.Clear();

                    double distanceFromFirst, distanceFromSecond;
                    Point currentPoint;
                    for (int index = 0; index < points.Count; index++)
                    {
                        currentPoint = points[index];

                        // Calculate the distance between a point and each of the centers
                        distanceFromFirst = distanceBetween(currentPoint, firstCenter);
                        distanceFromSecond = distanceBetween(currentPoint, secondCenter);

                        // Assign point to correct cluster
                        if (distanceFromFirst < distanceFromSecond)
                            currentFirstCluster.Add(currentPoint);
                        else
                            currentSecondCluster.Add(currentPoint);
                    }

                    // Recalculate center coordinates
                    firstCenter = calculateCenter(currentFirstCluster);
                    secondCenter = calculateCenter(currentSecondCluster);
                }
                // Loop algorithm until clusters are stable
                while (!(listsEqual(currentFirstCluster, previousFirstCluster)
                         || listsEqual(currentSecondCluster, previousSecondCluster)));

            } while (currentFirstCluster.Count == 0 || currentSecondCluster.Count == 0);

            return new List<Point>[] { currentFirstCluster, currentSecondCluster };
        }

        private static double distanceBetween(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        private static double distanceBetween(Vector2 p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        private static double distanceBetween(Point p1, Vector2 p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
        private static double distanceBetween(Vector2 p1, Vector2 p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        private static Vector2 calculateCenter(List<Point> points)
        {
            float totalX = 0, totalY = 0;
            foreach (Point p in points)
            {
                totalX += p.X;
                totalY += p.Y;
            }

            return new Vector2(totalX / points.Count, totalY / points.Count);
        }

        private static bool listsEqual(List<Point> l1, List<Point> l2)
        {
            // Check if l2 contains all elements of l1
            bool allList1InList2 = true;
            foreach (Point p in l1)
                if (!l2.Contains(p))
                    allList1InList2 = false;

            bool allList2InList1 = true;
            foreach (Point p in l2)
                if (!l1.Contains(p))
                    allList2InList1 = false;
            return allList2InList1 && allList1InList2;
        }
    }
}
