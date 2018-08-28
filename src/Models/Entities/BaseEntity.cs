using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }    
    }
}