using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace GestionLaboratoire
{
    partial class FormAnalyses
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblLibelle;
        private Label lblTarif;
        private Label lblStatut;
        private Label lblPatient;

        private TextBox txtLibelle;
        private TextBox txtTarif;
        private ComboBox cmbStatut;
        private ComboBox cmbPatient;

        private Button btnAjouter;
        private Button btnModifier;
        private Button btnSupprimer;
        private Button btnVider;

        private DataGridView dgvAnalyses;

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
            lblLibelle = new Label();
            lblTarif = new Label();
            lblStatut = new Label();
            lblPatient = new Label();

            txtLibelle = new TextBox();
            txtTarif = new TextBox();
            cmbStatut = new ComboBox();
            cmbPatient = new ComboBox();

            btnAjouter = new Button();
            btnModifier = new Button();
            btnSupprimer = new Button();
            btnVider = new Button();

            dgvAnalyses = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)(dgvAnalyses)).BeginInit();
            SuspendLayout();

            // lblLibelle
            lblLibelle.AutoSize = true;
            lblLibelle.Location = new Point(25, 25);
            lblLibelle.Text = "Libellé :";

            // txtLibelle
            txtLibelle.Location = new Point(105, 22);
            txtLibelle.Size = new Size(210, 27);

            // lblTarif
            lblTarif.AutoSize = true;
            lblTarif.Location = new Point(345, 25);
            lblTarif.Text = "Tarif (DT) :";

            // txtTarif
            txtTarif.Location = new Point(430, 22);
            txtTarif.Size = new Size(120, 27);

            // lblStatut
            lblStatut.AutoSize = true;
            lblStatut.Location = new Point(25, 65);
            lblStatut.Text = "Statut :";

            // cmbStatut
            cmbStatut.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatut.Location = new Point(105, 62);
            cmbStatut.Size = new Size(210, 28);

            // lblPatient
            lblPatient.AutoSize = true;
            lblPatient.Location = new Point(345, 65);
            lblPatient.Text = "Patient :";

            // cmbPatient
            cmbPatient.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPatient.Location = new Point(430, 62);
            cmbPatient.Size = new Size(210, 28);

            // btnAjouter
            btnAjouter.Location = new Point(25, 112);
            btnAjouter.Size = new Size(110, 35);
            btnAjouter.Text = "Ajouter";
            btnAjouter.Click += btnAjouter_Click;

            // btnModifier
            btnModifier.Location = new Point(145, 112);
            btnModifier.Size = new Size(110, 35);
            btnModifier.Text = "Modifier";
            btnModifier.Click += btnModifier_Click;

            // btnSupprimer
            btnSupprimer.Location = new Point(265, 112);
            btnSupprimer.Size = new Size(110, 35);
            btnSupprimer.Text = "Supprimer";
            btnSupprimer.Click += btnSupprimer_Click;

            // btnVider
            btnVider.Location = new Point(385, 112);
            btnVider.Size = new Size(110, 35);
            btnVider.Text = "Vider";
            btnVider.Click += btnVider_Click;

            // dgvAnalyses
            dgvAnalyses.AllowUserToAddRows = false;
            dgvAnalyses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAnalyses.Location = new Point(25, 165);
            dgvAnalyses.MultiSelect = false;
            dgvAnalyses.ReadOnly = true;
            dgvAnalyses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAnalyses.Size = new Size(760, 260);
            dgvAnalyses.CellClick += dgvAnalyses_CellClick;

            // FormAnalyses
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(810, 450);
            Controls.Add(lblLibelle);
            Controls.Add(txtLibelle);
            Controls.Add(lblTarif);
            Controls.Add(txtTarif);
            Controls.Add(lblStatut);
            Controls.Add(cmbStatut);
            Controls.Add(lblPatient);
            Controls.Add(cmbPatient);
            Controls.Add(btnAjouter);
            Controls.Add(btnModifier);
            Controls.Add(btnSupprimer);
            Controls.Add(btnVider);
            Controls.Add(dgvAnalyses);
            Name = "FormAnalyses";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Gestion des Analyses";
            Load += FormAnalyses_Load;

            ((System.ComponentModel.ISupportInitialize)(dgvAnalyses)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}