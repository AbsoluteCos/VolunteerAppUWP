using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolunteerAppUWP
{
    public class VOpp
    {
        public List<Data> data { get; set; }
    }

    public class Data
    {
        public string title { get; set; }
        public string summary { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }
}
