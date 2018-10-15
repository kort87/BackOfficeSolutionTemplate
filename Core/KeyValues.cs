namespace Core
{
    public interface IKeyValues
    {
        string Key { get; set; }
        string Value { get; set; }
    }

    public class KeyValues : IKeyValues
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
