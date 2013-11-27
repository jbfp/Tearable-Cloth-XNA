using System.Collections.Generic;
using InputManagement;
using Microsoft.Xna.Framework;

namespace TearableCloth
{
    public class Point
    {
        public const float MouseInfluence = 20.0f;
        public const float TearDistance = 60.0f;
        public const float Gravity = 1200.0f;
        public const float MouseCut = 5.0f;

        public Point(Vector2 coordinates)
        {
            Position = coordinates;
            PreviousPosition = coordinates;
            Constraints = new List<Constraint>();
        }

        public Vector2 Position { get; set; }
        public Vector2 PreviousPosition { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2? PinPosition { get; set; }
        public IList<Constraint> Constraints { get; set; }

        public void Update(GameTime gameTime)
        {
            if (MouseHandler.IsButtonDown(MouseButtons.Left) ||
                MouseHandler.IsButtonDown(MouseButtons.Right))
            {
                var mousePosition = new Vector2(MouseHandler.Location.X, MouseHandler.Location.Y);
                var deltaMousePosition = new Vector2(MouseHandler.DeltaX, MouseHandler.DeltaY);
                var distance = Vector2.Distance(PreviousPosition, mousePosition);

                if (MouseHandler.IsButtonDown(MouseButtons.Left))
                {
                    if (MouseInfluence > distance)
                    {
                        PreviousPosition = Position - deltaMousePosition*1.8f;
                    }
                }
                else if (MouseCut > distance)
                {
                    Constraints.Clear();
                }
            }

            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            AddForce(new Vector2(0, Gravity));
            delta *= delta;
            var newPosition = Position + ((Position - PreviousPosition)*0.99f) + ((Velocity/2)*delta);
            PreviousPosition = Position;
            Position = newPosition;
            Velocity = Vector2.Zero;
        }

        public void Attach(Point point)
        {
            Constraints.Add(new Constraint(this, point));
        }

        public void RemoveConstraint(Constraint constraint)
        {
            Constraints.Remove(constraint);
        }

        public void AddForce(Vector2 force)
        {
            Velocity += force;
        }

        public void ResolveConstraint()
        {
            if (PinPosition.HasValue)
            {
                Position = PinPosition.Value;
                return;
            }

            // For-loop to avoid InvalidOperationException from "modified collection" errors.
            for (int index = 0; index < Constraints.Count; index++)
            {
                Constraints[index].Resolve();
            }
        }
    }
}