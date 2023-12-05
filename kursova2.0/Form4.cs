using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kursova2._0
{
    public partial class Form4 : Form
    {
        private ToolTip toolTip1;
        public Form4()
        {
            InitializeComponent();
            toolTip1 = new ToolTip();
            toolTip1.SetToolTip(button1, "Нажмите, чтобы закрыть форму");
        }

        public void SetInfoText(string text)
        {
            infoLabel.Text = text;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Создайте новый экземпляр Form1
            Form1 form1 = new Form1();

            // Закройте текущую форму (Form2)
            this.Close();
        }

        private void infoLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
