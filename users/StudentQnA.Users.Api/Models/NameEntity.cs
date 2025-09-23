using System.ComponentModel.DataAnnotations.Schema;

namespace StudentQnA.Users.Api.Models
{
    [Table("names")]
    public class NameEntity
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("value")]
        public string Value { get; set; } = string.Empty;
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}
