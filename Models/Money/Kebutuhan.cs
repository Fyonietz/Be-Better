public class Kebutuhan
{
    public int id { get; set; }             // For existing records, optional for new ones
    public string nama { get; set; } = "";  // non-nullable string
    public double nominal { get; set; }     // double for numeric values
    public string? notes { get; set; }      // nullable string
}

