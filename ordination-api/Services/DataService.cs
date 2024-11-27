using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using shared.Model;
using static shared.Util;
using Data;

namespace Service;

public class DataService
{
    private OrdinationContext db { get; }

    public DataService(OrdinationContext db) {
        this.db = db;
    }

    /// <summary>
    /// Seeder noget nyt data i databasen, hvis det er nødvendigt.
    /// </summary>
    public void SeedData() {

        // Patients
        Patient[] patients = new Patient[5];
        patients[0] = db.Patienter.FirstOrDefault()!;

        if (patients[0] == null)
        {
            patients[0] = new Patient("121256-0512", "Jane Jensen", 63.4);
            patients[1] = new Patient("070985-1153", "Finn Madsen", 83.2);
            patients[2] = new Patient("050972-1233", "Hans Jørgensen", 89.4);
            patients[3] = new Patient("011064-1522", "Ulla Nielsen", 59.9);
            patients[4] = new Patient("123456-1234", "Ib Hansen", 87.7);

            db.Patienter.Add(patients[0]);
            db.Patienter.Add(patients[1]);
            db.Patienter.Add(patients[2]);
            db.Patienter.Add(patients[3]);
            db.Patienter.Add(patients[4]);
            db.SaveChanges();
        }

        Laegemiddel[] laegemiddler = new Laegemiddel[5];
        laegemiddler[0] = db.Laegemiddler.FirstOrDefault()!;
        if (laegemiddler[0] == null)
        {
            laegemiddler[0] = new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk");
            laegemiddler[1] = new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml");
            laegemiddler[2] = new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk");
            laegemiddler[3] = new Laegemiddel("Methotrexat", 0.01, 0.015, 0.02, "Styk");
            laegemiddler[4] = new Laegemiddel("Prednisolon", 0.1, 0.15, 0.2, "Styk");

            db.Laegemiddler.Add(laegemiddler[0]);
            db.Laegemiddler.Add(laegemiddler[1]);
            db.Laegemiddler.Add(laegemiddler[2]);
            db.Laegemiddler.Add(laegemiddler[3]);
            db.Laegemiddler.Add(laegemiddler[4]);

            db.SaveChanges();
        }

        Ordination[] ordinationer = new Ordination[6];
        ordinationer[0] = db.Ordinationer.FirstOrDefault()!;
        if (ordinationer[0] == null) {
            Laegemiddel[] lm = db.Laegemiddler.ToArray();
            Patient[] p = db.Patienter.ToArray();

            ordinationer[0] = new PN(new DateTime(2021, 1, 1), new DateTime(2021, 1, 12), 123, lm[1]);    
            ordinationer[1] = new PN(new DateTime(2021, 2, 12), new DateTime(2021, 2, 14), 3, lm[0]);    
            ordinationer[2] = new PN(new DateTime(2021, 1, 20), new DateTime(2021, 1, 25), 5, lm[2]);    
            ordinationer[3] = new PN(new DateTime(2021, 1, 1), new DateTime(2021, 1, 12), 123, lm[1]);
            ordinationer[4] = new DagligFast(new DateTime(2021, 1, 10), new DateTime(2021, 1, 12), lm[1], 2, 0, 1, 0);
            ordinationer[5] = new DagligSkæv(new DateTime(2021, 1, 23), new DateTime(2021, 1, 24), lm[2]);
            
            ((DagligSkæv) ordinationer[5]).doser = new Dosis[] { 
                new Dosis(CreateTimeOnly(12, 0, 0), 0.5),
                new Dosis(CreateTimeOnly(12, 40, 0), 1),
                new Dosis(CreateTimeOnly(16, 0, 0), 2.5),
                new Dosis(CreateTimeOnly(18, 45, 0), 3)        
            }.ToList();
            

            db.Ordinationer.Add(ordinationer[0]);
            db.Ordinationer.Add(ordinationer[1]);
            db.Ordinationer.Add(ordinationer[2]);
            db.Ordinationer.Add(ordinationer[3]);
            db.Ordinationer.Add(ordinationer[4]);
            db.Ordinationer.Add(ordinationer[5]);

            db.SaveChanges();

            p[0].ordinationer.Add(ordinationer[0]);
            p[0].ordinationer.Add(ordinationer[1]);
            p[2].ordinationer.Add(ordinationer[2]);
            p[3].ordinationer.Add(ordinationer[3]);
            p[1].ordinationer.Add(ordinationer[4]);
            p[1].ordinationer.Add(ordinationer[5]);

            db.SaveChanges();
        }
    }

    
    public List<PN> GetPNs() {
        return db.PNs.Include(o => o.laegemiddel).Include(o => o.dates).ToList();
    }

    public List<DagligFast> GetDagligFaste() {
        return db.DagligFaste
            .Include(o => o.laegemiddel)
            .Include(o => o.MorgenDosis)
            .Include(o => o.MiddagDosis)
            .Include(o => o.AftenDosis)            
            .Include(o => o.NatDosis)            
            .ToList();
    }

    public List<DagligSkæv> GetDagligSkæve() {
        return db.DagligSkæve
            .Include(o => o.laegemiddel)
            .Include(o => o.doser)
            .ToList();
    }

    public List<Patient> GetPatienter() {
        return db.Patienter.Include(p => p.ordinationer).ToList();
    }

    public List<Laegemiddel> GetLaegemidler() {
        return db.Laegemiddler.ToList();
    }

    /*public PN OpretPN(int patientId, int laegemiddelId, double antal, DateTime startDato, DateTime slutDato) {
        // TODO: Implement!
        return null!;
    }*/

    public PN OpretPN(int patientId, int laegemiddelId, double antal, DateTime startDato, DateTime slutDato)
    {
        //Step 1:
        // Find the patient by PatientId
        var patient = db.Patienter.FirstOrDefault(p => p.PatientId == patientId);
        if (patient == null) throw new ArgumentException("Invalid patientId");

        // Find the laegemiddel by LaegemiddelId
        var laegemiddel = db.Laegemiddler.FirstOrDefault(l => l.LaegemiddelId == laegemiddelId);
        if (laegemiddel == null) throw new ArgumentException("Invalid laegemiddelId");

        // Step 2: Create the PN object using the constructor
        PN pn = new PN(startDato, slutDato, antal, laegemiddel);

        // Step 3: Add the PN object to the patient's list of ordinations
        patient.ordinationer.Add(pn);

        //Step 4:
        // Add the new DagligFast to the database context and save
        db.Ordinationer.Add(pn);
        db.SaveChanges();

        // Step 5: Return the newly created PN object
        return pn;
    }


    public DagligFast OpretDagligFast(int patientId, int laegemiddelId,
    double antalMorgen, double antalMiddag, double antalAften, double antalNat,
    DateTime startDato, DateTime slutDato)
    {
        // Find the patient by PatientId
        var patient = db.Patienter.FirstOrDefault(p => p.PatientId == patientId);
        if (patient == null) throw new ArgumentException("Invalid patientId");

        // Find the laegemiddel by LaegemiddelId
        var laegemiddel = db.Laegemiddler.FirstOrDefault(l => l.LaegemiddelId == laegemiddelId);
        if (laegemiddel == null) throw new ArgumentException("Invalid laegemiddelId");

        // Create a new DagligFast instance
        var dagligFast = new DagligFast(startDato, slutDato, laegemiddel, antalMorgen, antalMiddag, antalAften, antalNat);

        // Add the new ordination to the patient
        patient.ordinationer.Add(dagligFast);

        // Add the new DagligFast to the database context and save
        db.Ordinationer.Add(dagligFast);
        db.SaveChanges();

        return dagligFast;
    }

    public DagligSkæv OpretDagligSkaev(int patientId, int laegemiddelId, Dosis[] doser, DateTime startDato, DateTime slutDato)
    {
        // Find patienten
        var patient = db.Patienter.Include(p => p.ordinationer).FirstOrDefault(p => p.PatientId == patientId);
        if (patient == null)
            throw new ArgumentException($"Patient med ID {patientId} blev ikke fundet.");

        // Find lægemidlet
        var laegemiddel = db.Laegemiddler.FirstOrDefault(l => l.LaegemiddelId == laegemiddelId);
        if (laegemiddel == null)
            throw new ArgumentException($"Lægemiddel med ID {laegemiddelId} blev ikke fundet.");

        // Opret ny DagligSkæv-ordination
        var nyOrdination = new DagligSkæv(startDato, slutDato, laegemiddel, doser);

        // Tilføj ordinationen til patienten
        patient.ordinationer.Add(nyOrdination);

        // Gem ændringer i databasen
        db.SaveChanges();

        // Returner den oprettede ordination
        return nyOrdination;
    }

    /*
    public string AnvendOrdination(int id, Dato dato) {
        // TODO: Implement!
        return null!;
    }
    */
    public string AnvendOrdination(int id, Dato dato)
    {
        // Find ordinationen baseret på dens ID
        var ordination = db.Ordinationer.OfType<PN>().FirstOrDefault(o => o.OrdinationId == id);
        if (ordination == null)
        {
            throw new ArgumentException($"Ordination med ID {id} blev ikke fundet.");
        }

        // Forsøg at give dosis på den angivne dato
        bool dosisGivet = ((PN)ordination).givDosis(dato);
        if (dosisGivet)
        {
            db.SaveChanges();
            return "Ordinationen blev anvendt.";
        }
        else
        {
            return "Ordinationen kan ikke anvendes på denne dato.";
        }
    }



    /// <summary>
    /// Den anbefalede dosis for den pågældende patient, per døgn, hvor der skal tages hensyn til
    /// patientens vægt. Enheden afhænger af lægemidlet. Patient og lægemiddel må ikke være null.
    /// </summary>
    /// <param name="patient"></param>
    /// <param name="laegemiddel"></param>
    /// <returns></returns>

    /*
    public double GetAnbefaletDosisPerDøgn(int patientId, int laegemiddelId) {
        // TODO: Implement!
        return -1;
	}
    */
    public double GetAnbefaletDosisPerDøgn(int patientId, int laegemiddelId)
    {
        var patient = db.Patienter.FirstOrDefault(p => p.PatientId == patientId);
        if (patient == null)
        {
            throw new ArgumentException($"Patient med ID {patientId} blev ikke fundet.");
        }

        var laegemiddel = db.Laegemiddler.FirstOrDefault(l => l.LaegemiddelId == laegemiddelId);
        if (laegemiddel == null)
        {
            throw new ArgumentException($"Lægemiddel med ID {laegemiddelId} blev ikke fundet.");
        }

        double vægt = patient.vaegt;

        // Vælg dosisfaktoren afhængigt af patientens vægt
        double dosisFaktor;

        if (vægt < 25)
        {
            dosisFaktor = laegemiddel.enhedPrKgPrDoegnLet;
        }
        else if (vægt >= 25 && vægt <= 120)
        {
            dosisFaktor = laegemiddel.enhedPrKgPrDoegnNormal;
        }
        else
        {
            dosisFaktor = laegemiddel.enhedPrKgPrDoegnTung;
        }

        // Beregner den anbefalede dosis per døgn
        double anbefaletDosis = vægt * dosisFaktor;

        return anbefaletDosis;
    }


}