using System.Threading.Tasks;
using Core.Services.Base;
using Data.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Helpers
{
    public abstract class BaseCrudApiController<T, TDomain,TDomainList, TPk,  TFk> : 
        BaseListApiController<T,TDomainList,TFk>
        where T : ICRUDBaseService<TDomain,TDomainList, TPk,  TFk> 
        where TFk : struct
    {
        protected BaseCrudApiController(T service, ILogger<BaseApiController<T>> logger) : base(service, logger)
        {
        }

        /// <summary>
        /// Méthode HTTP GET utilisée pour récupérer un unique enregistrement de l'entité ciblée.
        /// </summary>
        /// <param name="id">Clé primaire de l'entité recherchée.</param>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        public virtual async Task<IActionResult> Get(TPk id)
        {
            try
            {
                var result = await this.Service.Load(id);
                return Ok(result);
            }
            catch (RepositoryMappingException ex)
            {
                Logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
        }

        //[HttpPost]
        //public virtual IActionResult Post([FromBody]TDomain value)
        //{
        //    //this.Service.Create(value)
        //    throw new NotImplementedException();
        //}

        //[HttpPut("{id}")]
        //public virtual void Put(int id, [FromBody]TDomain value)
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpDelete("{id}")]
        //public virtual void Delete(TPk id)
        //{
        //    throw new NotImplementedException();
        //}

    }
}