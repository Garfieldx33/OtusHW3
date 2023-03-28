using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Customer
    {
        public long id { get; init; }
        
        [Required]
        public string firstname { get; init; }

        [Required]
        public string lastname { get; init; }
    }
}