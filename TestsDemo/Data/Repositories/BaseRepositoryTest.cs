using System;
using System.Collections.Generic;
using System.Text;
using Core.Data.Base;
using Core.Domains.Base;
using Data.Exceptions;
using NUnit.Framework;

namespace TestsDemo.Data.Repositories
{
    public abstract class BaseRepositoryTest<TRepo, TPoco, TDomain, TDomainList, TPk, TFk>
        where TRepo : IRepository<TPoco, TDomain, TDomainList, TPk, TFk>
        where TPoco : class, IEntity
        where TDomain : IDomainObject
        where TDomainList : IDomainObject
        where TFk : struct
    {
        /// <summary>
        /// Repository ciblé par les tests de la classe.
        /// </summary>
        protected abstract TRepo Repository { get; set; }
        /// <summary>
        /// Stub pour le Poco de l'entité ciblée par les tests.
        /// </summary>
        protected abstract TPoco PocoStub { get; set; }
        /// <summary>
        /// Stub pour le Dto de l'entité ciblée par les tests.
        /// </summary>
        protected abstract TDomain DtoStub { get; set; }

        /// <summary>
        /// Méthode d'initialisation de chaque test de la classe. Instancie les différents composants nécessaires à la bonne tenue des tests rédigés.
        /// </summary>
        public abstract void SetUp();

        /// <summary>
        /// Test vérifiant le mapping d'un poco vers un dto.
        /// </summary>
        public abstract void VerifyMap();

        /// <summary>
        /// Test vérifiant la levée d'exception en cas de paramètre null passé à la méthode Map().
        /// </summary>
        [Test(Description = "Test d'intégrité de la méthode Map. Doit lever une exception si le paramètre en entrée est null.")]
        public void VerifyMapNullArgument()
        {
            Assert.Throws<RepositoryMappingException>(delegate { this.Repository.Map(null); });
        }

        /// <summary>
        /// Test vérifiant le mapping d'un poco vers un DtoBase
        /// </summary>
        public abstract void VerifyMapList();

        /// <summary>
        /// Test vérifiant la levée d'exception en cas de paramètre null passé à la méthode MapList().
        /// </summary>
        [Test(Description = "Test d'intégrité de la méthode MapList. Doit lever une exception si le paramètre en entrée est null.")]
        public virtual void VerifyMapListNullArgument()
        {
            Assert.Throws<RepositoryMappingException>(delegate { this.Repository.MapList(null); });
        }

        /// <summary>
        /// Test vérifiant le mapping d'un dto vers un poco.
        /// </summary>
        public abstract void VerifyConvert();
    }
}
