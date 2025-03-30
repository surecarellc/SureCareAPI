using System.Text.Json.Serialization;

public class UserInput
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }
}
