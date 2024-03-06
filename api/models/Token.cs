using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;

namespace api.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public BigInteger TotalSupply { get; set; }
        public BigInteger CirculatingSupply { get; set; }
    }
}
