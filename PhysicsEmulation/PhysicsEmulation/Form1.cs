using System;
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

            PhysicsEngine = new PhysicsEngine(Log);

            GameTimer.Tick += new EventHandler(GameTimer_Tick);

            Polygon p = new Polygon();  
            p.Points.Add(new Vector(-100, -100));
            p.Points.Add(new Vector(100, -100));
            p.Points.Add(new Vector(100, 100));
            p.Points.Add(new Vector(-100, 100));
           
            PhysicsEngine.PhysicsObjects.Add(new PhysicsObject(new Vector(400, 300), p, new Rigidbody()));

            Polygon pg = new Polygon();
            pg.Points.Add(new Vector(-30, -50));
            pg.Points.Add(new Vector(50, -70));
            pg.Points.Add(new Vector(30, 50));
            pg.Points.Add(new Vector(-60, 40));
            
            PhysicsEngine.PhysicsObjects.Add(new PhysicsObject(new Vector(100, 200), pg, new Rigidbody()));

            p = new Polygon(); 
            p.Points.Add(new Vector(0, 740));
            p.Points.Add(new Vector(1024, 740));
            p.Points.Add(new Vector(1024, 768));
            p.Points.Add(new Vector(0, 768));

            PhysicsEngine.PhysicsObjects.Add(new PhysicsObject(new Vector(0,0), p, new Rigidbody()));

            foreach (PhysicsObject po in PhysicsEngine.PhysicsObjects)
            {

                po.Polygon.BuildEdges();

            }
            GameTimer.Interval = 16;
            GameTimer.Start();

        }

        void Log(string message)
        {

            System.Diagnostics.Debug.WriteLine(message);

        }

        void InitializeLayout()
        {

            Text = "Physics emulation";
            ClientSize = new Size(1024, 768);
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
            
                PhysicsEngine.PhysicsObjects[0].Rigidbody.Velocity += PhysicsSettings.Gravity * 0.016f * 4.5f;
                PhysicsEngine.PhysicsObjects[1].Rigidbody.Velocity += PhysicsSettings.Gravity * 0.016f * 4.5f;

                //PhysicsEngine.PhysicsObjects[0].Polygon.Offset(PhysicsSettings.Gravity * 0.016f * 5.0f);

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
                            p2 = p.Polygon.Points[i + 1];
                            e.Graphics.DrawLine(new Pen(Color.FromArgb(0, 200, 0), 5.0f), p1, p2);

                        }

                    }

                }

            
            label1.Text = "Polygon 1 Position: " + PhysicsEngine.PhysicsObjects[0].Position;
            label2.Text = "Polygon 2 Position: " + PhysicsEngine.PhysicsObjects[1].Position;


        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {

                //spawn g2g

            }
            lastClick = e;

        }
        
        //cool. now um not expecting this to work right now xD xD

        MouseEventArgs lastClick = null;
        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            //what DOUBLE CLICK  lOxLD
            //kind of a hacky workaround but a double click should be in the same position I think so it should work
            Vector v = new Vector((float)lastClick.X, (float)lastClick.Y);
            label3.Text = Convert.ToString(MousePosition);
            PhysicsEngine.PhysicsObjects[1].Position = v;
            PhysicsEngine.PhysicsObjects[1].Angle += 50.0f;//wut
            PhysicsEngine.Update();
            float rad = ((float)Math.PI / 180.0f);
            label3.Text = "Angle:" + PhysicsEngine.PhysicsObjects[1].Angle + "|Cosine:" + Math.Cos(PhysicsEngine.PhysicsObjects[1].Angle * rad).ToString();
            //maybe use label3 for text wait

        }

    }

}
