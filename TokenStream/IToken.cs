namespace DenysRedkoParser
{
    public interface IToken
    {
        string Content { get; set; }
        string Type { get; set; }
        string Value { get; set; }
    }
}