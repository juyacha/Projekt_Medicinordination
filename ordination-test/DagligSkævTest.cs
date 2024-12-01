using shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordination_test
{
    [TestClass]
    public class DagligSkævTest
    {
        [TestMethod]
        public void SamletDosis_CorrectCalculation()
        {
            // Arrange: Opretter en DagligSkæv-ordination med flere doser
            var laegemiddel = new Laegemiddel("Testmedicin", 1, 1.5, 2, "Ml");
            var doser = new[]
            {
            new Dosis(new DateTime(2024, 11, 29, 12, 0, 0), 2), // 2 enheder kl. 12.00
            new Dosis(new DateTime(2024, 11, 29, 16, 0, 0), 3)  // 3 enheder kl. 16.00
        };
            var dagligSkaev = new DagligSkæv(DateTime.Today, DateTime.Today.AddDays(2), laegemiddel, doser);

            // Act: Beregner samlet dosis
            double samletDosis = dagligSkaev.samletDosis();

            // Assert: Bekræfter, at den samlede dosis er korrekt
            Assert.AreEqual(15, samletDosis); // (2+3) * 3 dage = 15
        }

        [TestMethod]
        public void SamletDosis_NoDoses_ReturnsZero()
        {
            // Arrange: Opretter en DagligSkæv-ordination uden doser
            var laegemiddel = new Laegemiddel("Testmedicin", 1, 1.5, 2, "Ml");
            var dagligSkaev = new DagligSkæv(DateTime.Today, DateTime.Today.AddDays(2), laegemiddel, new Dosis[] { });

            // Act: Beregner samlet dosis
            double samletDosis = dagligSkaev.samletDosis();

            // Assert: Bekræfter, at den samlede dosis er 0
            Assert.AreEqual(0, samletDosis); // Ingen doser * 3 dage = 0
        }
    }

}
