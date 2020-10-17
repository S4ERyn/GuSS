using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace GuSS
{
    class Larynx
    {

        public SpeechSynthesizer synth;
        public bool voiceON;
        

        public Larynx()
        {
            voiceON = true;
            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            
        }

        public delegate void SaySomethingHandler(object sender, EventArgs e);
        public event SaySomethingHandler SaySomething;

        public void Speak(string TextToSpeak)
        {
            UpdateStatusBarMessage.ShowStatusMessage(TextToSpeak);
            if (voiceON) 
            {
            synth.Speak(TextToSpeak);
            }
            
        }

        public void Info()
        {

            Speak("I can speak in several different voices.  For instance...");
            string currentVoice = this.synth.Voice.Name;

            // Output information about all of the installed voices. 
            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                VoiceInfo info = voice.VoiceInfo;
                ChangeVoice(info.Name);
                Speak("There is " + info.Name); //+ " from " + info.Culture + ".");
                
                if (info.Gender == VoiceGender.Female) 
                {
                    Speak("She is " + info.Age + " and " + info.Culture);
                }
                else if (info.Gender == VoiceGender.Male) 
                {
                    Speak("He is " + info.Age + " and " + info.Culture);
                }

                ChangeVoice(currentVoice);
                Speak("Currently using " + currentVoice);

            }
        }
            
        void Playback(byte[] VoiceObject)
        { 
        }


        internal void ChangeVoice(string _command)
        {
            string holder = string.Empty;
            bool match = false;

            foreach (InstalledVoice voice in synth.GetInstalledVoices()) 
            {
                string voiceCheck = voice.VoiceInfo.Name;
                voiceCheck = voiceCheck.Replace("Microsoft","").Replace("Desktop","").Trim(); //all voices include Microsoft and Desktop

                if (_command.Contains(voiceCheck))
                {
                    holder = voice.VoiceInfo.Name;
                    match = true;
                }
                
            }

            if (match) {
                synth.SelectVoice(holder);
            }
            else
            {
                Speak("I was unable to find a matching voice to change to.");
            }

        }
    }
}
