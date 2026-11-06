using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace M223PunchclockDotnet.Model
{
    public class Tag
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public ICollection<Entry> Entries { get; set; } = new List<Entry>();
    }
}