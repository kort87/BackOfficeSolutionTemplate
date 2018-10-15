using Core.Data.Base;
using Core.Services.Base;
using Microsoft.Extensions.Logging;

namespace Services.Helpers
{
    public class BaseService : IBaseService
    {
        public ILogger LogService { get; set; }

        protected IUnitOfWork UnitOfWork { get; }

        public BaseService(ILogger<BaseService> ilogger, IUnitOfWork unitOfWork)
        {
            this.LogService = ilogger;
            this.UnitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            this.UnitOfWork?.Dispose();
        }
    }
}
