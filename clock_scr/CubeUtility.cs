using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace clock_scr
{
    public static class CubeUtility
    {
        public static Model3DGroup CreateFaceModel(string[,] squareInfoes)
        {
            int rowCount = squareInfoes.GetLength(0);
            var modelGroup = new Model3DGroup();

            for (int i = 0; i < rowCount; i++)
            {
                string positions = squareInfoes[i, 0];
                string key = squareInfoes[i, 1];

                var brush = new VisualBrush();
                BindingOperations.SetBinding(brush, VisualBrush.VisualProperty, new Binding { ElementName = key });
                var material = new DiffuseMaterial(brush);

                var squareModel = new GeometryModel3D
                {
                    Geometry = CreateSquareGeometry(positions),
                    Material = material,
                    BackMaterial = null,
                };

                modelGroup.Children.Add(squareModel);
            }

            return modelGroup;
        }

        public static Model3DGroup CreateFaceModel(string positions, Visual key)
        {
            var modelGroup = new Model3DGroup();

            var brush = new VisualBrush(key);
            var material = new DiffuseMaterial(brush);

            var squareModel = new GeometryModel3D
            {
                Geometry = CreateSquareGeometry(positions),
                Material = material,
                BackMaterial = null,
            };

            modelGroup.Children.Add(squareModel);

            return modelGroup;
        }

        public static Model3DGroup CreateFaceModel(string positions, string key)
        {
            var modelGroup = new Model3DGroup();

            var brush = new VisualBrush();
            BindingOperations.ClearBinding(brush, VisualBrush.VisualProperty);
            BindingOperations.SetBinding(brush, VisualBrush.VisualProperty, new Binding { ElementName = key });
            var material = new DiffuseMaterial(brush);

            var squareModel = new GeometryModel3D
            {
                Geometry = CreateSquareGeometry(positions),
                Material = material,
                BackMaterial = null,
            };

            modelGroup.Children.Add(squareModel);

            return modelGroup;
        }

        public static Model3DGroup CreateFaceModel(string positions, string key, string key2)
        {
            var modelGroup = new Model3DGroup();

            var brush = new VisualBrush();
            BindingOperations.SetBinding(brush, VisualBrush.VisualProperty, new Binding { ElementName = key });
            var material = new DiffuseMaterial(brush);

            var brush2 = new VisualBrush();
            BindingOperations.SetBinding(brush2, VisualBrush.VisualProperty, new Binding { ElementName = key2 });
            var material2 = new DiffuseMaterial(brush2);

            var squareModel = new GeometryModel3D
            {
                Geometry = CreateSquareGeometry(positions),
                Material = material,
                BackMaterial = material2,
            };

            modelGroup.Children.Add(squareModel);

            return modelGroup;
        }

        public static MeshGeometry3D CreateSquareGeometry(string positions)
        {
            return new MeshGeometry3D
            {
                Positions = Point3DCollection.Parse(positions),
                TriangleIndices = Int32Collection.Parse("0,1,2 0,2,3"),
                TextureCoordinates = PointCollection.Parse("0,0 0,1 1,1 1,0"),
            };
        }
        public static void RemoveCubeModel(Model3DGroup modelGroup, Model3D cubeModelToRemove)
        {
            if (modelGroup != null && cubeModelToRemove != null)
            {
                modelGroup.Children.Remove(cubeModelToRemove);
            }
        }
    }
}