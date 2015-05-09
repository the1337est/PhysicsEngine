using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine2D
{
    public struct PolygonCollisionResult
    {

        public bool WillIntersect; 
        public bool Intersect; 
        public Vector MinimumTranslationVector; // The translation to apply to polygon A to push the polygons appart.
    
    }
}
