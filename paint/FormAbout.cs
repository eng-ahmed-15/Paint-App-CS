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

namespace paint
{
    public partial class FormAbout: Form
    {
        public FormAbout()
        {
            InitializeComponent();
        }
        private void FormAbout_Load(object sender , EventArgs e)
        {
            this.Text = "About";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            // إنشاء PictureBox
            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // تحويل الصورة من Resources وتحميلها داخل PictureBox
            using (MemoryStream ms = new MemoryStream(Properties.Resources.Add2_))
            {
                pictureBox.Image = Image.FromStream(ms);
            }

            // ضبط حجم النافذة ليناسب الصورة
            this.ClientSize = pictureBox.Image.Size;

            // إضافة PictureBox إلى النافذة
            this.Controls.Add(pictureBox);
        }
    }
}
