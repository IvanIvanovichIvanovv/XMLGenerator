using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLGenerator.Classes
{
    public class VirtualMachine
    {
        private string name;
        private int memoryRAM;
        private int coresCPU;
        private string macAddress;
        private PCI pci;
        private int portVNC;

        public VirtualMachine(string name, int memoryRAM, int coresCPU, string macAddress, PCI pci, int portVNC)
        {
            this.name = name;
            this.memoryRAM = memoryRAM;
            this.coresCPU = coresCPU;
            this.macAddress = macAddress;
            this.pci = pci;
            this.portVNC = portVNC;
        }

    }
}
