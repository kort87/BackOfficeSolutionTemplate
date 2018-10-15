using NUnit.Framework;

namespace TestsDemo
{
    [TestFixture(Author = "Romain Bénard")]
    public class SampleTest
    {
        [Test(Description = "Test de démonstration")]
        public void AssertTestsCanBeLaunched()
        {
            Assert.IsTrue(true);
        }
    }
}
