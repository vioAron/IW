using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW.Model
{
    public class InstrumentMarketData
    {
        private static readonly Random _random = new Random();

        public string InstrumentId { get; set; }
        public string Description { get; set; }

        public static InstrumentMarketData New
        {
            get
            {
                var id = _random.Next(1000);
                return new InstrumentMarketData
            {

                InstrumentId = string.Format("Id - {0}", id),
                Description = string.Format("Desc - {0}", id)

            };
            }
        }
    }
}
