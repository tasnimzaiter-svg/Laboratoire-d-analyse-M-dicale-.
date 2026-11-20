using System;
using System.Linq;
using System.Windows.Forms;
using GestionLaboratoire;
using Microsoft.EntityFrameworkCore;
// Adaptez le namespace de vos entités et de LaboContext si nécessaire :
// using GestionLaboratoire.Models;
// using GestionLaboratoire.Data;

namespace GestionLaboratoire
{
    public partial class FormPatients : Form
    {
        private int? _selectedPatientId = null;

        public FormPatients()
        {
            InitializeComponent();
        }

        private void FormPatients_Load(object sender, EventArgs e)
        {
            ChargerPatients();
        }

        private void ChargerPatients()
        {
            try
            {
                using var db = new LaboContext();
                var liste = db.Patients
                    .Include(p => p.assurance)
                    .Select(p => new
                    {
                        p.idPatient,
                        p.nom,
                        p.prenom,
                        p.telephone,
                        p.idAssurance,
                        Assurance = p.assurance != null ? p.assurance.nomAssurance : "Aucune"
                    })
                    .ToList();

                dgvPatients.DataSource = liste;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du chargement des patients : {ex.InnerException?.Message ?? ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Si le texte est vide -> retourne null.
        /// Si l'assurance existe déjà (insensible à la casse) -> retourne son ID.
        /// Sinon -> crée l'assurance en base et retourne son nouvel ID.
        /// </summary>
        private int? ResoudreIdAssurance(LaboContext db, string saisieAssurance)
        {
            string nomNettoye = saisieAssurance.Trim();
            if (string.IsNullOrWhiteSpace(nomNettoye) || nomNettoye.Equals("Aucune", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var existante = db.Assurances
                .FirstOrDefault(a => a.nomAssurance.ToLower() == nomNettoye.ToLower());

            if (existante != null)
            {
                return existante.idAssurance;
            }

            var nouvelleAssurance = new Assurance
            {
                nomAssurance = nomNettoye
            };
            db.Assurances.Add(nouvelleAssurance);
            db.SaveChanges();

            return nouvelleAssurance.idAssurance;
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text))
            {
                MessageBox.Show("Veuillez saisir au minimum le nom et le prénom du patient.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var db = new LaboContext();
                int? idAssurance = ResoudreIdAssurance(db, txtAssurance.Text);

                var patient = new Patient
                {
                    nom = txtNom.Text.Trim(),
                    prenom = txtPrenom.Text.Trim(),
                    telephone = txtTelephone.Text.Trim(),
                    idAssurance = idAssurance
                };

                db.Patients.Add(patient);
                db.SaveChanges();

                MessageBox.Show("Patient ajouté avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ViderChamps();
                ChargerPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ajout : {ex.InnerException?.Message ?? ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (_selectedPatientId == null)
            {
                MessageBox.Show("Veuillez sélectionner un patient dans le tableau.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNom.Text) || string.IsNullOrWhiteSpace(txtPrenom.Text))
            {
                MessageBox.Show("Le nom et le prénom sont obligatoires.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var db = new LaboContext();
                var patient = db.Patients.Find(_selectedPatientId.Value);
                if (patient == null) return;

                patient.nom = txtNom.Text.Trim();
                patient.prenom = txtPrenom.Text.Trim();
                patient.telephone = txtTelephone.Text.Trim();
                patient.idAssurance = ResoudreIdAssurance(db, txtAssurance.Text);

                db.SaveChanges();

                MessageBox.Show("Patient modifié avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ViderChamps();
                ChargerPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la modification : {ex.InnerException?.Message ?? ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (_selectedPatientId == null)
            {
                MessageBox.Show("Veuillez sélectionner un patient à supprimer.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Voulez-vous vraiment supprimer ce patient ?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            try
            {
                using var db = new LaboContext();
                var patient = db.Patients.Find(_selectedPatientId.Value);
                if (patient != null)
                {
                    db.Patients.Remove(patient);
                    db.SaveChanges();
                }

                ViderChamps();
                ChargerPatients();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Impossible de supprimer ce patient (il est peut-être lié à des analyses) : {ex.InnerException?.Message ?? ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVider_Click(object sender, EventArgs e)
        {
            ViderChamps();
        }

        private void ViderChamps()
        {
            _selectedPatientId = null;
            txtNom.Clear();
            txtPrenom.Clear();
            txtTelephone.Clear();
            txtAssurance.Clear();
            dgvPatients.ClearSelection();
        }

        private void dgvPatients_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgvPatients.Rows[e.RowIndex];
            _selectedPatientId = Convert.ToInt32(row.Cells["idPatient"].Value);
            txtNom.Text = row.Cells["nom"].Value?.ToString() ?? "";
            txtPrenom.Text = row.Cells["prenom"].Value?.ToString() ?? "";
            txtTelephone.Text = row.Cells["telephone"].Value?.ToString() ?? "";

            string assurance = row.Cells["Assurance"].Value?.ToString() ?? "";
            txtAssurance.Text = assurance == "Aucune" ? "" : assurance;
        }
    }
}