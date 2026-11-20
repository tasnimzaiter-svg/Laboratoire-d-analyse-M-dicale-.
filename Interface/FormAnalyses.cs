using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace GestionLaboratoire
{
    public partial class FormAnalyses : Form
    {
        // Garde l'id de l'analyse sélectionnée dans le tableau
        private int? idAnalyseSelectionnee = null;

        public FormAnalyses()
        {
            InitializeComponent();
        }

        // 1. Chargement initial de la fenêtre
        private void FormAnalyses_Load(object sender, EventArgs e)
        {
            InitialiserStatuts();
            ChargerPatients();
            ChargerAnalyses();
        }

        private void InitialiserStatuts()
        {
            cmbStatut.Items.Clear();
            cmbStatut.Items.AddRange(new object[] { "En attente", "En cours", "Analysé", "Validé" });
            cmbStatut.SelectedIndex = 0;
        }

        private void ChargerPatients()
        {
            using (var db = new LaboContext())
            {
                var listePatients = db.Patients
                    .Select(p => new
                    {
                        p.idPatient,
                        NomComplet = p.prenom + " " + p.nom
                    })
                    .ToList();

                cmbPatient.DataSource = listePatients;
                cmbPatient.DisplayMember = "NomComplet";
                cmbPatient.ValueMember = "idPatient";
                cmbPatient.SelectedIndex = -1;
            }
        }

        private void ChargerAnalyses()
        {
            using (var db = new LaboContext())
            {
                dgvAnalyses.DataSource = db.Analyses
                    .Include(a => a.patient)
                    .Select(a => new
                    {
                        a.idAnalyse,
                        a.libelle,
                        a.tarif,
                        a.statut,
                        a.dateDemande,
                        Patient = a.patient != null ? a.patient.prenom + " " + a.patient.nom : "Non assigné"
                    })
                    .ToList();
            }
        }

        // Convertit le texte du tarif en double (accepte la virgule ou le point)
        private bool EssayerLireTarif(out double tarif)
        {
            string texte = txtTarif.Text.Trim().Replace(',', '.');
            if (!double.TryParse(texte, NumberStyles.Any, CultureInfo.InvariantCulture, out tarif) || tarif < 0)
            {
                MessageBox.Show("Veuillez saisir un tarif valide (ex: 45.5).");
                tarif = 0;
                return false;
            }
            return true;
        }

        // 2. Bouton AJOUTER (Create)
        // 2. Bouton AJOUTER (Create)
        private void btnAjouter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLibelle.Text))
            {
                MessageBox.Show("Veuillez saisir le libellé de l'analyse.");
                return;
            }

            if (!EssayerLireTarif(out double tarif))
                return;

            try
            {
                using (var db = new LaboContext())
                {
                    Patient? patientChoisi = null;
                    if (cmbPatient.SelectedIndex >= 0 && cmbPatient.SelectedValue != null)
                    {
                        int idPat = Convert.ToInt32(cmbPatient.SelectedValue);
                        patientChoisi = db.Patients.Find(idPat);
                    }

                    if (patientChoisi == null)
                    {
                        MessageBox.Show("Veuillez sélectionner un patient.");
                        return;
                    }

                    var nouvelleAnalyse = new Analyse
                    {
                        libelle = txtLibelle.Text.Trim(),
                        tarif = tarif,
                        statut = cmbStatut.SelectedItem?.ToString() ?? "En attente",
                        dateDemande = DateTime.Now,
                        valeur = "",
                        modeSaisie = "Manuel",
                        patient = patientChoisi
                    };

                    db.Analyses.Add(nouvelleAnalyse);
                    db.SaveChanges();
                }

                ViderChamps();
                ChargerAnalyses();
                MessageBox.Show("Analyse ajoutée !");
            }
            catch (Exception ex)
            {
                string msg = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show("Erreur lors de l'ajout de l'analyse : " + msg);
            }
        }
        // 3. Clic sur une ligne du tableau
        private void dgvAnalyses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow ligne = dgvAnalyses.Rows[e.RowIndex];
            idAnalyseSelectionnee = Convert.ToInt32(ligne.Cells["idAnalyse"].Value);

            txtLibelle.Text = ligne.Cells["libelle"].Value?.ToString() ?? "";
            txtTarif.Text = ligne.Cells["tarif"].Value?.ToString() ?? "";
            cmbStatut.SelectedItem = ligne.Cells["statut"].Value?.ToString() ?? "En attente";
        }

        // 4. Bouton MODIFIER (Update)
        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (idAnalyseSelectionnee == null)
            {
                MessageBox.Show("Cliquez d'abord sur une analyse dans le tableau.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtLibelle.Text) || !EssayerLireTarif(out double tarif))
                return;

            using (var db = new LaboContext())
            {
                var analyse = db.Analyses.Find(idAnalyseSelectionnee.Value);
                if (analyse == null) return;

                analyse.libelle = txtLibelle.Text.Trim();
                analyse.tarif = tarif;
                analyse.statut = cmbStatut.SelectedItem?.ToString() ?? analyse.statut;

                if (cmbPatient.SelectedValue != null)
                {
                    int idPat = Convert.ToInt32(cmbPatient.SelectedValue);
                    analyse.patient = db.Patients.Find(idPat);
                }

                db.SaveChanges();
            }

            ViderChamps();
            ChargerAnalyses();
            MessageBox.Show("Analyse modifiée !");
        }

        // 5. Bouton SUPPRIMER (Delete)
        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (idAnalyseSelectionnee == null)
            {
                MessageBox.Show("Cliquez d'abord sur une analyse dans le tableau.");
                return;
            }

            using (var db = new LaboContext())
            {
                var analyse = db.Analyses.Find(idAnalyseSelectionnee.Value);
                if (analyse == null) return;

                db.Analyses.Remove(analyse);
                db.SaveChanges();
            }

            ViderChamps();
            ChargerAnalyses();
            MessageBox.Show("Analyse supprimée !");
        }

        // 6. Bouton VIDER
        private void btnVider_Click(object sender, EventArgs e)
        {
            ViderChamps();
        }

        private void ViderChamps()
        {
            idAnalyseSelectionnee = null;
            txtLibelle.Clear();
            txtTarif.Clear();
            cmbStatut.SelectedIndex = 0;
            cmbPatient.SelectedIndex = -1;
        }
    }
}