using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using XMLGenerator.Classes;

namespace XMLGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> defaultXML =new List<string>();
        public List<VirtualMachine> Presets=new List<VirtualMachine>();
        public MainWindow()
        {
            InitializeComponent();
            ReadPresets(Presets);
            if (Presets != null)
            {
                for (int i = 0; i < Presets.Count; i++)
                {
                    cb_VMs.Items.Add(Presets[i].name);
                }
            }
            ReadDefaultXML(defaultXML);
        }

        private void btn_GenerateXML_Click(object sender, RoutedEventArgs e)
        {
            VirtualMachine VM = new VirtualMachine(tb_Name.Text, Convert.ToInt32(tb_RAM.Text), Convert.ToInt32(tb_CPU.Text), tb_MAC.Text, tb_BUS.Text, tb_Slot.Text, Convert.ToInt32(tb_Function.Text), Convert.ToInt32(tb_VNC.Text));
            defaultXML[8] = $"  <name>{VM.name}</name>";
            defaultXML[9] = $"  <memory unit='GiB'>{VM.memoryRAM}</memory>";
            defaultXML[10]= $"  <currentMemory unit='GiB'>{VM.memoryRAM}</currentMemory>";
            defaultXML[11] = $"  <vcpu placement='static'>{VM.coresCPU}</vcpu>";
            defaultXML[13] = $"    <type arch='x86_64' machine='pc'>hvm</type>";
            defaultXML[37] = $"      <mac address='{VM.macAddress}'/> ";
            defaultXML[44] = $"        <address type='pci' domain='0x0000' bus='0x{VM.pci.bus}' slot='0x{VM.pci.slot}' function='0x{VM.pci.function}'/>";
            defaultXML[56] = $"    <graphics type='vnc' port='{VM.portVNC}' autoport='no' listen='127.0.0.1'>";

            SaveFileDialog file = new SaveFileDialog
            {
                FileName = $"uXXYY.xml",
                Filter = "XML-File | *.xml"
            };
            file.ShowDialog();

            using (StreamWriter filewrite = new StreamWriter(file.FileName))
            {
                for (int i = 0; i < defaultXML.Count; i++)
                {
                    filewrite.WriteLine(String.Format(defaultXML[i].ToString()));
                }
            }
        }

        private void btn_SaveAsNew_Click(object sender, RoutedEventArgs e)
        {
            VirtualMachine VM = new VirtualMachine(tb_Name.Text, Convert.ToInt32(tb_RAM.Text), Convert.ToInt32(tb_CPU.Text), tb_MAC.Text, tb_BUS.Text, tb_Slot.Text, Convert.ToInt32(tb_Function.Text), Convert.ToInt32(tb_VNC.Text));
            using (StreamWriter filewrite = new StreamWriter(@"Presets.csv"))
            {
                for (int i = 0; i < Presets.Count; i++)
                {
                    filewrite.WriteLine(String.Format(Presets[i].ToString()));
                }
                filewrite.WriteLine(String.Format(VM.ToString()));
            }
            cb_VMs.Items.Clear();
            for (int i = 0; i < Presets.Count; i++)
            {
                cb_VMs.Items.Add(Presets[i].name);
            }
        }
        public void ReadDefaultXML(List<string> XML)
        {
            using (var reader = new StreamReader(@"uXXYY.xml"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    XML.Add(values[0]);
                }
            }
        }
        public void ReadPresets(List<VirtualMachine> Presets) 
        {
            using (var reader = new StreamReader(@"Presets.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values=line.Split(',');
                    Presets.Add(new VirtualMachine(values[0], Convert.ToInt32(values[1]), Convert.ToInt32(values[2]), values[3], values[4], values[5], Convert.ToInt32(values[6]), Convert.ToInt32(values[7])));
                }
            }
        }

        private void cb_VMs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int selectedIndex = cb_VMs.SelectedIndex;
                tb_Name.Text = Presets[selectedIndex].name;
                tb_RAM.Text = Presets[selectedIndex].memoryRAM.ToString();
                tb_CPU.Text = Presets[selectedIndex].coresCPU.ToString();
                tb_MAC.Text = Presets[selectedIndex].macAddress;
                tb_BUS.Text = Presets[selectedIndex].pci.bus;
                tb_Slot.Text = Presets[selectedIndex].pci.slot;
                tb_Function.Text = Presets[selectedIndex].pci.function.ToString();
                tb_VNC.Text = Presets[selectedIndex].portVNC.ToString();
            }
            catch (Exception ex) { }
        }
    }
}
