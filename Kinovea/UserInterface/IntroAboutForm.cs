using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kinovea.Root.Languages;

namespace Kinovea.Root
{
    public partial class IntroAboutForm : Form
    {
        public bool close = false;
        public IntroAboutForm()
        {
            InitializeComponent();

            btnAgreement.FlatStyle = FlatStyle.Flat;
            btnAgreement.FlatAppearance.BorderSize = 1;

        }

        private void btnAgreement_Click(object sender, EventArgs e)
        {
            
            this.Hide();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            FormAboutCH about = new FormAboutCH();
            about.ShowDialog();
        }

        private void IntroAboutForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            

        }

        private void IntroAboutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show(this, "Do you want to close the Application ? ", "Closing...",MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)                
                {
                        close = false;
                        e.Cancel = true;
                }                   
                else
                { 
                    Application.Exit();
                    close = true;
                }
            }
        }

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            FormInstructions formInst = new FormInstructions();
            formInst.ShowDialog();
        }
    }
}
