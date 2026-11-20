using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace GestionLaboratoire
{
    partial class FormMenuPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitre;
        private Button btnPatients;
        private Button btnAnalyses;
        private Button btnFermer;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitre = new Label();
            btnPatients = new Button();
            btnAnalyses = new Button();
            btnFermer = new Button();

            SuspendLayout();

            // lblTitre
            lblTitre.AutoSize = true;
            lblTitre.Font = new System.Drawing.Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitre.Location = new Point(75, 22);
            lblTitre.Text = "Menu Principal - Laboratoire";

            // btnPatients
            btnPatients.Location = new Point(95, 72);
            btnPatients.Size = new Size(210, 42);
            btnPatients.Text = "Gestion des &Patients";
            btnPatients.Click += btnPatients_Click;

            // btnAnalyses
            btnAnalyses.Location = new Point(95, 126);
            btnAnalyses.Size = new Size(210, 42);
            btnAnalyses.Text = "Gestion des &Analyses";
            btnAnalyses.Click += btnAnalyses_Click;

            // btnFermer
            btnFermer.Location = new Point(140, 185);
            btnFermer.Size = new Size(120, 36);
            btnFermer.Text = "&Fermer";
            btnFermer.Click += btnFermer_Click;

            // FormMenuPrincipal
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(400, 250);
            Controls.Add(lblTitre);
            Controls.Add(btnPatients);
            Controls.Add(btnAnalyses);
            Controls.Add(btnFermer);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "FormMenuPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Menu Principal";

            ResumeLayout(false);
            PerformLayout();
        }
    }
}