using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XMLGenerator.Classes;

namespace XMLGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> defaultXML = new List<string>();
        public List<VirtualMachine> Presets = new List<VirtualMachine>();
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
            defaultXML[10] = $"  <currentMemory unit='GiB'>{VM.memoryRAM}</currentMemory>";
            defaultXML[11] = $"  <vcpu placement='static'>{VM.coresCPU}</vcpu>";
            defaultXML[13] = $"    <type arch='x86_64' machine='pc'>hvm</type>";
            defaultXML[37] = $"      <mac address='{VM.macAddress}'/> ";
            defaultXML[44] = $"        <address type='pci' domain='0x0000' bus='0x{VM.pci.bus}' slot='0x{VM.pci.slot}' function='0x{VM.pci.function}'/>";
            defaultXML[56] = $"    <graphics type='vnc' port='{VM.portVNC}' autoport='no' listen='127.0.0.1'>";

            SaveFileDialog file = new SaveFileDialog
            {
                FileName = $"{VM.name}.xml",
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

            Presets.Add(VM);
            cb_VMs.Items.Add(VM.name);
            cb_VMs.SelectedIndex = cb_VMs.Items.Count - 1;

            SavePresets();
        }

        private void btn_DelVM_Click(object sender,RoutedEventArgs e) 
        {
            int index = cb_VMs.SelectedIndex;
            if (index != 0) 
            {
                cb_VMs.SelectedIndex = 0;
            }
            else if (index == 0&&Presets.Count>=2) 
            {
                cb_VMs.SelectedIndex=1;
            }

            Presets.RemoveAt(index);
            cb_VMs.Items.RemoveAt(index);

            SavePresets();
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
                    var values = line.Split(',');
                    Presets.Add(new VirtualMachine(values[0], Convert.ToInt32(values[1]), Convert.ToInt32(values[2]), values[3], values[4], values[5], Convert.ToInt32(values[6]), Convert.ToInt32(values[7])));
                }
            }
        }

        private void cb_VMs_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void MACValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9a-f:]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        public void SavePresets() 
        {
            using (StreamWriter filewrite = new StreamWriter(@"Presets.csv"))
            {
                for (int i = 0; i < Presets.Count; i++)
                {
                    filewrite.WriteLine(String.Format(Presets[i].ToString()));
                }
            }
        }
    }
}
