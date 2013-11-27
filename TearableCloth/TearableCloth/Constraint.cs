using Microsoft.Xna.Framework;

namespace TearableCloth
{
    public class Constraint
    {
        public Constraint(Point point1, Point point2)
        {
            Point1 = point1;
            Point2 = point2;
            Length = Cloth.Spacing;
        }

        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public float Length { get; set; }

        public void Resolve()
        {
            var difference = Vector2.Subtract(Point1.Position, Point2.Position);
            var distance = difference.Length();
            var relativeDifference = (Length - distance)/distance;

            if (distance > Point.TearDistance)
            {
                Point1.RemoveConstraint(this);
            }

            var position = difference*relativeDifference*0.5f;
            Point1.Position += position;
            Point2.Position -= position;
        }
    }
}