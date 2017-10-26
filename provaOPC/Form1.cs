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
using System.Collections;

namespace provaOPC
{
    public partial class Form1 : Form
    {
        public static string Url = "opcda://localhost/RSLinx OPC Server/{A05BB6D6-2F8A-11D1-9BB0-080009D01446}"; // URL del Server OPC di RSLinx
        public static int UpdateRate = 1000; // Tempo di aggiornamento dei gruppi dell'OPC

        public Form1()
        {
            InitializeComponent();
        }

        public static ItemValueResult RsLinx_OPC_Client_Read(string ItemName)
        {
            try
            {
                //Creo un istanza di OPC.server
                Opc.Da.Server server;
                //Parametro necessario alla connect
                OpcCom.Factory fact = new OpcCom.Factory();
                //Creo un istanza di Sottoscrizione
                Opc.Da.Subscription groupRead;
                //Creo un istanza di SubscriptionState, utile per controllare lo stato della sottoscrizione
                Opc.Da.SubscriptionState groupState;
                //Creo un array di OPC.Da.Item
                Opc.Da.Item[] items = new Opc.Da.Item[1];
                //Setto factory e url del server, come url utilizzo quello del RSLinx OPC Server
                server = new Opc.Da.Server(fact, null);
                server.Url = new Opc.URL(Url);

                //Connetto il server
                server.Connect();

                //Istanzio la sottoscrizione           
                groupState = new Opc.Da.SubscriptionState();
                groupState.Name = "Group";
                groupState.UpdateRate = UpdateRate;//Setto il tempo di Refresh del gruppo
                groupState.Active = true;//Questo valore deve essere true se voglio aver la possibilità di leggere
                //Creo il gruppo sul server
                groupRead = (Opc.Da.Subscription)server.CreateSubscription(groupState);

                //Istanzio l'Item
                items[0] = new Opc.Da.Item();
                //Gli do il nome (Rockwell utilizza questa formzattazione dei nomi [NomeTopicOPC]NomeTag es. [MyOPCTopic]Posizione)
                items[0].ItemName = ItemName;

                //Aggiungo l'oggetto al gruppo
                items = groupRead.AddItems(items);
                //Leggo il valore dell'item aggiunto
                ItemValueResult Ritorno = groupRead.Read(items)[0];
                //Controllo che la lettura sia andata a buon fine, se non è così lancio un'eccezione
                if (!Ritorno.ResultID.Name.Name.Equals("S_OK"))
                {
                    throw new System.Exception("Errore lettura OPC Tag");
                }
                return groupRead.Read(items)[0];
            }
            catch (Exception ex)
            {
                //Se viene lanciata un'eccezione ritorno un ItemValueResult con valore -1 e mostro un Messagebox con l'errore
                MessageBox.Show(ex.Message);
                ItemValueResult Errore = new ItemValueResult();
                Errore.Value = -1;
                return Errore;
            }
        }

        public static void RsLinx_OPC_Client_Write(string ItemName, int Value)
        {
            try
            {
                //Creo un istanza di OPC.server
                Opc.Da.Server server;
                //Parametro necessario alla connect
                OpcCom.Factory fact = new OpcCom.Factory();
                //Creo un istanza di Sottoscrizione
                Opc.Da.Subscription groupWrite;
                //Creo un istanza di SubscriptionState, utile per controllare lo stato della sottoscrizione
                Opc.Da.SubscriptionState groupStateWrite;
                //Creo un array di OPC.Da.Item
                Opc.Da.Item[] items = new Opc.Da.Item[1];
                //Setto factory e url del server, come url utilizzo quello del RSLinx OPC Server
                server = new Opc.Da.Server(fact, null);
                server.Url = new Opc.URL(Url);

                //Connetto il server
                server.Connect();

                //Istanzio la sottoscrizione                    
                groupStateWrite = new Opc.Da.SubscriptionState();
                groupStateWrite.Name = "Group Write";
                //Questo valore deve essere true se voglio aver la possibilità di leggere, se devo solo scrivere lo metto false
                groupStateWrite.Active = false;
                //Creo il gruppo sul server
                groupWrite = (Opc.Da.Subscription)server.CreateSubscription(groupStateWrite);

                //Creo l'Item da scrivere (se il gruppo non lo possiede, lo devo inserire)
                Opc.Da.Item[] itemToAdd = new Opc.Da.Item[1];
                itemToAdd[0] = new Opc.Da.Item();
                itemToAdd[0].ItemName = ItemName;

                //Creo l'istanza di ItemValue che possiede il mio Item e il valore che voglio assegnargli
                Opc.Da.ItemValue[] writeValues = new Opc.Da.ItemValue[1];
                writeValues[0] = new Opc.Da.ItemValue(itemToAdd[0]);

                //Controllo se l'oggetto esiste nel gruppo
                bool itemFound = false;
                foreach (Opc.Da.Item item in groupWrite.Items)
                {
                    if (item.ItemName == itemToAdd[0].ItemName)
                    {
                        //Se lo trovo gli setto il nuovo valore
                        writeValues[0].ServerHandle = item.ServerHandle;
                        itemFound = true;
                    }
                }
                if (!itemFound)
                {
                    //Se non ho trovato l'oggetto nel gruppo lo aggiungo..
                    groupWrite.AddItems(itemToAdd);
                    writeValues[0].ServerHandle = groupWrite.Items[groupWrite.Items.Length - 1].ServerHandle;
                }
                //...gli setto il valore
                writeValues[0].Value = Value;
                //e lo scrivo
                groupWrite.Write(writeValues);
            }
            catch (Exception ex)
            {
                //Se viene lanciata un'eccezione la mostro
                MessageBox.Show(ex.Message);
            }
        }

        public static ItemValueResult[] RsLinx_OPC_Client_Read_Array(string ItemName, int Length)
        {
            try
            {
                //Creo un istanza di OPC.server
                Opc.Da.Server server;
                //Parametro necessario alla connect
                OpcCom.Factory fact = new OpcCom.Factory();
                //Creo un istanza di Sottoscrizione
                Opc.Da.Subscription groupRead;
                //Creo un istanza di SubscriptionState, utile per controllare lo stato della sottoscrizione
                Opc.Da.SubscriptionState groupState;
                //Creo un array di OPC.Da.Item
                Opc.Da.Item[] items = new Opc.Da.Item[1];
                //Setto factory e url del server, come url utilizzo quello del RSLinx OPC Server
                server = new Opc.Da.Server(fact, null);
                server.Url = new Opc.URL(Url);

                //Connetto il server
                server.Connect();

                //Istanzio la sottoscrizione           
                groupState = new Opc.Da.SubscriptionState();
                groupState.Name = "Group";
                groupState.UpdateRate = UpdateRate;//Setto il tempo di Refresh del gruppo
                groupState.Active = true;//Questo valore deve essere true se voglio aver la possibilità di leggere
                //Creo il gruppo sul server
                groupRead = (Opc.Da.Subscription)server.CreateSubscription(groupState);
                //Istanzio l'Item
                items[0] = new Opc.Da.Item();
                //Gli do il nome (Rockwell utilizza questa formzattazione dei nomi per gli array
                //[NomeTopicOPC]NomeTag,LDimensioneArray es. [MyOPCTopic]Posizione,L50)
                items[0].ItemName = $"{ItemName},L{Length}";

                //Aggiungo l'oggetto al gruppo
                items = groupRead.AddItems(items);
                //Leggo il valore dell'item aggiunto
                ItemValueResult[] Ritorno = groupRead.Read(items);

                //Controllo che la lettura dell'array sia andata a buon fine, se non è così lancio un'eccezione
                if (!Ritorno[0].ResultID.Name.Name.Equals("S_OK"))
                {
                    throw new System.Exception("Errore lettura OPC Tag");
                }
                return groupRead.Read(items);
            }
            catch (Exception ex)
            {
                //Se viene lanciata un'eccezione ritorno un array di ItemValueResult con il primo che ha valore -1 e mostro un Messagebox con l'errore
                MessageBox.Show(ex.Message);
                ItemValueResult[] Errore = new ItemValueResult[1];
                Errore[0] = new ItemValueResult();
                float[] Err = { (float)-1, (float)-1 };
                Errore[0].Value = Err;
                return Errore;
            }
        }

        public static void RsLinx_OPC_Client_Write_Array(string ItemName,int Lenght, float[] Value)
        {
            try
            {
                //Creo un istanza di OPC.server
                Opc.Da.Server server;
                //Parametro necessario alla connect
                OpcCom.Factory fact = new OpcCom.Factory();
                //Creo un istanza di Sottoscrizione
                Opc.Da.Subscription groupWrite;
                //Creo un istanza di SubscriptionState, utile per controllare lo stato della sottoscrizione
                Opc.Da.SubscriptionState groupStateWrite;
                //Creo un array di OPC.Da.Item
                Opc.Da.Item[] items = new Opc.Da.Item[1];
                //Setto factory e url del server, come url utilizzo quello del RSLinx OPC Server
                server = new Opc.Da.Server(fact, null);
                server.Url = new Opc.URL(Url);

                //Connetto il server
                server.Connect();

                //Istanzio la sottoscrizione                    
                groupStateWrite = new Opc.Da.SubscriptionState();
                groupStateWrite.Name = "Group Write";
                //Questo valore deve essere true se voglio aver la possibilità di leggere, se devo solo scrivere lo metto false
                groupStateWrite.Active = false;
                //Creo il gruppo sul server
                groupWrite = (Opc.Da.Subscription)server.CreateSubscription(groupStateWrite);

                //Creo l'Item da scrivere (se il gruppo non lo possiede, lo devo inserire)
                Opc.Da.Item[] itemToAdd = new Opc.Da.Item[1];
                itemToAdd[0] = new Opc.Da.Item();
                itemToAdd[0].ItemName = $"{ItemName},L{Lenght}";

                //Creo l'istanza di ItemValue che possiede il mio Item e il valore che voglio assegnargli
                Opc.Da.ItemValue[] writeValues = new Opc.Da.ItemValue[1];
                writeValues[0] = new Opc.Da.ItemValue(itemToAdd[0]);

                //Controllo se l'oggetto esiste nel gruppo
                bool itemFound = false;
                foreach (Opc.Da.Item item in groupWrite.Items)
                {
                    if (item.ItemName == itemToAdd[0].ItemName)
                    {
                        //Se lo trovo gli setto il nuovo valore
                        writeValues[0].ServerHandle = item.ServerHandle;
                        itemFound = true;
                    }
                }
                if (!itemFound)
                {
                    //Se non ho trovato l'oggetto nel gruppo lo aggiungo..
                    groupWrite.AddItems(itemToAdd);
                    writeValues[0].ServerHandle = groupWrite.Items[groupWrite.Items.Length - 1].ServerHandle;
                }
                //...gli setto il valore
                writeValues[0].Value = Value;
                //e lo scrivo
                groupWrite.Write(writeValues);
            }
            catch (Exception ex)
            {
                //Se viene lanciata un'eccezione la mostro
                MessageBox.Show(ex.Message);
            }
        }

        private void butScrivi_Click(object sender, EventArgs e)
        {
            StreamReader File = new StreamReader(textBoxPath.Text);
            string Ppm = openFileDialog1.SafeFileName.Split('_')[0];
            File.ReadLine();//Salto la prima,...
            File.ReadLine();//...la seconda,...
            File.ReadLine();//...la terza...
            File.ReadLine();//... e la quarta riga perchè sono legende
            string[] Line = new string[5];
            int LengthArray = 100;
            int Total = 1200;
            float[] PosNow = new float[LengthArray];
            float[] VelNow = new float[LengthArray];
            float[] CorNow = new float[LengthArray];
            double progresso = 0.0;
            progressBar1.Value = 0;

            float[] test = { (float)1.22, (float)2.3, (float)2.3, (float)2.3 };
            
            for (int i = 0; i < Total / LengthArray; i++)
            {
                for (int j = 0; j < LengthArray; j++)
                {
                    Line = File.ReadLine().Split('\t');//Leggo la linea
                    PosNow[j] = float.Parse(Line[1]);//0 Time, 1 Pos, 2 Vel, 3 Cor
                    VelNow[j] = float.Parse(Line[2]);
                    CorNow[j] = float.Parse(Line[3]);
                }

                RsLinx_OPC_Client_Write_Array($"[{textBoxTopic.Text}]Posizione_{Ppm}[{i * LengthArray}]", LengthArray, (float[]) PosNow);
                RsLinx_OPC_Client_Write_Array($"[{textBoxTopic.Text}]Velocita_{Ppm}[{i * LengthArray}]", LengthArray, (float[]) VelNow);
                RsLinx_OPC_Client_Write_Array($"[{textBoxTopic.Text}]Corrente_{Ppm}[{i * LengthArray}]", LengthArray, (float[]) CorNow);
                progresso += (double) progressBar1.Maximum / LengthArray;
                progressBar1.Value = (int) progresso;
            }

            File.Close();
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
