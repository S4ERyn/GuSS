using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuSS.Utility.RSSReader;
using libZPlay;
using System.Speech;
using System.Speech.Recognition;
using GuSS.Head;
using System.Security.Cryptography.X509Certificates;
//new change to force git sync
namespace GuSS
{
    public class Cerebellum
    {
        Perception ears = new Perception();
        Larynx voice = new Larynx(); //an organ in the neck of mammals involved in breathing, and sound production. It manipulates pitch and volume.
        Hippocampus memory = new Hippocampus(); //Humans and other mammals have two hippocampi, one in each side of the brain. It belongs to the limbic system and plays important roles in the consolidation of information from short-term memory to long-term memory and spatial navigation. 

        public object ReceivedAudio { get; set; }
        public bool IsCommand;
        public bool IsBanter;
        public string TextReceived { get; set; }

        void ProcessInput(SpeechRecognizedEventArgs _receivedaudio)
        {
            string commandaudio = _receivedaudio.Result.Text;
            SemanticValue semantics = _receivedaudio.Result.Semantics;
            var confidence = _receivedaudio.Result.Confidence.ToString();
            var another = _receivedaudio.Result.Alternates.ToList();
            var another_string = "";

            foreach (RecognizedPhrase x in another)
            {
                another_string += x.Text + " score: " + x.Confidence + "| ";
            }
            string translatedaudio = semantics["DictationInput"].Value.ToString();

            LogIt("Audio: " + commandaudio);
            if (commandaudio != translatedaudio) {
                LogIt("Semantics: " + translatedaudio);
            }
            LogIt("Confidence: " + confidence);
            LogIt("Alternates: " + another_string);

            Command(commandaudio);
        }

        void LogIt(string stringtolog)
        {
            memory.SomethingCleverToSaveALog(stringtolog);
        }

        //play news, play music, shopping list, weather, what can I say, voice info, change voice

        #region "Command" 

        public void Command(string _command)
        {

            if (_command.Equals("play news"))
            {
                SaySomething("Starting news update...");
                PlayNews();
            }
            else if (_command.Equals("play music"))
            {
                PlayMusic();
            }
            else if (_command.Contains("shopping list"))
            {
                ShoppingList(_command);
            }
            else if (_command.Contains("weather"))
            {
                SaySomething("Retrieving weather...");
                Weather();
            }
            else if (_command.Contains("whether"))
            {
                SaySomething("Retrieving weather...");
                Weather();
            }
            else if (_command.Contains("stop"))
            {
                //TODO is music playing, news playing?
                //TODO way to track what is currently happening.
                //TODO is the next word talking?

                if (_command.Contains("talking"))
                {
                    //disablevoice
                    SaySomething("O K");
                    voice.voiceON = false;
                    SaySomething("I'll be quiet.");
                }
            }
            else if (_command.Contains("start"))  //use to toggle voice
            {
                //TODO is music playing, news playing?
                //TODO way to track what is currently happening.
                //TODO is the next word talking?
                if (_command.Contains("talking"))
                {
                    //disablevoice
                    SaySomething("O.K.");
                    voice.voiceON = true;
                    SaySomething("don't mind if I do");
                }
            }
            else if (_command.Contains("what,can,i,say".ToLower()))
            {

                //this is the general help text to relay the available commands
                //I can add items to a shopping list
                //I can play the latest news update
                //I can provide the latest weather update

                //Todo
                //What time is it
                //What is my schedule for today?

                //More advanced camera related
                //Follow Me
                //Sentry Mode
                HelpMe(_command);

            }
            else if (_command.Contains("voice info"))
            {

                voice.Info();
            }
            else if (_command.Contains("change voice"))
            {
                voice.ChangeVoice(_command);
            }
        }
        #endregion

        #region Music
        void PlayMusic()
        { }
        #endregion

        #region "Help / What can I say"
        void HelpMe(string _command)
        {

            string[] context = _command.Split(); //todo semantics should be stored in a second array so can be evaulated but primarily ignored
            string[] ignore = { "add", "and" };

        }
        #endregion

        #region "Shopping List"
        void ShoppingList(string _command)
        {
            var context = _command.Split(); //todo semantics should be stored in a second array so can be evaulated but primarily ignored
            string[] ignore = { "add", "put", "shopping", "list", "remind", "me", "buy", "to", "the", "and", "remove" };
            //string[] helplist = {"",""};
            switch (context[0])
            {
                case "add":
                    foreach (string element in context)
                    {
                        if (!ignore.Contains(element))
                        {
                            //how to extract a phrase versus a single word
                            AddToList(element);
                            SaySomething("Adding " + element + " to the shopping list.");
                        }

                    }

                    break;
                //case "send":

                //  break;
                case "review":
                case "read":
                    ReadTheList();
                    break;
                case "remove":
                    foreach (string element in context)
                    {
                        if (!ignore.Contains(element))
                        {
                            if (ValidItemForRemoval(element)) //checks that it's actually in the list
                            {
                                //how to extract a phrase versus a single word
                                RemoveFromList(element);
                                SaySomething("Removing " + element + " from the shopping list.");
                            }
                        }

                    }

                    break;
                default:
                    break;
            }

        }

        private bool ValidItemForRemoval(string element)
        {
            return Properties.Settings.Default.ShoppingList.Contains(element);
        }

        private void ReadTheList()
        {
            if (Properties.Settings.Default.ShoppingList != null)
            {
                SaySomething("Ok... here is the shopping list as I currently have it.  Total Items: " + Properties.Settings.Default.ShoppingList.Count);
                foreach (string x in Properties.Settings.Default.ShoppingList)
                {
                    SaySomething(x);
                }
            }

        }

        private void AddToList(string element)
        {
            try
            {
                System.Collections.Specialized.StringCollection newList = Properties.Settings.Default.ShoppingList;
                newList.Add(element);
                Properties.Settings.Default.ShoppingList = newList;

                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                LogIt("Unable to add " + element + " to the shopping list." + "/r/nError: " + ex.Message);
            }
        }

        private void RemoveFromList(string element)
        {
            try
            {
                System.Collections.Specialized.StringCollection newList = Properties.Settings.Default.ShoppingList;
                if (newList.Contains(element))
                {
                    newList.Remove(element);
                    Properties.Settings.Default.ShoppingList = newList;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    SaySomething("Unable to find " + element + " in the current shopping list.");
                }
            }
            catch (Exception ex)
            {
                LogIt("Unable to remove " + element + " from the shopping list." + "/r/nError: " + ex.Message);
            }
        }
        #endregion
        
        #region "Weather"
        void Weather()
        {
            //http://forecast.weather.gov/MapClick.php?CityName=Holyoke&state=MA&site=BOX&textField1=42.2135&textField2=-72.6424&e=0#.VNOn753F8bI
            //RSS http://w1.weather.gov/xml/current_obs/KBAF.rss
            //RSSWWLP http://wwlp.com/category/weather/feed/
            string uri;

            bool gov;
            gov = false; //if it goes government or wwlp
            RssReader weather = new RssReader();

            string forecast = string.Empty;
            switch (true)    //each has their own formatting to strip
            {
                case true:

                    uri = "https://w1.weather.gov/xml/current_obs/KHFD.rss";
                    //forecast = weather.ReadRSS(uri);
                    //forecast = weather.ReadWeb(uri);
                    forecast = weather.ReadXML(uri);
                    //forecast = forecast.Substring(forecast.IndexOf("<br />") + "<br />".Length);
                    forecast = forecast.Replace("\n", " ");
                    break;
                case false:
                    uri = "https://wwlp.com/category/weather/feed/";
                    forecast = weather.ReadRSS(uri);
                    
                    forecast = forecast.Substring(0, forecast.IndexOf("<description>"));
                    break;
                default:
                    break;
            }

            if (forecast == string.Empty)
            {
                forecast = "I was unable to find weather information";
            }

            SaySomething(forecast);
            KeepListening();
        }
        #endregion

        #region "News" 
        void PlayNews()
        {
            String _newsurl = "http://public.npr.org/anon.npr-podcasts/podcast/500005/376418824/npr_376418824.mp3?dl=1";


            WebClient Client = new WebClient();
            Client.DownloadFile(_newsurl, "nprnews.mp3");

            //System.Diagnostics.ProcessStartInfo thePSI = new System.Diagnostics.ProcessStartInfo("wmplayer");
            //thePSI.Arguments = _newsurl;
            //thePSI.CreateNoWindow = true;
            //thePSI.UseShellExecute = true;
            //System.Diagnostics.Process.Start(thePSI);


            #endregion

        #region "MP3Player"
            ZPlay player = new ZPlay();
            if (player.OpenFile("nprnews.mp3", TStreamFormat.sfAutodetect) == false)
            {
                // error
                System.Windows.Forms.MessageBox.Show("can't do it");

                //Windows 8.1 Way
                //var dialog = new MessageDialog("Are you sure?");
                //dialog.Title = "Really?";
                //dialog.Commands.Add(new UICommand { Label = "Ok", Id = 0 });
                //dialog.Commands.Add(new UICommand { Label = "Cancel", Id = 1 });
                //var res = await dialog.ShowAsync();

            }
            //TODO announce how long the file is
            //TODO convert to direct stream of the mp3 file from the source, needs to create a memory
            //stream in order to stream file.  LoadDynamicInfo and StreamLength should be possible
            else
            {
                player.StartPlayback();
            }


        }
        //TODO break mp3 player into its own function for program-wide use

        #endregion


        public void Init()
        {
            SaySomething("Loading...");
            string greeting;
            int timeofday = DateTime.Now.Hour;
            if (timeofday >= 6 && timeofday <= 12)
            {
                greeting = "Good Morning";
            }
            else if (timeofday >= 12 && timeofday <= 17)
            {
                greeting = "Good Afternoon";
            }
            else if (timeofday >= 18 && timeofday <= 19)
            {
                greeting = "Good Evening";
            }
            else
            {
                greeting = "Hello";
            }
            SaySomething(greeting);
            Listen();
        }


        internal void Listen()
        {
            ears.eardrum.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(ears_HeardSomething);
            ears.eardrum.SpeechRecognitionRejected += new EventHandler<SpeechRecognitionRejectedEventArgs>(ears_badtalk);
        }

        void ears_HeardSomething(object sender, SpeechRecognizedEventArgs e)
        {
            //this will trigger when something is "heard"
            SaySomething("You said: " + e.Result.Text); //DEBUG
            ProcessInput(e); //Send it for anaysis

        }

        void ears_badtalk(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            SaySomething("I didn't really hear that: " + e.Result.Text);
        }

        void KeepListening()
        {
            
            ears.eardrum.RecognizeAsync(RecognizeMode.Multiple);
            

        }

        void SaySomething(string TextToSpeak)
        {
            voice.Speak(TextToSpeak);
        }
    }
}
