using System.ComponentModel.DataAnnotations;

namespace Student.UI.DTOModel
{
    public class StudentDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Roll { get; set; }
    }
}
