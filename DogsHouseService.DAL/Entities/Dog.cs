using System.ComponentModel.DataAnnotations;

namespace DogsHouseService.DAL.Entities
{
    public class Dog
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Tail_Length { get; set; }
        public int Weight { get; set; }
    }
}
