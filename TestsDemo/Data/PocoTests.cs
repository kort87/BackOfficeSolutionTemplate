using System;
using System.Collections.Generic;
using System.Text;
using Data.Entities;
using NUnit.Framework;

namespace TestsDemo.Data
{
    [TestFixture(Author = "Romain Bénard", Category = "Poco test")]
    public class PocoTests
    {
        /// <summary>
        /// Test d'intégrité des POCO du projet.
        /// Objets devant définir des propriétés simples, en lecture et écriture.
        /// </summary>
        /// <param name="T"></param>
        [Test]
        public void CheckPocoIntegrity([Values(
            typeof(Entity))] Type T)
        {
            foreach (var methodInfo in T.GetProperties())
            {
                Assert.IsTrue(methodInfo.CanRead);
                Assert.IsTrue(methodInfo.CanWrite);
            }

        }
    }
}
