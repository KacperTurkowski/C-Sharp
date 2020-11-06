using System;
using Xamarin.Forms;

namespace TTT
{
    public partial class MainPage : ContentPage
    {
        bool queue = false;//True -> x, False -> o
        public MainPage()
        {
            InitializeComponent();
            draw();
        }
        private void draw()
        {
            Random rand = new Random();
            int x = rand.Next();
            if (x % 2 == 0)
            {
                queue = true;
                this.Information.Text = "Zaczyna x";
            }
            else
            {
                queue = false;
                this.Information.Text = "Zaczyna o";
            }
        }
        private string getChar()
        {
            if (queue == true)
            {
                queue = !queue;
                return "X";
            }
            else
            {
                queue = !queue;
                return "O";
            }
        }
        private void check()
        {
            if (Button00.Text == Button01.Text && Button00.Text == Button02.Text && Button00.Text != "") this.Information.Text = "Wygrał " + Button00.Text;
            else if (Button10.Text == Button11.Text && Button10.Text == Button12.Text && Button10.Text != "") this.Information.Text = "Wygrał " + Button10.Text;
            else if (Button20.Text == Button21.Text && Button20.Text == Button22.Text && Button20.Text != "") this.Information.Text = "Wygrał " + Button20.Text;

            else if (Button00.Text == Button10.Text && Button00.Text == Button20.Text && Button00.Text != "") this.Information.Text = "Wygrał " + Button00.Text;
            else if (Button01.Text == Button11.Text && Button01.Text == Button21.Text && Button01.Text != "") this.Information.Text = "Wygrał " + Button01.Text;
            else if (Button02.Text == Button12.Text && Button02.Text == Button22.Text && Button02.Text != "") this.Information.Text = "Wygrał " + Button02.Text;

            else if (Button00.Text == Button11.Text && Button00.Text == Button22.Text && Button00.Text != "") this.Information.Text = "Wygrał " + Button00.Text;
            else if (Button20.Text == Button11.Text && Button20.Text == Button02.Text && Button20.Text != "") this.Information.Text = "Wygrał " + Button20.Text;
        }
        private void Button00_Clicked(object sender, EventArgs e)
        {
            if(this.Button00.Text == "")
                this.Button00.Text = this.getChar();
            check();
        }
        private void Button10_Clicked(object sender, EventArgs e)
        {
            if (this.Button10.Text == "")
                this.Button10.Text = this.getChar();
            check();
        }

        private void Button20_Clicked(object sender, EventArgs e)
        {
            if (this.Button20.Text == "")
                this.Button20.Text = this.getChar();
            check();
        }

        private void Button01_Clicked(object sender, EventArgs e)
        {
            if (this.Button01.Text == "")
                this.Button01.Text = this.getChar();
            check();
        }

        private void Button11_Clicked(object sender, EventArgs e)
        {
            if (this.Button11.Text == "")
                this.Button11.Text = this.getChar();
            check();
        }

        private void Button21_Clicked(object sender, EventArgs e)
        {
            if (this.Button21.Text == "")
                this.Button21.Text = this.getChar();
            check();
        }

        private void Button02_Clicked(object sender, EventArgs e)
        {
            if (this.Button02.Text == "")
                this.Button02.Text = this.getChar();
            check();
        }

        private void Button12_Clicked(object sender, EventArgs e)
        {
            if (this.Button12.Text == "")
                this.Button12.Text = this.getChar();
            check();
        }

        private void Button22_Clicked(object sender, EventArgs e)
        {
            if (this.Button22.Text == "")
                this.Button22.Text = this.getChar();
            check();
        }

        private void ResetButton_Clicked(object sender, EventArgs e)
        {
            Button00.Text = "";
            Button01.Text = "";
            Button02.Text = "";

            Button10.Text = "";
            Button11.Text = "";
            Button12.Text = "";

            Button20.Text = "";
            Button21.Text = "";
            Button22.Text = "";

            draw();
        }
    }
}
