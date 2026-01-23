using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInjection.SQL.DTOs
{
    public class MapDataDto
    {
        public ICollection<MapMarkerDto>? Markers { get; set; }
        public double? CenterLatitude { get; set; }
        public double? CenterLongitude { get; set; }
    }
}
