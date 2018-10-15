using System;
using System.Linq;
using Core.Services.Base;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Api.Helpers
{
    // TODO Mettre la sécurité par token ici
    //[Route("api/v1/[controller]")]

    /// <summary>
    /// Classe de base de ControllerApi. Gère l'injection de service et du logger, ainsi que la sécurisation des Apis en héritant.
    /// L'utilisation publique se fait en ajoutant l'attribut anonymous
    /// </summary>
    /// <typeparam name="T">Service à injecter devant répondre au contrat IBaseService</typeparam>
    [Route("api/v1/common/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public abstract class BaseApiController<T> : Controller
        where T : IBaseService
    {
        protected ILogger Logger;

        protected T Service;
        protected IConfiguration _settings;

        protected BaseApiController(T service, ILogger<BaseApiController<T>> logger)
        {
            this.Service = service;
            this.Logger = logger;
            this.Service.LogService = this.Logger;
        }

        protected override void Dispose(bool disposing)
        {
            this.Service?.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Méthode de test pour vérification de l'existence du controller et du log.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Test")]
        public virtual string Test()
        {
            this.Logger.LogInformation("It works!");
            return nameof(BaseApiController<T>);
        }

        //protected virtual bool IsAuthorize(ref string rootId, string authorize= "AuthorizeForAdministrator")
        //{
        //    bool result = false;
        //    if (_settings != null)
        //    {
        //        string authorizeRoles = _settings.GetSection("AppSettings").GetValue<string>(authorize);

        //        var userInfo = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        //        if (userInfo!=null && userInfo.Count() > 0)
        //        {
        //            rootId = userInfo.FirstOrDefault(user => user.Type == "sub").Value;
        //            var roles = userInfo.Where(user => user.Type == "role").ToList();
        //            foreach (string item in authorizeRoles.Split(';').Where(item => !String.IsNullOrEmpty(item)).ToList())
        //            {
        //                if (result = roles.Exists(user => user.Value == item))
        //                    break;
        //            }
        //        }
        //    }
        // return (result);
        //}
    }
}
