﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine2D
{

    public class PhysicsObject 
    {
        //When we assign a Rigidbody to this object, we assign this object as the parent
        public Rigidbody Rigidbody;
        public Polygon Polygon;
        public Vector Position;

        public PhysicsObject(Vector position, Polygon polygon, Rigidbody rigidbody)
        {

            Rigidbody = rigidbody;
            Polygon = polygon;
            Position = position;

            Rigidbody.RefreshParent(this);

        }

    }

}