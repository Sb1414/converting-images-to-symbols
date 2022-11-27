using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace convertImages
{
    public partial class Form1 : Form
    {
        private const double WIDTH_OFFSET = 1.5;  // для компенсации ширины
        private bool flag = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Point lastPoint;
        private void panelUp_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panelUp_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }

        private void buttonMinimiz_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void buttonMaximiz_Click(object sender, EventArgs e)
        {
            if (!flag)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
            flag = !flag;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Images | *.bmp; *.png; *.jpg; *.JPEG"
            };

            // openFileDialog.ShowDialog();
             if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var bitmap = new Bitmap(openFileDialog.FileName);
                bitmap = ResizeBitmap(bitmap);
                bitmap.ToCrayscale(); // конвертация в ч/б
                var converter = new BitmapToASCII(bitmap);
                var rows = converter.Convert();

                string path = @"array.txt";
                using (StreamWriter sw = new StreamWriter(path))
                {
                    foreach (var row in rows)
                    {
                        sw.WriteLine(row);
                    }             
                    sw.Close();
                }
                // string allText = File.ReadAllText(path, Encoding.UTF8);

                string[] lines = File.ReadAllLines(path);
                char[,] num = new char[lines.Length, lines[0].Split(' ').Length];
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] temp = lines[i].Split(' ');
                    for (int j = 0; j < temp.Length; j++)
                        num[i, j] = Convert.ToChar(temp[j]);
                }
                // проверяем выводом на консоль
                for (int i = 0; i < num.GetLength(0); i++)

                    for (int j = 0; j < num.GetLength(1); j++)
                        richTextBox1.Text += num[i, j];


                // richTextBox1.Clear();
                // richTextBox1.LoadFile("array.txt");

            }
        }

        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            var maxWidth = 350;
            var newHeight = bitmap.Height / WIDTH_OFFSET * maxWidth / bitmap.Width;
            if (bitmap.Width > maxWidth || bitmap.Height > newHeight)
            {
                bitmap = new Bitmap(bitmap, new Size(maxWidth, (int)newHeight));
            }
            return bitmap;
        }
    }
}
