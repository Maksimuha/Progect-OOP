namespace Core.Shared;

public record DOnotEmptyString
{
    public string Value { get; }
    private DOnotEmptyString(string notEmptyString)
    {
        Value = notEmptyString;
    }
    public static DOnotEmptyString Create(string notEmptyString, int size)
    {
        notEmptyString = notEmptyString.Trim();
        if (string.IsNullOrWhiteSpace(notEmptyString))
        {
            throw new ArgumentException("Строка не может быть пустой!");
        }

        if (notEmptyString.Length > size)
        {
            throw new ArgumentException("Строка не может быть больше " + size + " символов!");
        }
        return new DOnotEmptyString(notEmptyString);
    }
}
