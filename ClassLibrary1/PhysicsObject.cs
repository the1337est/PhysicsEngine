using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine2D
{

    public class PhysicsObject 
    {
        
        public Rigidbody Rigidbody;
        public Polygon Polygon;
        public Polygon IdentityPolygon;
        
        float _Angle = 0.0f;
        public float Angle
        {

            get
            {

                return _Angle;

            }
            set
            {

                Polygon.Points = IdentityPolygon.Points;

                float rad = ((float)Math.PI / 180.0f);
                for (int i = 0; i < Polygon.Points.Count; i++)
                {

                    Vector distanceFromCenter = Polygon.Points[i] - Position;
                    //Polygon.Points[i] = new Vector(Position.X + (float)(Math.Cos(Angle * rad) * distanceFromCenter.X), Position.Y + (float)(Math.Sin(Angle * rad) * distanceFromCenter.Y));
                    Polygon.Points[i].X = Position.X + (Polygon.Points[i].X - Position.X) * (float)(Math.Cos(Angle * rad)) - (Polygon.Points[i].Y - Position.Y) * (float)(Math.Sin(Angle * rad));
                    Polygon.Points[i].Y = Position.Y + ((Polygon.Points[i].Y - Position.Y) * (float)Math.Sin(Angle * rad)) + (Polygon.Points[i].Y - Position.Y) * (float)Math.Cos(Angle * rad);
                }

                _Angle = value;

            }


        }
        
        Vector _Position;
        public Vector Position
        {

            get
            {

                return _Position;

            }
            set
            {

                IdentityPolygon.Offset(value - _Position);
                Polygon.Offset(value - _Position);

                _Position = value;

            }

        }
        
        public PhysicsObject(Vector position, Polygon polygon, Rigidbody rigidbody)
        {

            IdentityPolygon = polygon;

            Rigidbody = rigidbody;
            Polygon = polygon;
            Position = position;

            Rigidbody.RefreshParent(this);

        }

    }

}
