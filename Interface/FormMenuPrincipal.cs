using System;
using System.Windows.Forms;

namespace GestionLaboratoire
{
    public partial class FormMenuPrincipal : Form
    {
        public FormMenuPrincipal()
        {
            InitializeComponent();
        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            using (var frm = new FormPatients())
            {
                this.Hide();
                frm.ShowDialog();
                this.Show();
            }
        }

        private void btnAnalyses_Click(object sender, EventArgs e)
        {
            using (var frm = new FormAnalyses())
            {
                this.Hide();
                frm.ShowDialog();
                this.Show();
            }
        }

        private void btnFermer_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}