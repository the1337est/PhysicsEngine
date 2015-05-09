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
        //you could probably apply it in increments like we did with offset but this is how we'll do it for now ok

        float _Angle = 0.0f;
        public float Angle
        {

            get
            {

                return _Angle;

            }
            set
            {

                //apply rotation to all the polygon points

                //one issue is that our polygon points aren't really defined locally yet but i'll see if I can fix this somehow

                Polygon.Points = IdentityPolygon.Points;

                float rad = ((float)Math.PI / 180.0f);
                for (int i = 0; i < Polygon.Points.Count; i++)
                {

                    Vector distanceFromCenter = Polygon.Points[i] - Position;
                    //hmm
                    Polygon.Points[i] = new Vector(Position.X + (float)(Math.Cos(Angle * rad) * distanceFromCenter.X), Position.Y + (float)(Math.Sin(Angle * rad) * distanceFromCenter.Y));

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

                //we still offset the original polygon as well (not by angle but just position) alriht
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
