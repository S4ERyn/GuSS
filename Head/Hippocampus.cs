using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuSS.Head
{
    class Hippocampus
    { //Logging and other long term to short term storage conversion functions

        public Hippocampus()
        {
            //streamr = new StreamWriter(Properties.Settings.Default.log, true);
        }

        public void SomethingCleverToSaveALog(string stufftolog)
        {

            string fileName = Properties.Settings.Default.log;
            string textToAdd = stufftolog;
            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Append);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("{0} {1} {2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), stufftolog);
                }
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

            //DoLogging(stufftolog, streamr);
        }

        private void DoLogging(string stufftolog, TextWriter writ)
        {
            
            writ.Write("\r\nLog Entry : ");
            writ.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            writ.WriteLine("  :");
            writ.WriteLine("  :{0}", stufftolog);
            writ.WriteLine("-------------------------------");
        }

        
    }
}
