using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace paint
{
    public partial class Form1: System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
            bm = new Bitmap(pic1.Width, pic1.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            pic1.Image = bm;
        }
        Bitmap bm;
        Graphics g;
        bool paint = false;
        Point px, py;
        Pen p = new Pen(Color.Black, 1);
        Pen er = new Pen(Color.White, 4);
        int select;
        Color new_color;
       

        private void button1_Click(object sender, EventArgs e)
        {
            select = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            select = 3;
            panel2.Visible = false;
            button16.Visible = false;
            button17.Visible = true;


        }

        private void button5_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.ShowDialog();
            p.Color = cd.Color;
            button6.BackColor = cd.Color;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pic1.Size = this.ClientSize;
            pic1.BackColor = Color.White;
            bm = new Bitmap(pic1.Width, pic1.Height);
            g = Graphics.FromImage(bm);
            pic1.Image = bm;
            button16.Visible = false;
        }
 
        private void pic1_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            py = e.Location;

            if (select == 5)
            {
                if (pic1.Image == null) return;

                new_color = p.Color;

                if (bm.GetPixel(e.X, e.Y) == Color.White)
                {
                    MessageBox.Show("الرجاء النقر داخل الشكل!", "تحذير", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                FloodFill(bm, new Point(e.X, e.Y), new_color);
                pic1.Refresh();
            }
            
        }

        private void pic1_MouseMove(object sender, MouseEventArgs e)
        {
            if (paint)
            {
                if (select == 1)
                {
                    px = e.Location;
                    g.DrawLine(p, py, px);
                    py = px;
                }
                if (select == 2)
                {
                    px = e.Location;
                    g.DrawLine(er, py, px);
                    py = px;
                }
                if (select == 4)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    SolidBrush brush = new SolidBrush(Color.FromArgb(50, p.Color));
                    int brushSize = (int)p.Width * 2;
                    g.FillEllipse(brush, e.X - brushSize / 2, e.Y - brushSize / 2, brushSize, brushSize);
                }
                if (select != 10)
                {
                    panel2.Visible = false;
                }

                pic1.Refresh();
            }
        }

        private void pic1_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            if(select == 3)
            {
                g.DrawLine(p, py, e.Location);
                
            }
            

            if (select == 6)
            {
                g.DrawEllipse(p, Math.Min(py.X, e.X), Math.Min(py.Y, e.Y), Math.Abs(e.X - py.X), Math.Abs(e.Y - py.Y));
            }
            if(select == 7)
            {
                SolidBrush brushE = new SolidBrush(p.Color);
                g.FillEllipse(brushE, Math.Min(py.X, e.X), Math.Min(py.Y, e.Y), Math.Abs(e.X - py.X), Math.Abs(e.Y - py.Y));
            }
            if (select == 8)
            {
                g.DrawRectangle(p, Math.Min(py.X, e.X), Math.Min(py.Y, e.Y), Math.Abs(e.X - py.X), Math.Abs(e.Y - py.Y));
            }
            if (select == 9)
            {
                SolidBrush brushR = new SolidBrush(p.Color);
                g.FillRectangle(brushR, Math.Min(py.X, e.X), Math.Min(py.Y, e.Y), Math.Abs(e.X - py.X), Math.Abs(e.Y - py.Y));
            }
            pic1.Refresh();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            select = 2;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            select = 4;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
            pic1.Refresh();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
            saveDialog.Title = "حفظ الصورة";
            saveDialog.FileName = "MyDrawing";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap savedImage = new Bitmap(bm.Width, bm.Height);
                using (Graphics g = Graphics.FromImage(savedImage))
                {
                    g.Clear(Color.White); // جعل الخلفية بيضاء
                    g.DrawImage(bm, 0, 0); // رسم الصورة فوق الخلفية البيضاء
                }

                savedImage.Save(saveDialog.FileName);
                MessageBox.Show("تم حفظ الصورة بنجاح!", "تم الحفظ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void button9_Click_1(object sender, EventArgs e)
        {
            FormAbout aboutForm = new FormAbout();
            aboutForm.ShowDialog();
        }
        private void FloodFill(Bitmap bmp, Point pt, Color newColor)
        {
            Color oldColor = bmp.GetPixel(pt.X, pt.Y);
            if (oldColor.ToArgb() == newColor.ToArgb()) return;

            Stack<Point> pixels = new Stack<Point>();
            pixels.Push(pt);

            while (pixels.Count > 0)
            {
                Point p = pixels.Pop();
                if (p.X < 0 || p.Y < 0 || p.X >= bmp.Width || p.Y >= bmp.Height)
                    continue;

                if (bmp.GetPixel(p.X, p.Y) == oldColor)
                {
                    bmp.SetPixel(p.X, p.Y, newColor);

                    pixels.Push(new Point(p.X + 1, p.Y));
                    pixels.Push(new Point(p.X - 1, p.Y));
                    pixels.Push(new Point(p.X, p.Y + 1));
                    pixels.Push(new Point(p.X, p.Y - 1));
                }
            }
        }


        private void button10_Click(object sender, EventArgs e)
        {
            select = 5;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            select = 10; 
            panel2.Visible = !panel2.Visible;
            if (panel2.Visible)
            {
                button16.Visible = true;
                button17.Visible = false;
            }
            else
            {
                button16.Visible = false;
                button17.Visible = true;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            select = 6;
            panel2.Visible = false;
            button16.Visible = false;
            button17.Visible = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            select = 7;
            panel2.Visible = false;
            button16.Visible = false;
            button17.Visible = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            select = 8;
            panel2.Visible = false;
            button16.Visible = false;
            button17.Visible = true;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            select = 9;
            panel2.Visible = false;
            button16.Visible = false;
            button17.Visible = true;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            button17.Visible = true;
            button16.Visible = false;
        }

        private void button17_Click(object sender, EventArgs e)
        {

        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            select = 11;
            panel2.Visible = !panel2.Visible;
            button17.Visible = false;
            button16.Visible = true;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            p.Width = trackBar1.Value;
            er.Width = trackBar1.Value;
            
        }
       
       
    }
}
