using System;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Symulacja_świata
{
    public partial class Form1 : Form
    {
        int amountOfMesh = 0;
        int widthOfMesh = 0;
        int amountOfRound = 0;
        private int heightOfMesh = 0;
        int animal = 0;
        
        Pixel[,] mesh;

        Graphics gr = null;
        private Bitmap canvas;

        string caption = "Error";
        MessageBoxButton button = MessageBoxButton.OK;
        MessageBoxImage icon = MessageBoxImage.Warning;

        public Form1()
        {
            InitializeComponent();
            canvas = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            gr = Graphics.FromImage(canvas);
        }
        private void draw()
        {
            gr.Clear(Color.White);
            drawMesh();
            drawRect();
            this.pictureBox1.Image = (Image)canvas;
            pictureBox1.Refresh();
        }
        private void trackBar1_Scroll(object sender, EventArgs e) => animal = trackBar1.Value;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            gr.Clear(Color.White);
            if (this.textBox1.Text != "")
            {
                try
                {
                    this.amountOfMesh = int.Parse(this.textBox1.Text);
                    if (this.amountOfMesh < 0)
                    {
                        System.Windows.MessageBox.Show("Liczba okienek jest za mała", caption, button, icon);
                        this.amountOfMesh = 0;
                    }
                    else if (this.amountOfMesh > 1000)
                    {
                        System.Windows.MessageBox.Show("Liczba okienek jest za duża", caption, button, icon);
                        this.amountOfMesh = 0;
                    }
                }
                catch (FormatException exception)
                {
                    System.Windows.MessageBox.Show(exception.Message, caption, button, icon);
                }
                drawMesh();
                mesh = new Pixel[amountOfMesh, amountOfMesh];
                for (int i = 0; i < amountOfMesh; i++)
                    for (int j = 0; j < amountOfMesh; j++)
                        mesh[i, j] = new Pixel((int)(i * ((double)canvas.Width / amountOfMesh)), (int)(j * ((double)canvas.Height / amountOfMesh)));
            }
        }
        private void drawMesh()
        {
            Pen pen = new Pen(Color.FromArgb(192, 192, 129), 1);
            for (int i = 0; i < this.amountOfMesh; i++)
            {
                gr.DrawLine(pen, new Point((int)(i * ((double)this.canvas.Width / this.amountOfMesh)), 0), new Point((int)(i * ((double)this.canvas.Width / amountOfMesh)), canvas.Height));//vertical
                gr.DrawLine(pen, new Point(0, (int)(i * ((double)this.canvas.Height / amountOfMesh))), new Point(canvas.Width, (int)(i * ((double)this.canvas.Height / amountOfMesh))));//horizontal
            }
            gr.DrawLine(pen, new Point(0, canvas.Height - 1), new Point(canvas.Width, canvas.Height - 1));//last horizontal
            gr.DrawLine(pen, new Point(canvas.Width - 1, 0), new Point(canvas.Width - 1, canvas.Height));//last vertical

            this.widthOfMesh = (int)(2 * ((double)this.canvas.Width / amountOfMesh)) - (int)(1 * ((double)this.canvas.Width / amountOfMesh));
            this.heightOfMesh = (int)(2 * ((double)this.canvas.Height / amountOfMesh)) - (int)(1 * ((double)this.canvas.Height / amountOfMesh));

            this.pictureBox1.Image = (Image)canvas;
            try
            {
                this.pictureBox1.Refresh();
            }catch(OutOfMemoryException e) { System.Windows.MessageBox.Show(e.Message, caption, button, icon); }
        }
        private void drawRect()
        {
            for (int i = 0; i < amountOfMesh; i++)
                for (int j = 0; j < amountOfMesh; j++)
                    if (mesh[i, j].isAlive)
                            this.gr.DrawImage(mesh[i, j].animal.image, new Rectangle(mesh[i, j].a, mesh[i, j].b, this.widthOfMesh, this.heightOfMesh));
            this.pictureBox1.Image = (Image)canvas;
            this.pictureBox1.Refresh();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                bool temp = mesh[0, 0].isAlive;
            }
            catch (System.NullReferenceException)
            {
                textBox1.Text = "1";
                textBox1_TextChanged(sender,e);
            }
            int xPicture = this.Left + 2 * this.pictureBox1.Location.X;
            int yPicture = this.Top + 4 * this.pictureBox1.Location.Y;
            int xMouse = MousePosition.X;
            int yMouse = MousePosition.Y;
            int x, y;
            x = xMouse - xPicture;
            y = yMouse - yPicture;

            x = (int)(x / (double)this.widthOfMesh);
            y = (int)(y / (double)this.heightOfMesh);
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
                    mesh[x, y] = new Pixel(mesh[x, y].a, mesh[x, y].b, new Animal(this.animal));
                    this.gr.DrawImage(mesh[x, y].animal.image, new Rectangle(mesh[x, y].a, mesh[x, y].b, this.widthOfMesh, this.heightOfMesh));
                    this.mesh[x, y].born();
                }
                drawMesh();
            }
            catch (IndexOutOfRangeException) { }
        }
        private void go( int[] neighbours, Pixel[,] temp)
        {
            int priority;
            //eating
            for (int x = 0; x < amountOfMesh; x++)//horizontal
                for (int y = 0; y < amountOfMesh; y++)//vertical
                {
                    if (mesh[x, y].isAlive && !mesh[x, y].animal.eat)
                    {
                        priority = mesh[x, y].animal.priority;
                        ////////////////////////
                        try
                        {
                            if (mesh[x - 1, y - 1].isAlive && mesh[x - 1, y - 1].animal.priority == priority - 1)
                            {
                                mesh[x - 1, y - 1].setAnimal(mesh[x, y].animal);
                                mesh[x - 1, y - 1].animal.attack();
                                mesh[x, y].kill();
                                continue;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        try
                        {
                            if (mesh[x - 1, y].isAlive && mesh[x - 1, y].animal.priority == priority - 1)
                            {
                                mesh[x - 1, y].setAnimal(mesh[x, y].animal);
                                mesh[x - 1, y].animal.attack();
                                mesh[x, y].kill();
                                continue;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        try
                        {
                            if (mesh[x - 1, y + 1].isAlive && mesh[x - 1, y + 1].animal.priority == priority - 1)
                            {
                                mesh[x - 1, y + 1].setAnimal(mesh[x, y].animal);
                                mesh[x - 1, y + 1].animal.attack();
                                mesh[x, y].kill();
                                continue;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        //////////////////////
                        try
                        {
                            if (mesh[x, y - 1].isAlive && mesh[x, y - 1].animal.priority == priority - 1)
                            {
                                mesh[x, y - 1].setAnimal(mesh[x, y].animal);
                                mesh[x, y - 1].animal.attack();
                                mesh[x, y].kill();
                                continue;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        try
                        {
                            if (mesh[x, y + 1].isAlive && mesh[x, y + 1].animal.priority == priority - 1)
                            {
                                mesh[x, y + 1].setAnimal(mesh[x, y].animal);
                                mesh[x, y + 1].animal.attack();
                                mesh[x, y].kill();
                                continue;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        /////////////////////////////
                        try
                        {
                            if (mesh[x + 1, y - 1].isAlive && mesh[x + 1, y - 1].animal.priority == priority - 1)
                            {
                                mesh[x + 1, y - 1].setAnimal(mesh[x, y].animal);
                                mesh[x + 1, y - 1].animal.attack();
                                mesh[x, y].kill();
                                continue;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        try
                        {
                            if (mesh[x + 1, y].isAlive && mesh[x + 1, y].animal.priority == priority - 1)
                            {
                                mesh[x + 1, y].setAnimal(mesh[x, y].animal);
                                mesh[x + 1, y].animal.attack();
                                mesh[x, y].kill();
                                continue;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        try
                        {
                            if (mesh[x + 1, y + 1].isAlive && mesh[x + 1, y + 1].animal.priority == priority - 1)
                            {
                                mesh[x + 1, y + 1].setAnimal(mesh[x, y].animal);
                                mesh[x + 1, y + 1].animal.attack();
                                mesh[x, y].kill();
                                continue;
                            }
                        }
                        catch (IndexOutOfRangeException) { }
                        ///////////////////////////////////////
                    }
                }
            bool kill;
            //deleting old and hungry animal
            for (int x = 0; x < amountOfMesh; x++)
                for (int y = 0; y < amountOfMesh; y++)
                {

                    if (mesh[x, y].isAlive && mesh[x, y].animal.priority == 0) continue;//if live and grass
                    else if (mesh[x, y].isAlive)//if live
                    {
                        kill = mesh[x, y].animal.round();
                        if (kill == false)
                        {
                            mesh[x, y].kill();
                        }
                    }
                }

            //born
            for (int x = 0; x < amountOfMesh; x++)//horizontal
                for (int y = 0; y < amountOfMesh; y++)//vertical
                {
                    for (int p = 0; p < neighbours.Length; p++) neighbours[p] = 0;
                    if (mesh[x, y].isAlive && mesh[x, y].animal.priority == 0)//grass
                    {
                        temp[x, y].born(0);
                        try { if (!mesh[x - 1, y - 1].isAlive) temp[x - 1, y - 1].born(0); } catch (IndexOutOfRangeException) { }
                        try { if (!mesh[x - 1, y].isAlive) temp[x - 1, y].born(0); } catch (IndexOutOfRangeException) { }
                        try { if (!mesh[x - 1, y + 1].isAlive) temp[x - 1, y + 1].born(0); } catch (IndexOutOfRangeException) { }

                        try { if (!mesh[x, y - 1].isAlive) temp[x, y - 1].born(0); } catch (IndexOutOfRangeException) { }
                        try { if (!mesh[x, y + 1].isAlive) temp[x, y + 1].born(0); } catch (IndexOutOfRangeException) { }

                        try { if (!mesh[x + 1, y - 1].isAlive) temp[x + 1, y - 1].born(0); } catch (IndexOutOfRangeException) { }
                        try { if (!mesh[x + 1, y].isAlive) temp[x + 1, y].born(0); } catch (IndexOutOfRangeException) { }
                        try { if (!mesh[x + 1, y + 1].isAlive) temp[x + 1, y + 1].born(0); } catch (IndexOutOfRangeException) { }
                    }
                    else if (!mesh[x, y].isAlive)//born
                    {
                        try { if (mesh[x - 1, y - 1].isAlive) neighbours[mesh[x - 1, y - 1].animal.priority]++; } catch (IndexOutOfRangeException) { }
                        try { if (mesh[x - 1, y].isAlive) neighbours[mesh[x - 1, y].animal.priority]++; } catch (IndexOutOfRangeException) { }
                        try { if (mesh[x - 1, y + 1].isAlive) neighbours[mesh[x - 1, y + 1].animal.priority]++; } catch (IndexOutOfRangeException) { }

                        try { if (mesh[x, y - 1].isAlive) neighbours[mesh[x, y - 1].animal.priority]++; } catch (IndexOutOfRangeException) { }
                        try { if (mesh[x, y + 1].isAlive) neighbours[mesh[x, y + 1].animal.priority]++; } catch (IndexOutOfRangeException) { }

                        try { if (mesh[x + 1, y - 1].isAlive) neighbours[mesh[x + 1, y - 1].animal.priority]++; } catch (IndexOutOfRangeException) { }
                        try { if (mesh[x + 1, y].isAlive) neighbours[mesh[x + 1, y].animal.priority]++; } catch (IndexOutOfRangeException) { }
                        try { if (mesh[x + 1, y + 1].isAlive) neighbours[mesh[x + 1, y + 1].animal.priority]++; } catch (IndexOutOfRangeException) { }

                        for (int k = 0; k < neighbours.Length; k++)
                        {
                            if (neighbours[k] == 2)
                            {
                                temp[x, y].born(k);
                                break;
                            }
                        }
                    }
                    else if (mesh[x, y].isAlive) temp[x, y] = new Pixel(mesh[x, y]);
                }

            for (int x = 0; x < amountOfMesh; x++)
                for (int j = 0; j < amountOfMesh; j++)
                {
                    if (temp[x, j].isAlive)
                    {
                        mesh[x, j].copy(temp[x, j]);
                        temp[x, j].clear();
                    }
                    else mesh[x, j].copy(temp[x, j]);
                }

            draw();
            Thread.Sleep(250);//animation
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int[] neighbours = new int[5];
            Pixel[,] temp = new Pixel[amountOfMesh, amountOfMesh];
            for (int i = 0; i < amountOfMesh; i++)
                for (int j = 0; j < amountOfMesh; j++)
                    temp[i, j] = new Pixel(mesh[i, j].a, mesh[i, j].b);

            
            for (int i = 0; i < this.amountOfRound; i++)//round
            {
                go(neighbours, temp);
            }
            System.Windows.MessageBox.Show("koniec", "success", button, MessageBoxImage.Information);
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox2.Text != "")
            {
                try
                {
                    this.amountOfRound = int.Parse(this.textBox2.Text);
                    if (this.amountOfRound < 0)
                    {
                        System.Windows.MessageBox.Show("Liczba tur jest za mała", caption, button, icon);
                        this.amountOfRound = 0;
                    }
                }
                catch (FormatException exception)
                {
                    System.Windows.MessageBox.Show(exception.Message, caption, button, icon);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            for(int x = 0; x < amountOfMesh; x++)
            {
                for(int y = 0; y < amountOfMesh; y++)
                {
                    if (mesh[x, y].isAlive) mesh[x, y].kill();
                }
            }
            gr.Clear(Color.White);
            drawMesh();
            drawRect();
            this.pictureBox1.Image = (Image)canvas;
            pictureBox1.Refresh();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            String userManual = "Aby rozpocząć grę należy ustalić liczbę okienek oraz liczbę tur. \n";
            userManual += "Następnie na suwaku ustawiamy jaki obiekt chcemy umieścić na planszy.\n";
            userManual += "Aby umieścić wybrany obiekt na planszy wystarczy kliknąc w odpowiedni prostokąt na planszy.\n";
            userManual += "Następnie wystarczy kliknąć przycisk Start.\n\n\n";
            userManual += "ZASADY: \n";
            userManual += "Trawa rozmnaża się na wszystkich wolnych polach obok,\n";
            userManual += "reszta obiektów rozmnaza się gdy wolne pole ma dwóch sąsiadów.\n";
            userManual += "Obiekt zjada obiekt znajdujący się po lewej stronie na suwaku.\n";
            userManual += "Do zagłodzenia dochodzi gdy obiekt nic nie zje w danej rundzie.\n\n";
            userManual += "Celem gry jest utrzymanie wszystkich gatunków przy życiu przez jak najdłuszy czas";
            System.Windows.MessageBox.Show(userManual, "Instrukcja obsługi", MessageBoxButton.OKCancel, MessageBoxImage.Information);
        }
    }
    class Animal
    {
        public int priority, old, oldEat;
        public Image image;
        private readonly int maxOld = 7;
        private readonly int maxEat = 2;
        public bool eat = false;
       
        public Animal(int priority)
        {
            this.priority = priority;
            this.old = 0;
            this.oldEat = 0;
            switch (this.priority)
            {
                case 0:
                    this.image= global::Symulacja_świata.Properties.Resources.trawa;
                    break;//trawa
                case 1: 
                    this.image= global::Symulacja_świata.Properties.Resources.zając;
                    break;//zając
                case 2: 
                    this.image = global::Symulacja_świata.Properties.Resources.lis;
                    break;//lis
                case 3: 
                    this.image = global::Symulacja_świata.Properties.Resources.niedźwiedź;
                    break;//niedźwiedź
                case 4: 
                    this.image = global::Symulacja_świata.Properties.Resources.myśliwy;
                    break;//myśliwy
            }
        }
        public Animal(Animal animal)
        {
            this.priority = animal.priority;
            this.old = animal.old;
            this.oldEat = animal.oldEat;
            this.image = (Image)animal.image.Clone();
        }
        public void attack() { this.eat = true; this.oldEat = 0; }
        public bool round()
        {
            this.old++;
            this.oldEat++;
            if (this.old > this.maxOld)
            {
                return false;
            }
            if(this.oldEat > this.maxEat)
            {
                return false;
            }
            this.eat = false;
            return true;
        }
    }
    class Pixel
    {
        public int a, b;
        public bool isAlive = false;
        public Animal animal = null;
        public Pixel(int a, int b)
        {
            this.a = a;
            this.b = b;
        }
        public Pixel(int a, int b, Animal animal)
        {
            this.a = a;
            this.b = b;
            this.isAlive = true;
            this.animal = new Animal(animal);
        }
        public Pixel(Pixel pixel)
        {
            this.a = pixel.a;
            this.b = pixel.b;
            if (pixel.isAlive == true)
            {
                this.isAlive = true;
                this.animal = new Animal(pixel.animal);
            }
            else
            {
                this.isAlive = false;
            }
        }
        public void born() { this.isAlive = true; }
        public void born(int priority) { this.isAlive = true; this.animal = new Animal(priority); }
        public void kill() { 
            this.isAlive = false;
            this.animal = null;
        }
        public void setAnimal(Animal animal)
        {
            this.animal = animal;
            this.isAlive = true;
        }
        public void copy(Pixel pixel)
        {
            this.a = pixel.a;
            this.b = pixel.b;
            this.isAlive = pixel.isAlive;
            this.animal = pixel.animal;
        }
        public void copyCoordinates(Pixel pixel)
        {
            this.a = pixel.a;
            this.b = pixel.b;
        }
        public void clear()
        {
            this.animal = null;
            this.isAlive = false;
        }
    }
}
