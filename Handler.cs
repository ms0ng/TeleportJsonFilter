using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Handler
    {
        #region Instance
        public static Handler Instance
        {
            get
            {
                if (instance == null) instance = new Handler();
                return instance;
            }
        }
        private static Handler instance;
        #endregion
        #region Params
        float pickupRange = 15;
        float step = 1000;
        #endregion
        public List<Vector3> AllPoints => GetAllPoints();
        List<Vector3> allPoints;
        List<List<Vector3>> pointsInRange;
        bool pointChanged;


        private Handler()
        {
            allPoints = new List<Vector3>();
            pointsInRange = new List<List<Vector3>>();
            pointChanged = true;
        }

        public void SetParams(float pickupRange = 10, float step = 1000)
        {
            this.pickupRange = pickupRange;
            this.step = step;
        }

        public void AddPoint(Vector3 point, out bool success)
        {
            int rangeIndex = (int)(point.Magnitude / step);
            while (pointsInRange.Count <= rangeIndex) { pointsInRange.Add(new List<Vector3>()); }

            foreach (var item in pointsInRange[rangeIndex])
            {
                if (item.GetDisdanceSqrt(point) > pickupRange) continue;
                Console.WriteLine($"点{point}距离另一个点{item}太近（{item.GetDisdanceSqrt(point)}m）");
                success = false;
                return;
            }
            pointsInRange[rangeIndex].Add(point);
            pointChanged = true;
            success = true;
            //Console.WriteLine($"点{point}距离原点{(rangeIndex + 1) * step}m内（{point.Magnitude}m）");
        }

        private List<Vector3> GetAllPoints()
        {
            if (!pointChanged) return allPoints;
            allPoints.Clear();
            foreach (var points in pointsInRange)
            {
                allPoints.AddRange(points);
            }
            pointChanged = false;
            return allPoints;
        }
    }
}
