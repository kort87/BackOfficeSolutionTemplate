namespace Core.Domains.Base
{
    /// <summary>
    /// Interface décrivant les factories de domain object.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public interface IDomainObjectFactory<out TDomainObject> where TDomainObject : IDomainObject
    {
        TDomainObject CreateInstance();
    }
}
