using System;
using System.Collections.Generic;
using System.Text;
using Domain.Base;
using NUnit.Framework;

namespace TestsDemo.Domain
{
    /// <summary>
    /// Classe de tests vérifiant l'intégrité d'un Dto, à savoir qu'il n'est constitué 
    /// que de propriétés définies en lecture et en écriture.
    /// </summary>
    [TestFixture(Author = "Romain Bénard", Category = "Dto test")]
    public class DtoTests
    {
        [Test]
        public void CheckDtoIntegrity([Values(
            typeof(DomainObject)
        )] Type T)
        {
            foreach (var methodInfo in T.GetProperties())
            {
                Assert.IsTrue(methodInfo.CanRead);
                Assert.IsTrue(methodInfo.CanWrite);
            }
        }
    }
}
