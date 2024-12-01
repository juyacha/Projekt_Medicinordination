using shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordination_test
{
    [TestClass]
    public class OrdinationTest
    {
        [TestMethod]
        public void AntalDage_CorrectDays() //Unit test af Ordination; happy path
        {
            // Arrange: Startdato og slutdato er to dage
            var ordination = new MockOrdination
            {
                startDen = DateTime.Today,
                slutDen = DateTime.Today.AddDays(2)
            };

            // Act: Beregner antal dage
            int antalDage = ordination.antalDage();

            // Assert: Bekræfter at antalDage er 3 (inklusive start og slutdato)
            Assert.AreEqual(3, antalDage);
        }

        [TestMethod]
        public void AntalDage_InvalidDates_ReturnsZero() //Unit test af Ordination; unhappy path
        {
            // Arrange: Startdato er senere end slutdato
            var ordination = new MockOrdination
            {
                startDen = DateTime.Today.AddDays(1),
                slutDen = DateTime.Today
            };

            // Act: Beregner antal dage
            int antalDage = ordination.antalDage();

            // Assert: Antal dage skal være 0, fordi start > slut
            Assert.AreEqual(0, antalDage);
        }

        // Mock ordination til test af antalDage
        private class MockOrdination : Ordination
        {
            public override double samletDosis() => 0;
            public override double doegnDosis() => 0;
            public override string getType() => "Mock";
        }
    }

}
