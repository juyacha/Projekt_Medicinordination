using shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordination_test
{
    [TestClass]
    public class PNTest
    {
        [TestMethod]
        public void GivDosis_ValidDate()
        {
            // Arrange: Opretter en PN-ordination med en gyldig periode
            var laegemiddel = new Laegemiddel("Testmedicin", 1, 1.5, 2, "Styk");
            var pn = new PN(DateTime.Today, DateTime.Today.AddDays(2), 5, laegemiddel);

            // Act: Opretter et Dato-objekt og giver dosis på en gyldig dato
            Dato dosisDato = new Dato { dato = DateTime.Today }; 
            bool result = pn.givDosis(dosisDato); 

            // Assert: Bekræfter, at dosis blev givet og resultatet er true
            Assert.IsTrue(result);
            Assert.AreEqual(1, pn.dates.Count);
        }

        [TestMethod]
        public void GivDosis_InvalidDate_ReturnsFalse()
        {
            // Arrange: Opretter en PN-ordination med en gyldig periode
            var laegemiddel = new Laegemiddel("Testmedicin", 1, 1.5, 2, "Styk");
            var pn = new PN(DateTime.Today, DateTime.Today.AddDays(2), 5, laegemiddel);

            // Act: Opretter et Dato-objekt og forsøger at give dosis på en ugyldig dato
            Dato dosisDato = new Dato { dato = DateTime.Today.AddDays(3) }; // Udenfor perioden = ugyldig
            bool result = pn.givDosis(dosisDato);

            // Assert: Bekræfter, at dosis ikke blev givet og resultatet er false
            Assert.IsFalse(result);
            Assert.AreEqual(0, pn.dates.Count);
        }
    }
}
