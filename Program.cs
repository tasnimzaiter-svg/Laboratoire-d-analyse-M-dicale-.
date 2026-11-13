using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GestionLaboratoire
{
    // =========================================================
    // 1. UTILISATEURS (HÉRITAGE)
    // =========================================================

    public class Utilisateur
    {
        public int idUtilisateur { get; set; }
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
        public List<Patient> patientsEnregistres { get; set; } = new List<Patient>();
        public List<Echantillon> echantillonsEnregistres { get; set; } = new List<Echantillon>();
        public List<Analyse> analysesEnregistrees { get; set; } = new List<Analyse>();
        public List<Paiement> paiementsEnregistres { get; set; } = new List<Paiement>();

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
        public List<Echantillon> echantillonsSuivis { get; set; } = new List<Echantillon>();
        public List<Analyse> analysesTraitees { get; set; } = new List<Analyse>();

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
        public List<Facture> facturesEtablies { get; set; } = new List<Facture>();
        public List<Paiement> paiementsEnregistres { get; set; } = new List<Paiement>();
        public List<Assurance> assurancesGerees { get; set; } = new List<Assurance>();

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
        public List<Utilisateur> utilisateursGeres { get; set; } = new List<Utilisateur>();

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