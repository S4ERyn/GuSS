using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GuSS
{

    public partial class MainScreen : Form
    {
        public Cerebellum Brain;
        public MainScreen()
        {
            InitializeComponent();
            UpdateStatusBarMessage.OnNewStatusMessage += UpdateStatusBarMessage_OnNewStatusMessage;
            splitContainer1.Resize += splitContainer1_Resize;
            btnGo.Click += button1_Click;
            Brain = new Cerebellum();
            Brain.Init();
            //highlight the text field for typing
            this.ActiveControl = txtCommand;

        }

        void button1_Click(object sender, EventArgs e)
        {
            string newText = txtCommand.Text;
            txtCommand.Text = string.Empty;
            Brain.Command(newText);
        }

        
        void splitContainer1_Resize(object sender, EventArgs e)
        {
            ResizeInterface();
        }

        private void ResizeInterface()
        {
            //splitContainer1.Panel2.Height = 200;
            //splitContainer2.Panel2.Width = 75;
            btnGo.Height = splitContainer2.Panel2.Height;
            btnGo.Width = splitContainer2.Panel2.Width;
            txtCommand.Width = splitContainer2.Panel1.Width;
            txtCommand.Height = splitContainer2.Panel1.Height;
        }

        

        void UpdateStatusBarMessage_OnNewStatusMessage(string strMessage)
        {
            WriteText(strMessage);
        }

        public void WriteText(string texttoprint)
        {
            richTextBox1.Text = DateTime.Now.ToString() + ": " + texttoprint + Environment.NewLine + richTextBox1.Text;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //nothing here when button is clicked
            string newText = txtCommand.Text;
            Brain.Command(newText);
            txtCommand.Text = string.Empty;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13) 
            {
                //enter was pressed
                Brain.Command(txtCommand.Text);
                txtCommand.Text = string.Empty;
            }
        }

    }

    public delegate void AddStatusMessageDelegate(string strMessage);

    public static class UpdateStatusBarMessage
    {

        public static Form mainwin;
        public static event AddStatusMessageDelegate OnNewStatusMessage;
        public static void ShowStatusMessage(string strMessage)
        {
            ThreadSafeStatusMessage(strMessage);
        }

        private static void ThreadSafeStatusMessage(string strMessage)
        {
            if (mainwin != null && mainwin.InvokeRequired)  // we are in a different thread to the main window
                mainwin.Invoke(new AddStatusMessageDelegate(ThreadSafeStatusMessage), new object[] { strMessage });  // call self from main thread
            else
                OnNewStatusMessage(strMessage);
        }
    }


}
