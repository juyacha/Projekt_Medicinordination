namespace shared.Model;

public class PN : Ordination {
	public double antalEnheder { get; set; }
    public List<Dato> dates { get; set; } = new List<Dato>();

    public PN (DateTime startDen, DateTime slutDen, double antalEnheder, Laegemiddel laegemiddel) : base(laegemiddel, startDen, slutDen) {
		this.antalEnheder = antalEnheder;
	}

    public PN() : base(null!, new DateTime(), new DateTime()) {
    }

    /// <summary>
    /// Registrerer at der er givet en dosis på dagen givesDen
    /// Returnerer true hvis givesDen er inden for ordinationens gyldighedsperiode og datoen huskes
    /// Returner false ellers og datoen givesDen ignoreres
    /// </summary>
    /*public bool givDosis(Dato givesDen) {
        // TODO: Implement!
        return false;
    }*/

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


    /*public override double doegnDosis() {
    	// TODO: Implement!
        return -1;
    }*/
    public override double doegnDosis()
    {
        // Since PN doesn't have a fixed daily dose, we return a placeholder value
        // You could also implement logic to calculate a daily average dose if needed
        return -1;
    }



    /*public override double samletDosis() {
        return dates.Count() * antalEnheder;
    }*/
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
