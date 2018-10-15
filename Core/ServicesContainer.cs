using System;

namespace Core
{
    public static class ServicesContainer
    {
        /// <summary>
        /// Recupere le service provider qui permet l'injection de dépendance (pour le faire manuellement)
        /// </summary>
        public static IServiceProvider Container { get; set; }
    }
}
