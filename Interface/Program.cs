using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionLaboratoire
{
    // =========================================================
    // 1. UTILISATEURS (HÉRITAGE)
    // =========================================================

    public class Utilisateur
    {

        [Key] public int idUtilisateur { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string login { get; set; }
        public string motDePasse { get; set; }

        public bool sAuthentifier(string loginSaisi, string mdpSaisi)
        {
            if (login == loginSaisi && motDePasse == mdpSaisi)
            {
                Console.WriteLine($"[Connexion OK] Bienvenue {prenom} {nom}");
                return true;
            }
            Console.WriteLine("[Erreur] Identifiants incorrects.");
            return false;
        }

        public void seDeconnecter()
        {
            Console.WriteLine($"{prenom} {nom} s'est déconnecté.");
        }
    }

    public class Secretaire : Utilisateur
    {
        public string matricule { get; set; }

        // Relations 0..*
        [NotMapped] public List<Patient> patientsEnregistres { get; set; } = new List<Patient>();
        [NotMapped] public List<Echantillon> echantillonsEnregistres { get; set; } = new List<Echantillon>();
        [NotMapped] public List<Analyse> analysesEnregistrees { get; set; } = new List<Analyse>();
        [NotMapped] public List<Paiement> paiementsEnregistres { get; set; } = new List<Paiement>();

        public void enregistrerPatient(Patient p)
        {
            patientsEnregistres.Add(p);
            Console.WriteLine($"Secrétaire {nom} a enregistré le patient {p.prenom} {p.nom}.");
        }

        public void enregistrerDemandeAnalyse(Analyse a)
        {
            analysesEnregistrees.Add(a);
            Console.WriteLine($"Secrétaire {nom} a enregistré la demande : {a.libelle}.");
        }

        public void enregistrerEchantillon(Echantillon e)
        {
            echantillonsEnregistres.Add(e);
            e.genererEtiquette();
            Console.WriteLine($"Secrétaire {nom} a enregistré l'échantillon {e.codeBarres}.");
        }

        public void enregistrerPaiement(Paiement p, Facture f)
        {
            paiementsEnregistres.Add(p);
            f.paiements.Add(p);
            Console.WriteLine($"Secrétaire {nom} a encaissé {p.montant} DT.");
        }
    }

    public class TechnicienLabo : Utilisateur
    {
        public string matricule { get; set; }
        public string specialite { get; set; }

        // Relations 0..*
        [NotMapped] public List<Echantillon> echantillonsSuivis { get; set; } = new List<Echantillon>();
        [NotMapped] public List<Analyse> analysesTraitees { get; set; } = new List<Analyse>();

        public void suivreStatutEchantillon(Echantillon e)
        {
            Console.WriteLine($"[Technicien {nom}] Statut de l'échantillon {e.codeBarres} : {e.statut}");
        }

        public void rejeterEchantillon(Echantillon e, string motif)
        {
            e.statut = "Rejeté";
            e.motifRejet = motif;
            echantillonsSuivis.Add(e);
            Console.WriteLine($"[ALERTE] Échantillon {e.codeBarres} rejeté par {nom}. Motif : {motif}");
        }

        public void saisirResultats(Analyse a, string resultat)
        {
            a.valeur = resultat;
            a.dateSaisie = DateTime.Now;
            a.modeSaisie = "Manuel";
            a.statut = "Analysé";
            analysesTraitees.Add(a);
            Console.WriteLine($"Technicien {nom} a saisi le résultat ({resultat}) pour {a.libelle}.");
        }

        public void importerResultatsAutomate(Analyse a, string resultatAutomate)
        {
            a.valeur = resultatAutomate;
            a.dateSaisie = DateTime.Now;
            a.modeSaisie = "Automate";
            a.statut = "Analysé";
            analysesTraitees.Add(a);
            Console.WriteLine($"Résultat importé depuis l'automate ({resultatAutomate}) pour {a.libelle}.");
        }

        public void validerResultats(Analyse a)
        {
            a.valide = true;
            Console.WriteLine($"Analyse '{a.libelle}' VALIDÉE par {nom}.");
        }

        public void editerRapport(Analyse a)
        {
            if (a.valide)
                Console.WriteLine($"=== RAPPORT D'ANALYSE ===\nPatient : {a.patient.nom} | Analyse : {a.libelle} | Résultat : {a.valeur}\n=========================");
            else
                Console.WriteLine("Impossible d'éditer le rapport : résultat non validé.");
        }
    }

    public class Comptable : Utilisateur
    {
        public string matricule { get; set; }

        // Relations 0..*
        [NotMapped] public List<Facture> facturesEtablies { get; set; } = new List<Facture>();
        [NotMapped] public List<Paiement> paiementsEnregistres { get; set; } = new List<Paiement>();
        [NotMapped] public List<Assurance> assurancesGerees { get; set; } = new List<Assurance>();

        public void etablirFacture(Facture f)
        {
            calculerMontant(f);
            gererPriseEnCharge(f);
            facturesEtablies.Add(f);
            Console.WriteLine($"Comptable {nom} a établi la facture N°{f.numFacture}.");
        }

        public void calculerMontant(Facture f)
        {
            f.montantTotal = 0;
            foreach (Analyse a in f.analyses)
            {
                f.montantTotal += a.tarif;
            }
        }

        public void gererPriseEnCharge(Facture f)
        {
            if (f.assurance != null)
            {
                f.partAssurance = f.assurance.calculerPartAssurance(f.montantTotal);
                assurancesGerees.Add(f.assurance);
            }
            else
            {
                f.partAssurance = 0;
            }
            f.partPatient = f.montantTotal - f.partAssurance;
        }

        public void enregistrerPaiement(Paiement p, Facture f)
        {
            paiementsEnregistres.Add(p);
            f.paiements.Add(p);
            f.statut = "Payée";
            Console.WriteLine($"Comptable {nom} a enregistré un paiement de {p.montant} DT pour la facture N°{f.numFacture}.");
        }

        public void suivreImpayes()
        {
            Console.WriteLine("--- Liste des factures impayées ---");
            foreach (Facture f in facturesEtablies)
            {
                if (f.statut != "Payée")
                    Console.WriteLine($"Facture N°{f.numFacture} - Reste à payer : {f.partPatient} DT");
            }
        }
    }

    public class Administrateur : Utilisateur
    {
        public string matricule { get; set; }

        // Relation 0..*
        [NotMapped] public List<Utilisateur> utilisateursGeres { get; set; } = new List<Utilisateur>();

        public void gererUtilisateurs(Utilisateur u)
        {
            utilisateursGeres.Add(u);
            Console.WriteLine($"Admin {nom} a ajouté/modifié l'utilisateur : {u.prenom} {u.nom}.");
        }

        public void gererDroits(Utilisateur u, string nouveauRole)
        {
            Console.WriteLine($"Admin {nom} a mis à jour les droits de {u.nom} vers : {nouveauRole}.");
        }

        public void gererCatalogue(string nomAnalyse, double nouveauTarif)
        {
            Console.WriteLine($"Admin {nom} a mis à jour le catalogue : {nomAnalyse} = {nouveauTarif} DT.");
        }

        public void genererStatistiques()
        {
            Console.WriteLine($"Admin {nom} génère les statistiques globales ({utilisateursGeres.Count} utilisateurs actifs).");
        }
    }

    // =========================================================
    // 2. CLASSES MÉTIER
    // =========================================================

    public class Patient
    {
        [Key] public int idPatient { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string telephone { get; set; }

        // Relations
        public int? idAssurance { get; set; }       // clé étrangère nullable (0..1)
        [ForeignKey(nameof(idAssurance))]
        public Assurance? assurance { get; set; }
        public List<Analyse> analyses { get; set; } = new List<Analyse>(); // 0..*

        public void consulterResultats(Analyse a)
        {
            if (a.valide)
                Console.WriteLine($"[Patient {nom}] Résultat de {a.libelle} : {a.valeur}");
            else
                Console.WriteLine($"[Patient {nom}] Résultat en cours...");
        }

        public void suivreStatutEchantillon(Echantillon e)
        {
            Console.WriteLine($"[Patient {nom}] État de votre prélèvement ({e.typeEchantillon}) : {e.statut}");
        }
    }

    public class Medecin
    {
        [Key] public int idMedecin { get; set; }
        public string nom { get; set; }
        public string specialite { get; set; }

        // Relation 0..*
        public List<Analyse> analysesPrescrites { get; set; } = new List<Analyse>();

        public void prescrireAnalyse(Analyse a)
        {
            a.medecin = this;
            analysesPrescrites.Add(a);
            Console.WriteLine($"Dr. {nom} ({specialite}) a prescrit l'analyse : {a.libelle}.");
        }

        public void consulterResultats(Analyse a)
        {
            if (a.valide)
                Console.WriteLine($"[Dr. {nom}] Consultation résultat {a.libelle} : {a.valeur}");
            else
                Console.WriteLine($"[Dr. {nom}] Résultat non encore validé.");
        }

        public void suivreStatutEchantillon(Echantillon e)
        {
            Console.WriteLine($"[Dr. {nom}] Statut échantillon {e.codeBarres} : {e.statut}");
        }
    }

    public class Echantillon
    {
        [Key] public string codeBarres { get; set; }
        public string typeEchantillon { get; set; }
        public DateTime dateReception { get; set; }
        public string statut { get; set; } = "Reçu";
        public string motifRejet { get; set; }

        // Relation 1..*
        public List<Analyse> analyses { get; set; } = new List<Analyse>();

        public void genererEtiquette()
        {
            Console.WriteLine($"[Code-barres généré] {codeBarres} ({typeEchantillon})");
        }
    }

    public class Analyse
    {
        [Key] public int idAnalyse { get; set; }
        public string libelle { get; set; }
        public double tarif { get; set; }
        public DateTime dateDemande { get; set; }
        public string statut { get; set; } = "En attente";
        public string valeur { get; set; }
        public DateTime dateSaisie { get; set; }
        public string modeSaisie { get; set; }
        public bool valide { get; set; } = false;

        // Relations (côté "1" ou "0..1")
        public Patient? patient { get; set; }
        public Medecin? medecin { get; set; }
        public Echantillon? echantillon { get; set; }
        public Facture? facture { get; set; }
    }

    public class Assurance
    {
        [Key] public int idAssurance { get; set; }
        public string nomAssurance { get; set; }
        public double tauxPriseEnCharge { get; set; } // Exemple : 0.70 pour 70%

        public double calculerPartAssurance(double montantTotal)
        {
            return montantTotal * tauxPriseEnCharge;
        }
    }

    public class Facture
    {
        [Key] public int numFacture { get; set; }
        public DateTime dateFacture { get; set; }
        public double montantTotal { get; set; }
        public double partAssurance { get; set; }
        public double partPatient { get; set; }
        public string statut { get; set; } = "Non payée";

        // Relations
        public Assurance assurance { get; set; } // 0..1
        public List<Analyse> analyses { get; set; } = new List<Analyse>(); // 1..*
        public List<Paiement> paiements { get; set; } = new List<Paiement>(); // 0..*
    }

    public class Paiement
    {
        [Key] public int idPaiement { get; set; }
        public DateTime datePaiement { get; set; }
        public double montant { get; set; }
        public string modePaiement { get; set; }
    }
    namespace GestionLaboratoire
    {
        internal static class Program
        {
            [STAThread]
            static void Main()
            {
                ApplicationConfiguration.Initialize();
                Application.Run(new FormMenuPrincipal());
            }
        }
    }
}