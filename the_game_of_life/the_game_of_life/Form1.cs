using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace the_game_of_life
{
    public partial class Form1 : Form
    {
        int amountOfMesh = 0;
        int widthOfMesh = 0;
        int amountOfRound = 0;
        private int heightOfMesh=0;

        Graphics gr = null;
        private Bitmap canvas;

        Pixel[,] mesh;
        
        string caption = "Error";
        MessageBoxButton button = MessageBoxButton.OK;
        MessageBoxImage icon = MessageBoxImage.Warning;

        public Form1()
        {
            InitializeComponent();
            canvas = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            gr = Graphics.FromImage(canvas);
        }
        private void drawMesh()
        {
            Pen pen = new Pen(Color.FromArgb(192,192,192), 1);
            for (int i = 0; i < amountOfMesh; i++)
            {
                gr.DrawLine(pen, new Point((int)(i * ((double)this.canvas.Width / amountOfMesh)), 0), new Point((int)(i * ((double)this.canvas.Width / amountOfMesh)), canvas.Height));//pion
                gr.DrawLine(pen, new Point(0, (int)(i * ((double)this.canvas.Height / amountOfMesh))), new Point(canvas.Width, (int)(i * ((double)this.canvas.Height / amountOfMesh))));//poziom
            }
            gr.DrawLine(pen, new Point(0, canvas.Height - 1), new Point(canvas.Width, canvas.Height - 1));//last horizontal
            gr.DrawLine(pen, new Point(canvas.Width - 1, 0), new Point(canvas.Width - 1, canvas.Height));//last vertical

            this.widthOfMesh = (int)(2 * ((double)this.canvas.Width / amountOfMesh)) - (int)(1 * ((double)this.canvas.Width / amountOfMesh));//window width
            this.heightOfMesh = (int)(2 * ((double)this.canvas.Height / amountOfMesh)) - (int)(1 * ((double)this.canvas.Height / amountOfMesh));//window height

            this.pictureBox1.Image = (Image)canvas;

        }
        private void drawRect()
        {
            for(int i = 0; i < amountOfMesh; i++)
            {
                for(int j = 0; j < amountOfMesh; j++)
                {
                    if (mesh[i, j].isAlive) {
                        this.gr.FillRectangle(new SolidBrush(Color.Blue), mesh[i, j].a, mesh[i, j].b, this.widthOfMesh,this.heightOfMesh);
                    }
                }
            }
            this.pictureBox1.Image = (Image)canvas;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            gr.Clear(Color.White);
            if (this.textBox1.Text != "")//window is empty
            {
                try
                {
                    this.amountOfMesh = int.Parse(this.textBox1.Text);
                        if (this.amountOfMesh < 0)
                        {
                            System.Windows.MessageBox.Show("Amount of mesh is too small", caption, button, icon);
                            this.amountOfMesh = 0;
                        }
                    }
                catch (FormatException exception)
                {
                    System.Windows.MessageBox.Show(exception.Message, caption, button, icon);
                }
            }
            drawMesh();
            mesh = new Pixel[amountOfMesh, amountOfMesh];
            for (int i = 0; i < amountOfMesh; i++)
            {
                for (int j = 0; j < amountOfMesh; j++)
                {
                    mesh[i, j] = new Pixel((int)(i * ((double)canvas.Width / amountOfMesh)), (int)(j * ((double)canvas.Height / amountOfMesh)));
                }
            }
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox4.Text != "")
            {
                try
                {
                    this.amountOfRound = int.Parse(this.textBox4.Text);
                    if (this.amountOfRound < 0)
                    {
                        System.Windows.MessageBox.Show("Amount of round is too small", caption, button, icon);
                        this.amountOfRound = 0;
                    }
                }
                catch (FormatException exception)
                {
                    System.Windows.MessageBox.Show(exception.Message, caption, button, icon);
                }
            }
        }
        //start gry
        private void button1_Click(object sender, EventArgs e)
        {
            int neighbour = 0;
            Pixel[,] temp= new Pixel[amountOfMesh, amountOfMesh];
            for(int i = 0; i < amountOfMesh; i++)
                for (int j = 0; j < amountOfMesh; j++)
                    temp[i, j] = new Pixel(mesh[i, j].a, mesh[i, j].b, mesh[i, j].isAlive);
          

            for (int i = 0; i < this.amountOfRound; i++)
            {
                for (int x = 0; x < amountOfMesh; x++)
                {
                    for(int y = 0; y < amountOfMesh; y++)
                    {
                        neighbour = 0;
                        try{ if (mesh[x - 1, y - 1].isAlive) neighbour++;}catch (IndexOutOfRangeException){ }
                        try{ if (mesh[x - 1, y].isAlive) neighbour++;}catch (IndexOutOfRangeException) { }
                        try{ if (mesh[x - 1, y + 1].isAlive) neighbour++;}catch (IndexOutOfRangeException) { }

                        try{ if (mesh[x, y - 1].isAlive) neighbour++;} catch (IndexOutOfRangeException) { }
                        try{ if (mesh[x, y + 1].isAlive) neighbour++;}catch (IndexOutOfRangeException) { }

                        try{ if (mesh[x + 1, y - 1].isAlive) neighbour++;}catch (IndexOutOfRangeException) { }
                        try{ if (mesh[x + 1, y ].isAlive) neighbour++;} catch (IndexOutOfRangeException) { }
                        try{ if (mesh[x + 1, y + 1].isAlive) neighbour++;} catch (IndexOutOfRangeException) { }


                        if (neighbour == 3) temp[x, y].born();
                        else if (neighbour == 2 && mesh[x,y].isAlive) temp[x, y].born();
                        else temp[x, y].kill();
                    }
                }
                
                for (int x = 0; x < amountOfMesh; x++)
                    for (int j = 0; j < amountOfMesh; j++)
                        mesh[x, j] = new Pixel(temp[x, j].a, temp[x, j].b, temp[x, j].isAlive);

                gr.Clear(Color.White);
                drawMesh();
                drawRect();
                this.pictureBox1.Image = (Image)canvas;
                pictureBox1.Refresh();
                Thread.Sleep(250);
            }

            System.Windows.MessageBox.Show("End", "success", button, icon);
        }
             
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            int xPicture = this.Left +2*this.pictureBox1.Location.X;
            int yPicture = this.Top + 4* this.pictureBox1.Location.Y;
            int xMouse = MousePosition.X;
            int yMouse = MousePosition.Y;
            int x, y;
            x = xMouse - xPicture;
            y = yMouse - yPicture;

            x = (int)(x /(double) this.widthOfMesh);
            y = (int)(y /(double) this.heightOfMesh);
            try
            {
                if (mesh[x, y].isAlive)
                {
                    this.mesh[x, y].kill();
                    gr.Clear(Color.White);
                    drawRect();
                }
                else
                {
                        this.gr.FillRectangle(new SolidBrush(Color.Blue), mesh[x, y].a, mesh[x, y].b, this.widthOfMesh, this.heightOfMesh);
                        this.mesh[x, y].born();
                }
                drawMesh();
            }
            catch (IndexOutOfRangeException) { }
        }

    }
    class Pixel
    {
        public int a, b;
        public bool isAlive=false;
        public Pixel(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
        public Pixel(int a, int b,bool isAlive)
        {
            this.a = a;
            this.b = b;
            this.isAlive = isAlive;
        }
        public void born() { this.isAlive = true; }
        public void kill() { this.isAlive = false;}
    }
}
