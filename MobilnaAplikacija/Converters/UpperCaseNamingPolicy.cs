using System.Text.Json;

public class UpperCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        // Convert the property name to uppercase
        return name.ToUpperInvariant();
    }
}