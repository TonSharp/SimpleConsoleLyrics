using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLyrics
{
    public class Song
    {
        public string Compositor { get; set; }
        public string Name { get; set; }

        public Song(string Compositor, string SongName)
        {
            this.Compositor = Compositor;
            this.Name = SongName;
        }
    }
}
