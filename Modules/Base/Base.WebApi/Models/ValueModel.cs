using System.ComponentModel.DataAnnotations;

namespace Base.WebApi.Models
{
    public class ValueModel<T>
    {
        [Required]
        public T Value { get; set; }
    }
}