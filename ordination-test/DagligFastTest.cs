using shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordination_test
{
    [TestClass]
    public class DagligFastTest
    {
        [TestMethod]
        public void SamletDosis_CorrectCalculation()
        {
            // Arrange: Opretter en DagligFast-ordination
            var laegemiddel = new Laegemiddel("Testmedicin", 1, 1.5, 2, "Styk");
            var dagligFast = new DagligFast(
                DateTime.Today, 
                DateTime.Today.AddDays(2), 
                laegemiddel, 
                1, 1, 1, 1 
            );

            // Act: Beregner samlet dosis
            double samletDosis = dagligFast.samletDosis();

            // Assert: Bekræfter, at den samlede dosis er korrekt
            Assert.AreEqual(12, samletDosis); // 4 doser * 3 dage
        }

        [TestMethod]
        public void SamletDosis_NoDoses_ReturnsZero()
        {
            // Arrange: Opretter en DagligFast-ordination med 0 doser
            var laegemiddel = new Laegemiddel("Testmedicin", 1, 1.5, 2, "Styk");
            var dagligFast = new DagligFast(
                DateTime.Today, // Startdato i dag
                DateTime.Today.AddDays(2), // Slutdato om 2 dage
                laegemiddel, // Lægemiddel
                0, 0, 0, 0 // Ingen doser
            );

            // Act: Beregner samlet dosis
            double samletDosis = dagligFast.samletDosis();

            // Assert: Bekræfter, at den samlede dosis er 0
            Assert.AreEqual(0, samletDosis); // 0 doser * 3 dage
        }

    }

}
