using System.ComponentModel.DataAnnotations;

namespace shared.Model;

public class PN : Ordination {
    public double antalEnheder { get; set; }
    public List<Dato> dates { get; set; } = new List<Dato>();

    public PN(DateTime startDen, DateTime slutDen, double antalEnheder, Laegemiddel laegemiddel) : base(laegemiddel, startDen, slutDen) {
        this.antalEnheder = antalEnheder;
    }

    public PN() : base(null!, new DateTime(), new DateTime()) {
    }

    /// <summary>
    /// Registrerer at der er givet en dosis på dagen givesDen
    /// Returnerer true hvis givesDen er inden for ordinationens gyldighedsperiode og datoen huskes
    /// Returner false ellers og datoen givesDen ignoreres
    /// </summary>
   

    public bool givDosis(Dato givesDen)
    {
        // Check if the givesDen is within the valid date range of the ordination
        if (givesDen.dato >= startDen && givesDen.dato <= slutDen)
        {
            // Add the given date to the list
            dates.Add(givesDen);
            return true;
        }
        return false;
    }
  


    public override double doegnDosis()
    {
        // Tjek om der er nogen registrerede datoer
        if (dates.Count == 0) return 0; // Undgå division med 0

        // Find minimum og maksimum datoer i listen
        DateTime min = dates.OrderBy(d => d.dato).First().dato.Date;
        DateTime max = dates.OrderBy(d => d.dato).Last().dato.Date;


        // Beregn antallet af dage (inklusive begge ender af perioden)
        int dage = (max - min).Days + 1;

        // Beregn den gennemsnitlige dosis per dag
        double sum = samletDosis() / dage;

        return sum;
    }

    public override double samletDosis()
    {
        // Total dose is the number of times a dose was given (dates.Count) multiplied by the number of units per dose
        return dates.Count * antalEnheder;
    }


    public int getAntalGangeGivet() {
        return dates.Count();
    }

	public override String getType() {
		return "PN";
	}
}
