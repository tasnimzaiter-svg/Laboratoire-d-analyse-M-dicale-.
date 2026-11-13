public class Patient
{
    public int idPatient { get; set; }
    public string nom { get; set; }
    public string prenom { get; set; }
    public string telephone { get; set; }

    // Relations
    public Assurance assurance { get; set; } // 0..1
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
    public int idMedecin { get; set; }
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
    public string codeBarres { get; set; }
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
    public int idAnalyse { get; set; }
    public string libelle { get; set; }
    public double tarif { get; set; }
    public DateTime dateDemande { get; set; }
    public string statut { get; set; } = "En attente";
    public string valeur { get; set; }
    public DateTime dateSaisie { get; set; }
    public string modeSaisie { get; set; }
    public bool valide { get; set; } = false;

    // Relations (côté "1" ou "0..1")
    public Patient patient { get; set; }
    public Medecin medecin { get; set; }
    public Echantillon echantillon { get; set; }
    public Facture facture { get; set; }
}

public class Assurance
{
    public int idAssurance { get; set; }
    public string nomAssurance { get; set; }
    public double tauxPriseEnCharge { get; set; } // Exemple : 0.70 pour 70%

    public double calculerPartAssurance(double montantTotal)
    {
        return montantTotal * tauxPriseEnCharge;
    }
}

public class Facture
{
    public int numFacture { get; set; }
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
    public int idPaiement { get; set; }
    public DateTime datePaiement { get; set; }
    public double montant { get; set; }
    public string modePaiement { get; set; }
}