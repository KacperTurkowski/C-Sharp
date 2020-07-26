using System;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace SłownieOkienko
{
    public partial class Słownie : Form
    {
        private static string[] unity = { "", "jeden ", "dwa ", "trzy ", "cztery ", "pięć ", "sześć ", "siedem ", "osiem ", "dziewięć " };
        private static string[] teen = { "", "jedenaście ", "dwanaście ", "trzynaście ", "czternaście ", "piętnaście ", "szesnaście ", "siedemnaście ", "osiemnaście ", "dziewiętnaście " };
        private static string[] dozens = { "", "dziesięć ", "dwadzieścia ", "trzydzieści ", "czterdzieści ", "pięćdziesiąt ", "sześćdziesiąt ", "siedemdziesiąt ", "osiemdziesiąt ", "dziewięćdziesiąt " };
        private static string[] hundreds = { "", "sto ", "dwieście ", "trzysta ", "czterysta ", "pięćset ", "sześćset ", "siedemset ", "osiemset ", "dziewięćset " };
        protected static string[,] variety = new string[,]
                {
                { "", "", ""                           },
                { "tysiąc ", "tysiące ", "tysięcy "       },
                { "milion " , "miliony " , "milionów "    },
                { "miliard ", "miliardy ", "miliardów "   },
                { "bilion " , "biliony " , "bilionów "    },
                { "biliard ", "biliardy ", "biliardów "   },
                { "trylion " , "tryliony ", "trylionów " },
                { "tryliard ", "tryliardy ", "tryliardów "},
                { "kwadrylion ", "kwadryliony ", "kwadrylionów " },
                { "kwadryliard ", "kwadryliardy ", "kwadryliardów " },
                { "kwintylion ", "kwintyliony ", "kwintylionów " },
                { "kwintyliard ", "kwintyliardy ", "kwintyliardów"},
                { "sekstylion ","sekstyliony ", "sekstylionów "},
                { "sekstyliard ","sekstyliardy ", "sekstyliardów" },
                { "septylion ","septyliony ","septylionów "},
                { "septyliard ","septyliardów ","septyliardów " },
                { "oktylion ","oktyliony ","oktylionów "},
                { "oktyliard ","oktyliardy ","oktyliardów " },
                { "nonylion ","nonyliony ","nonylionów "},
                { "nonyliard ","nonyliardy ","nonyliardów " },
                { "decylion ","decyliony ","decylionów "},
                { "decyliard ","decyliardy ","decyliardów " },
                {"wicylion ","wicyliony ","wicylionów " },
                {"wicyliard ","wicyliardy ","wicyliardów " },
                {"trycylion ","trycyliony ","trycylionów " },
                {"trycliard ","trycyliardy ","trycyliardów " },
                {"kwadragilion ","kwadragiliony ","kwadragilionów " },
                {"kwadragiliard ","kwadragiliardy ","kwadragiliardów " },
                {"kwinkwagilion ","kwinkwagiliony ","kwinkwagilionów " },
                {"kwinkwagiliard ","kwinkwagiliardy ","kwinkwagiliardów " },
                {"seskwilion ","seskwiliony ","seskwilionów " },
                {"seskwiliard ","seskwiliardy ","seskwiliardów " },
                {"septagilion ","septagiliony ","septagilionów " },
                {"septagiliard ","septagiliardy ","septagiliardów " },
                {"oktogilion ","oktogiliony ","oktogilionów " },
                {"oktogiliard ","oktogiliardy ","oktogiliardów " },
                {"nonagilion ","nonagiliony ","nonagilionów " },
                {"nonagiliard ","nonagiliardy ","nonagiliardów " },
                };
        string caption = "Error";
        MessageBoxButton button = MessageBoxButton.OK;
        MessageBoxImage icon = MessageBoxImage.Warning;
        private string słownie(string number)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                if(number[0]=='-')
                {
                    result.Append("minus ");
                    number = number.Substring(1, number.Length - 1);
                }
            }catch(IndexOutOfRangeException) { /*Empty Text Box*/ return ""; }
            bool check = true;
            for (int i = 0; i < number.Length; i++) if (number[i] != '0') check = false;
            if (check) return "zero";
            string a, b, c;
            string[] three;
            if (number.Length % 3 == 0) three = new string[number.Length / 3];
            else three = new string[(number.Length / 3) + 1];
            int index = 0;
            while (number.Length-3 > 0)
            {
                a = number.Substring(number.Length - 1);
                number = number.Substring(0, number.Length - 1);

                b = number.Substring(number.Length - 1);
                number = number.Substring(0, number.Length - 1);

                c = number.Substring(number.Length - 1);
                number = number.Substring(0, number.Length - 1);

                three[index] = c + "" + b + "" + a;
                index++;
            }
            while (number.Length > 0)
            {
                a = number.Substring(number.Length - 1);
                number = number.Substring(0, number.Length - 1);
                three[index] = a + "" + three[index];
            }
            int[] array = new int[three.Length];
            for (int i = 0; i < three.Length; i++)
            {
                try
                {
                    array[i] = int.Parse(three[i]);
                }
                catch (FormatException e)
                {
                    System.Windows.MessageBox.Show(e.Message, caption, button, icon);
                    return ""; 
                }
            }
            for (int i = three.Length - 1; i >= 0; i--)
            {
                result.Append(GetNumber(array, i));
            }
            return result.ToString();
        }
        private string GetNumber(int[] k, int i)
        {
            StringBuilder str = new StringBuilder();
            int three, second, one;
            three = k[i] / 100;
            second = (k[i] / 10) - three * 10;
            one = k[i] - second * 10 - three * 100;
            if (k[i]-three*100>=11 && k[i]-three*100<=19) str.Append(hundreds[three] + teen[one]);
            else str.Append(hundreds[three] + dozens[second] + unity[one]);
            try
            {
                if (k[i] == 0) return "";
                else if (k[i] == 1) str.Append(variety[i, 0]);
                else if (k[i] >= 5 && k[i] <= 21) str.Append(variety[i, 2]);
                else if (one >= 2 && one <= 4) str.Append(variety[i, 1]);
                else str.Append(variety[i, 2]);
            }catch(IndexOutOfRangeException e) { System.Windows.MessageBox.Show(e.Message, caption, button, icon); }
            return str.ToString();
        }
        public Słownie()
        {
            InitializeComponent();
            this.button1.Text = "RESET";
            this.Text = "Słownie";
            textBox1.Font = new Font(textBox1.Font.FontFamily, 16);
            textBox1.Height = 16;
            richTextBox1.Font = new Font(richTextBox1.Font.FontFamily, 16);
        }

        private void Form1_Load(object sender, EventArgs e){}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            string Text = słownie(this.textBox1.Text);
            this.CreateMyMultilineTextBox(Text);
        }

        public void CreateMyMultilineTextBox(String Text)
        {
            richTextBox1.Multiline = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Vertical;
            richTextBox1.AcceptsTab = true;
            richTextBox1.WordWrap = true;
            richTextBox1.Text = Text;
        }

        private void RESET_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "";
            this.textBox1.Text = "";
        }
    }

}
