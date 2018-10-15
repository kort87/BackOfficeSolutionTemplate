using System;
using Microsoft.Extensions.Logging;

namespace Core.Services.Base
{
    /// <summary>
    /// Interface décrivant tous les services
    /// </summary>
    public interface IBaseService : IDisposable
    {
        ILogger LogService { get; set; }
    }
}
