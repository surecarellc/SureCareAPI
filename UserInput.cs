using System.Text.Json.Serialization;

public class UserInput
{
    [JsonPropertyName("lat")]
    public float lat { get; set; }

    [JsonPropertyName("lng")]
    public float lng { get; set; }

    [JsonPropertyName("radius")]
    public int radius { get; set; }
}
