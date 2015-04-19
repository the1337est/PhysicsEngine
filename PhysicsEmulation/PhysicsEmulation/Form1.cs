﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Engine2D;

namespace PhysicsEmulation
{

    public partial class Form1 : Form
    {

        PhysicsEngine PhysicsEngine;
        


        public Form1()
        {

            InitializeComponent();
            InitializeLayout();
            this.Paint += new PaintEventHandler(Form1_Paint);

            DoubleBuffered = true;

            PhysicsEngine = new PhysicsEngine();

            GameTimer.Tick += new EventHandler(GameTimer_Tick);

            Polygon p = new Polygon();
            p.Points.Add(new Vector(-100, -100));
            p.Points.Add(new Vector(100, -100));
            p.Points.Add(new Vector(100, 100));
            p.Points.Add(new Vector(-100, 100));

            p.Offset(300, 400);
           
            PhysicsEngine.PhysicsObjects.Add(new PhysicsObject(new Vector(400, 300), p, new Rigidbody()));


            p = new Polygon();
            p.Points.Add(new Vector(0, 580));
            p.Points.Add(new Vector(800, 580));
            p.Points.Add(new Vector(800, 600));
            p.Points.Add(new Vector(0, 600));

            PhysicsEngine.PhysicsObjects.Add(new PhysicsObject(new Vector(0,0), p, new Rigidbody()));

            foreach (PhysicsObject po in PhysicsEngine.PhysicsObjects)
            {

                po.Polygon.BuildEdges();

            }
            GameTimer.Interval = 16;
            GameTimer.Start();

        }

        void InitializeLayout()
        {

            Text = "Physics emulation";
            ClientSize = new Size(800, 600);
            MaximizeBox = false;
            MaximumSize = Size;
            MinimumSize = Size;
            ShowIcon = false;

        }

        void GameTimer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
            //this.Update();
        }

        void Form1_Paint(object sender, PaintEventArgs e)
        {

            PhysicsEngine.PhysicsObjects[0].Polygon.Offset(PhysicsSettings.Gravity * 0.016f * 5.0f);

            /*foreach (PhysicsObject po in PhysicsEngine.PhysicsObjects)
            {

                po.Polygon.BuildEdges();

            }*/
            PhysicsEngine.Update();

            foreach (PhysicsObject p in PhysicsEngine.PhysicsObjects)
            {

                for (int i = 0; i < p.Polygon.Points.Count; i++)
                {

                    Point p1, p2;
                    if (i == p.Polygon.Points.Count - 1)
                    {

                        p1 = p.Polygon.Points[i];
                        p2 = p.Polygon.Points[0];
                        e.Graphics.DrawLine(new Pen(Color.FromArgb(200, 0, 0), 5.0f), p1, p2);

                    }
                    else
                    {
                        //k
                        p1 = p.Polygon.Points[i];
                        p2 = p.Polygon.Points[i+1];
                        e.Graphics.DrawLine(new Pen(Color.FromArgb(0, 200, 0), 5.0f), p1, p2);

                    }

                }

            }

        }

    }

}