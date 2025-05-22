using System.Text.Json.Serialization;

public class UserInput
{
    [JsonPropertyName("lat")]
    public double lat { get; set; }

    [JsonPropertyName("lng")]
    public double lng { get; set; }

    [JsonPropertyName("radius")]
    public int radius { get; set; }
}
