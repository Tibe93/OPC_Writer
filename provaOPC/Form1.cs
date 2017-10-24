using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Opc.Da;
using Opc;
using System.IO;

namespace provaOPC
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static ItemValueResult RsLinx_OPC_Client_Read(string ItemName)
        {
            Opc.Da.Server server;
            OpcCom.Factory fact = new OpcCom.Factory();
            Opc.Da.Subscription groupRead;
            Opc.Da.SubscriptionState groupState;
            Opc.Da.Item[] items = new Opc.Da.Item[1];
            // 1st: Create a server object and connect to the RSLinx OPC Server
            server = new Opc.Da.Server(fact, null);
            server.Url = new Opc.URL("opcda://localhost/RSLinx OPC Server/{A05BB6D6-2F8A-11D1-9BB0-080009D01446}");

            //2nd: Connect to the created server
            server.Connect();

            //3rd Create a group if items            
            groupState = new Opc.Da.SubscriptionState();
            groupState.Name = "Group";
            groupState.UpdateRate = 1000;// this isthe time between every reads from OPC server
            groupState.Active = true;//this must be true if you the group has to read value
            groupRead = (Opc.Da.Subscription)server.CreateSubscription(groupState);


            // add items to the group    (in Rockwell names are identified like [Name of PLC in the server]ItemName)
            items[0] = new Opc.Da.Item();
            items[0].ItemName = ItemName;

            items = groupRead.AddItems(items);
            return groupRead.Read(items)[0];
        }

        public static void RsLinx_OPC_Client_Write(string ItemName, float Value)
        {
            Opc.Da.Server server;
            OpcCom.Factory fact = new OpcCom.Factory();
            Opc.Da.Subscription groupWrite;
            Opc.Da.SubscriptionState groupStateWrite;
            Opc.Da.Item[] items = new Opc.Da.Item[1];
            // 1st: Create a server object and connect to the RSLinx OPC Server
            server = new Opc.Da.Server(fact, null);
            server.Url = new Opc.URL("opcda://localhost/RSLinx OPC Server/{A05BB6D6-2F8A-11D1-9BB0-080009D01446}");

            //2nd: Connect to the created server
            server.Connect();

            // Create a write group            
            groupStateWrite = new Opc.Da.SubscriptionState();
            groupStateWrite.Name = "Group Write";
            groupStateWrite.Active = false;//not needed to read if you want to write only
            groupWrite = (Opc.Da.Subscription)server.CreateSubscription(groupStateWrite);

            //Create the item to write (if the group doesn't have it, we need to insert it)
            Opc.Da.Item[] itemToAdd = new Opc.Da.Item[1];
            itemToAdd[0] = new Opc.Da.Item();
            itemToAdd[0].ItemName = ItemName;

            //create the item that contains the value to write
            Opc.Da.ItemValue[] writeValues = new Opc.Da.ItemValue[1];
            writeValues[0] = new Opc.Da.ItemValue(itemToAdd[0]);

            //make a scan of group to see if it already contains the item
            bool itemFound = false;
            foreach (Opc.Da.Item item in groupWrite.Items)
            {
                if (item.ItemName == itemToAdd[0].ItemName)
                {
                    // if it find the item i set the new value
                    writeValues[0].ServerHandle = item.ServerHandle;
                    itemFound = true;
                }
            }
            if (!itemFound)
            {
                //if it doesn't find it, we add it
                groupWrite.AddItems(itemToAdd);
                writeValues[0].ServerHandle = groupWrite.Items[groupWrite.Items.Length - 1].ServerHandle;
            }
            //set the value to write
            writeValues[0].Value = Value;
            //write
            groupWrite.Write(writeValues);
        }

        private void butScrivi_Click(object sender, EventArgs e)
        {
            StreamReader File = new StreamReader(textBoxPath.Text);
            string Ppm = openFileDialog1.SafeFileName.Split('_')[0];
            File.ReadLine();//Salto la prima...
            File.ReadLine();//... e la seconda riga perchè sono legende
            string[] Line = new string[5];
            double progresso = 0.0;
            progressBar1.Value = 0;
            for (int i = 0; i < 1250; i++)
            {
                Line = File.ReadLine().Split('\t');//Leggo la linea
                RsLinx_OPC_Client_Write($"[{textBoxTopic.Text}]Posizione_{Ppm}[{i}]", float.Parse(Line[1]));//0 Time, 1 Pos, 2 Vel, 3 Cor
                RsLinx_OPC_Client_Write($"[{textBoxTopic.Text}]Velocita_{Ppm}[{i}]", float.Parse(Line[2]));
                RsLinx_OPC_Client_Write($"[{textBoxTopic.Text}]Corrente_{Ppm}[{i}]", float.Parse(Line[3]));
                progresso += (double) 100 / 1250;
                progressBar1.Value = (int) progresso;
            }
            progressBar1.Value = 100;
            textBoxPath.BackColor = Color.LightGreen;
        }

        private void butPath_Click(object sender, EventArgs e)
        {
            //Utilizzo una finestra di dialogo per selezionare la cartella dove sono salvati i .CSV
            openFileDialog1.ShowDialog();
            textBoxPath.Text = openFileDialog1.FileName;
        }

        private void textBoxPath_TextChanged(object sender, EventArgs e)
        {
            //Operazioni sulla grafica
            textBoxPath.BackColor = Color.White;
            if (!textBoxPath.Text.Equals("Inserire Path Salvataggio .CSV"))
            {
                textBoxTopic.Enabled = true;
                textBoxTopic.BackColor = Color.LightGreen;
                if (textBoxTopic.Text.Equals("Creg_OPC_Topic"))
                {
                    butScrivi.Enabled = true;
                    textBoxTopic.BackColor = Color.White;
                }
            }
        }

        private void textBoxTopic_Click(object sender, EventArgs e)
        {
            //Operazioni sulla grafica
            if (textBoxTopic.Text.Equals("Inserire il nome del topic OPC"))
            {
                textBoxTopic.Text = "";
            }
        }

        private void textBoxTopic_TextChanged(object sender, EventArgs e)
        {
            //Operazioni sulla grafica
            if (!(textBoxTopic.Text.Equals("Inserire il nome del Topic OPC") || textBoxTopic.Text.Equals("")))
            {
                butScrivi.Enabled = true;
            }
            textBoxTopic.BackColor = Color.White;
        }

        private void textBoxTopic_Leave(object sender, EventArgs e)
        {
            //Operazioni sulla grafica
            if (textBoxTopic.Text.Equals(""))
            {
                textBoxTopic.Text = "Inserire il nome del Topic OPC";
                textBoxTopic.BackColor = Color.LightGreen;
                butScrivi.Enabled = false;
            }
        }
    }
}
