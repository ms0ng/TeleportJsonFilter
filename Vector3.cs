using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Vector3
    {
        private float x; private float y; private float z;

        public float X { get => x; set => x = value; }
        public float Y { get => y; set => y = value; }
        public float Z { get => z; set => z = value; }

        public float Magnitude => GetMagnitude();
        public float MagnitudeSqrt => GetMagnitudeSqrt();

        public Vector3()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        private float GetMagnitudeSqrt()
        {
            return x * x + y * y + z * z;
        }

        private float GetMagnitude()
        {
            return (float)Math.Sqrt(GetMagnitudeSqrt());
        }

        public float GetDisdanceSqrt(Vector3 vector3)
        {
            return (vector3 - this).GetMagnitudeSqrt();
        }
        public float GetDisdance(Vector3 vector3)
        {
            return (vector3 - this).GetMagnitude();
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v2.x - v1.x, v2.y - v1.y, v2.z - v1.z);
        }

        public override string ToString()
        {
            return $"({x},{y},{z})";
        }
    }
}
