using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class MultipleFileUpload
    {
        [Required]
        public List<IFormFile> Files { get; set; }
        public string FileName { get; set; }
    }
}