using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApiServer.Models
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="Varchar(100)")]
        public string? Title { get; set; }
        public DateTime? TodoDate { get; set; }
        public bool? IsComplete { get; set; }
    }
}
