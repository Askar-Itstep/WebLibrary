using System.ComponentModel.DataAnnotations;

namespace WebLibrary
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] ImageData { get; set; }
    }
}