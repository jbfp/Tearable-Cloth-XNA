using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace TearableCloth
{
    public class Cloth
    {
        public const int Accuracy = 5;
        public const int ClothWidth = 50;
        public const int ClothHeight = 30;
        public const int CanvasWidth = 700;
        public const int CanvasHeight = 500;
        public const float StartX = CanvasWidth / 2 - ClothWidth * Spacing / 2;
        public const float StartY = 20.0f;
        public const float Spacing = 10.0f;

        public Cloth()
        {
            Points = new List<Point>();

            for (var y = 0; y <= ClothHeight; y++)
            {
                for (var x = 0; x <= ClothWidth; x++)
                {
                    var position = new Vector2(StartX + x*Spacing,
                                               StartY + y*Spacing);
                    var point = new Point(position);

                    if (x != 0)
                    {
                        point.Attach(Points.Last());
                    }

                    if (y == 0)
                    {
                        point.PinPosition = point.PreviousPosition;
                    }
                    else
                    {
                        point.Attach(Points[x + (y - 1)*(ClothWidth + 1)]);
                    }

                    Points.Add(point);
                }
            }
        }

        public IList<Point> Points { get; set; }

        public void Update(GameTime gameTime)
        {
            for (var i = Accuracy; i > 0; i--)
            {
                foreach (var point in Points)
                {
                    point.ResolveConstraint();
                }
            }

            foreach (var point in Points)
            {
                point.Update(gameTime);
            }
        }
    }
}