using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLGenerator.Classes
{
    public class PCI
    {
        private string bus;
        private string slot;
        private int function;

        public PCI(string bus, string slot, int funtion) 
        {
            this.bus = bus;
            this.slot = slot;   
            this.function = funtion;
        }
        public override string ToString()
        {
            return $"bus='0x{bus}' slot='0x{slot}' function='0x{function}'";
        }
    }
}
