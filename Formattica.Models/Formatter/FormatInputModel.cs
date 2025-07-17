using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formattica.Models.Formatter
{
    public class FormatInputModel
    {
        public string? Content { get; set; }
        public string? FormatType { get; set; } // JSON, XML, SQL
    }
}
