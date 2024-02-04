using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace clock_scr
{
    public class MainViewModel
    {
        const double AngleDelta = 5.0;

        const double AngleRatio = 0.5;
        const double AxisXYLength = 50.0;

        public Transform3D CubeTransform { get; }

        public MatrixTransform3D matrixTransform = new MatrixTransform3D();

        public MainViewModel()
        {
            CubeTransform = InitializeCubeTransform();
        }

        public string Angle
        {
            get
            {
                return Matrix3DToString(matrixTransform.Matrix);
            }
            set
            {
                string[] matrixValues = value.Split(',');
                Matrix3D matrix = new Matrix3D(
                    Convert.ToDouble(matrixValues[0]), Convert.ToDouble(matrixValues[1]), Convert.ToDouble(matrixValues[2]), Convert.ToDouble(matrixValues[3]),
                    Convert.ToDouble(matrixValues[4]), Convert.ToDouble(matrixValues[5]), Convert.ToDouble(matrixValues[6]), Convert.ToDouble(matrixValues[7]),
                    Convert.ToDouble(matrixValues[8]), Convert.ToDouble(matrixValues[9]), Convert.ToDouble(matrixValues[10]), Convert.ToDouble(matrixValues[11]),
                    Convert.ToDouble(matrixValues[12]), Convert.ToDouble(matrixValues[13]), Convert.ToDouble(matrixValues[14]), Convert.ToDouble(matrixValues[15])
                );
                matrixTransform.Matrix = matrix;

            }
        }

        static string Matrix3DToString(Matrix3D matrix)
        {
            string[] elements = new string[16];

            elements[0] = matrix.M11.ToString();
            elements[1] = matrix.M12.ToString();
            elements[2] = matrix.M13.ToString();
            elements[3] = matrix.M14.ToString();

            elements[4] = matrix.M21.ToString();
            elements[5] = matrix.M22.ToString();
            elements[6] = matrix.M23.ToString();
            elements[7] = matrix.M24.ToString();

            elements[8] = matrix.M31.ToString();
            elements[9] = matrix.M32.ToString();
            elements[10] = matrix.M33.ToString();
            elements[11] = matrix.M34.ToString();

            elements[12] = matrix.OffsetX.ToString();
            elements[13] = matrix.OffsetY.ToString();
            elements[14] = matrix.OffsetZ.ToString();
            elements[15] = matrix.M44.ToString();

            return string.Join(",", elements);
        }
    
    Transform3D InitializeCubeTransform()
        {
            var transform = new Transform3DGroup();
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 0), 60)));
            transform.Children.Add(matrixTransform);
            return transform;
        }

        public void RotateDelta(Vector3D axis)
        {
            matrixTransform.Rotate(axis, AngleDelta);
        }

        public void RotateDelta(DeltaInfo info)
        {
            var delta = info.End - info.Start;
            var deltaLength = delta.Length;
            if (deltaLength == 0) return;

            var distance = GetDistance((Vector)info.Start, delta);
            delta *= AxisXYLength / deltaLength;
            matrixTransform.Rotate(new Vector3D(delta.Y, delta.X, distance), AngleRatio * deltaLength);


        }

        // 原点から delta までの符号付き距離を求めます。
        static double GetDistance(Vector start, Vector delta)
        {
            var angle = Vector.AngleBetween(delta, start);
            return start.Length * Math.Sin(angle * Math.PI / 180);
        }

        public void RotateDelta_ForTrackball(DeltaInfo info)
        {
            var delta = info.End - info.Start;
            if (delta.Length == 0) return;

            matrixTransform.Rotate(new Vector3D(delta.Y, delta.X, 0), delta.Length);
        }
    }

    public static class Media3DUtility
    {
        public static void Rotate(this MatrixTransform3D transform, Vector3D axis, double angle)
        {
            var matrix = transform.Matrix;
            matrix.Rotate(new Quaternion(axis, angle));
            transform.Matrix = matrix;
        }

        public static void RotateAt(this MatrixTransform3D transform, Vector3D axis, double angle, Point3D center)
        {
            var matrix = transform.Matrix;
            matrix.RotateAt(new Quaternion(axis, angle), center);
            transform.Matrix = matrix;
        }
    }
}