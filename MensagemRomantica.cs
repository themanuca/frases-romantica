namespace frases_romantica
{
    internal static class Lista
    {
        public static List<MensagemRomantica> FraseRomantica = new();
    }
    public class MensagemRomantica
    {
        public Guid ID { get; set; }
        public string Menssage { get; set; }
        public string Status { get; set; }
        public DateTime? ProcessedTime { get; set; }
    }
}
