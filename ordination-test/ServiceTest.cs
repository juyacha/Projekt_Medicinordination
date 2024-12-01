namespace ordination_test;

using Data;
using Microsoft.EntityFrameworkCore;
using Service;
using shared.Model;

[TestClass]
public class ServiceTest
{
    private DataService service;

    [TestInitialize]
    public void SetupBeforeEachTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
        var context = new OrdinationContext(optionsBuilder.Options);
        service = new DataService(context);
        service.SeedData();
    }

    [TestMethod]
    public void PatientsExist()
    {
        Assert.IsNotNull(service.GetPatienter());
    }

    [TestMethod]
    public void OpretDagligFast()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        Assert.AreEqual(1, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestAtKodenSmiderEnException()
    {
        // Herunder skal man så kalde noget kode,
        // der smider en exception.

        // Hvis koden _ikke_ smider en exception,
        // så fejler testen.

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }

    [TestMethod]
    public void GetAnbefaletDosisPerDøgn_CorrectCalculation() //Unit test af DataService; happy path
    {
        // Arrange
        var patient = service.GetPatienter().First(); 
        var laegemiddel = service.GetLaegemidler().First();

        // Act
        double dosis = service.GetAnbefaletDosisPerDøgn(patient.PatientId, laegemiddel.LaegemiddelId);

        //Assert: Bekræfter, at der returneres en valid dosis
        Assert.IsTrue(dosis > 0); 
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetAnbefaletDosisPerDøgn_InvalidPatientId() //Unit test af DataService; unhappy path
    {
        // Arrange
        int invalidPatientId = -1; // Ugyldigt ID
        var laegemiddel = service.GetLaegemidler().First();

        // Act
        service.GetAnbefaletDosisPerDøgn(invalidPatientId, laegemiddel.LaegemiddelId);
    }

}