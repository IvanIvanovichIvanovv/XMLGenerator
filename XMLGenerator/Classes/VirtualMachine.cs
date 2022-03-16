namespace XMLGenerator.Classes
{
    public class VirtualMachine
    {
        public string name;
        public int memoryRAM;
        public int coresCPU;
        public string macAddress;
        public PCI pci;
        public int portVNC;

        public VirtualMachine(string name, int memoryRAM, int coresCPU, string macAddress, PCI pci, int portVNC)
        {
            this.name = name;
            this.memoryRAM = memoryRAM;
            this.coresCPU = coresCPU;
            this.macAddress = macAddress;
            this.pci = pci;
            this.portVNC = portVNC;
        }
        public VirtualMachine(string name, int memoryRAM, int coresCPU, string macAddress, string BUS, string SLOT, int Function, int portVNC)
        {
            this.name = name;
            this.memoryRAM = memoryRAM;
            this.coresCPU = coresCPU;
            this.macAddress = macAddress;
            this.pci = new PCI(BUS, SLOT, Function);
            this.portVNC = portVNC;
        }
        public override string ToString()
        {
            return $"{name},{memoryRAM.ToString()},{coresCPU.ToString()},{macAddress},{pci.ToString()},{portVNC.ToString()}";
        }
    }
}
