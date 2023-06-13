using System.Text.Json.Serialization;

namespace Backend.Models
{
    public record Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Height { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly Birthdate { get; set; }
    }
}
