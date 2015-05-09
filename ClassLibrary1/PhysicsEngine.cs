﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Engine2D
{

    public class PhysicsEngine
    {

        public List<PhysicsObject> PhysicsObjects = new List<PhysicsObject>();

        public static PhysicsEngine Instance;

        public delegate void OnLogHandler(string message);

        public event OnLogHandler OnLog;

        public PhysicsEngine(OnLogHandler logTarget)
        {

            if (Instance == null)
            {

                Instance = this;

            }

            OnLog += logTarget;

            Log("Physics Engine started.");
           
        }

        public void Update()
        {

            foreach(PhysicsObject p1 in PhysicsObjects)
            {

                foreach (PhysicsObject p2 in PhysicsObjects)
                {

                    if (p1 == p2) continue;

                    PolygonCollisionResult r = PolygonCollision(p1.Polygon, p2.Polygon, p1.Rigidbody.Velocity);

                    Vector collisionTranslation = p1.Rigidbody.Velocity;

                    if (r.WillIntersect)
                    {

                        Log(string.Format("Intersection found. ({0}, {1})", p1.ToString(), p2.ToString()));
                        collisionTranslation = p1.Rigidbody.Velocity + r.MinimumTranslationVector;


                        p1.Rigidbody.Velocity = new Vector(0, 0);

                    }

                    p1.Polygon.Offset(collisionTranslation);
                    p1.Position += collisionTranslation;
                
                }
                
            }

        }

        public PolygonCollisionResult PolygonCollision(Polygon polygonA, Polygon polygonB, Vector velocity)
        {

            PolygonCollisionResult result = new PolygonCollisionResult();
            result.Intersect = true;
            result.WillIntersect = true;

            int edgeCountA = polygonA.Edges.Count;
            int edgeCountB = polygonB.Edges.Count;

            float minIntervalDistance = float.PositiveInfinity;
            Vector translationAxis = new Vector();
            Vector edge;

            for (int edgeIndex = 0; edgeIndex < edgeCountA + edgeCountB; edgeIndex++)
            {

                if (edgeIndex < edgeCountA)
                {

                    edge = polygonA.Edges[edgeIndex];

                }
                else
                {

                    edge = polygonB.Edges[edgeIndex - edgeCountA];

                }



                // ===== 1. Find if the polygons are currently intersecting =====

                Vector axis = new Vector(-edge.Y, edge.X);
                axis.Normalize();

                // Find the projection of the polygon on the current axis
                float minA = 0; float minB = 0; float maxA = 0; float maxB = 0;
                ProjectPolygon(axis, polygonA, ref minA, ref maxA);
                ProjectPolygon(axis, polygonB, ref minB, ref maxB);

                // Check if the polygon projections are currentlty intersecting
                if (IntervalDistance(minA, maxA, minB, maxB) > 0) result.Intersect = false;

                // ===== 2. Now find if the polygons *will* intersect =====

                // Project the velocity on the current axis
                float velocityProjection = axis.DotProduct(velocity);

                // Get the projection of polygon A during the movement
                if (velocityProjection < 0)
                {

                    minA += velocityProjection;

                }
                else
                {

                    maxA += velocityProjection;

                }

                // Do the same test as above for the new projection
                float intervalDistance = IntervalDistance(minA, maxA, minB, maxB);
                if (intervalDistance > 0) result.WillIntersect = false;

                // If the polygons are not intersecting and won't intersect, exit the loop
                if (!result.Intersect && !result.WillIntersect) break;

                // Check if the current interval distance is the minimum one. If so store
                // the interval distance and the current distance.
                // This will be used to calculate the minimum translation vector
                intervalDistance = Math.Abs(intervalDistance);
                if (intervalDistance < minIntervalDistance)
                {

                    minIntervalDistance = intervalDistance;
                    translationAxis = axis;

                    Vector d = polygonA.Center - polygonB.Center;
                    if (d.DotProduct(translationAxis) < 0) translationAxis = -translationAxis;

                }

            }

            // The minimum translation vector can be used to push the polygons appart.
            // First moves the polygons by their velocity
            // then move polygonA by MinimumTranslationVector.
            if (result.WillIntersect) result.MinimumTranslationVector = translationAxis * minIntervalDistance;

            return result;
        
        }

        // Calculate the distance between [minA, maxA] and [minB, maxB]
        // The distance will be negative if the intervals overlap
        public float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            
            if (minA < minB)
            {
                
                return minB - maxA;
            
            }
            else
            {
            
                return minA - maxB;
        
            }
        
        }


        // Calculate the projection of a polygon on an axis and returns it as a [min, max] interval
        public void ProjectPolygon(Vector axis, Polygon polygon, ref float min, ref float max)
        {

            float d = axis.DotProduct(polygon.Points[0]);
            min = d;
            max = d;
            for (int i = 0; i < polygon.Points.Count; i++)
            {

                d = polygon.Points[i].DotProduct(axis);
                if (d < min)
                {

                    min = d;

                }
                else
                {

                    if (d > max)
                    {

                        max = d;

                    }

                }

            }

        }

        public static void Log(string message)
        {
            
            System.Diagnostics.Debug.WriteLine(message);

            if (Instance != null)
            {

                if (Instance.OnLog != null)
                {

                    Instance.OnLog(message);

                }

            }
            else
            {

                System.Diagnostics.Debug.WriteLine("Unable to find a single instance of PhysicsEngine.");

            }

        }

    }

}

