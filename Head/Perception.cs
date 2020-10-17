using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Recognition; //for actual recognition, uses SDK 11



namespace GuSS
{
    class Perception
    {
        public SpeechRecognitionEngine eardrum;
        public Perception()
        {

            //eardrum = new SpeechRecognitionEngine();
            
            
            //eardrum = LoadDictationGrammars2();
            //eardrum = _LoadDictationGrammars();
            eardrum = LoadDictationGrammars();
            //eardrum.UpdateRecognizerSetting("ResourceUsage", 100);
            eardrum.UpdateRecognizerSetting("CFGConfidenceRejectionThreshold", 80);
            eardrum.UpdateRecognizerSetting("ResponseSpeed", 1000);
            eardrum.UpdateRecognizerSetting("ComplexResponseSpeed", 1500);
            eardrum.SetInputToDefaultAudioDevice();
            eardrum.RecognizeAsync(RecognizeMode.Multiple);
            eardrum.AudioStateChanged += new EventHandler<AudioStateChangedEventArgs>(AudioStateChangedHandler);

        }

        private SpeechRecognitionEngine LoadDictationGrammars()
        {

            GrammarBuilder startStop = new GrammarBuilder();
            GrammarBuilder dictation = new GrammarBuilder();
            dictation.AppendDictation();

            //startStop.Append(new SemanticResultKey("StartDictation", new SemanticResultValue("wake up Gus", true)));
            startStop.Append(new SemanticResultKey("DictationInput", dictation));
            //startStop.Append(new SemanticResultKey("StopDictation", new SemanticResultValue("go to sleep Gus", false)));

            // Add Custom List of Commands to be sure they're in context
            Choices commands = new Choices();
            commands.Add(new string[] { "weather", "shopping list", "music", "news", "start", "stop", "voice" });

            dictation.Append(commands);

            Grammar grammar = new Grammar(startStop);
            grammar.Enabled = true;
            grammar.Name = " Free-Text Dictation ";

            // Create a SpeechRecognitionEngine object and add the grammars to it.
            SpeechRecognitionEngine recoEngine = new SpeechRecognitionEngine();
            recoEngine.LoadGrammar(grammar);
            // Sets all the timeout values
            recoEngine.BabbleTimeout = TimeSpan.FromSeconds(1);
            recoEngine.EndSilenceTimeout = TimeSpan.FromSeconds(1);
            recoEngine.EndSilenceTimeoutAmbiguous = TimeSpan.FromSeconds(1);
            recoEngine.InitialSilenceTimeout = TimeSpan.FromSeconds(3);
            recoEngine.MaxAlternates = 10;
           
            return recoEngine;
        }

        private SpeechRecognitionEngine _LoadDictationGrammars()
        {
            // Create a SpeechRecognitionEngine object and add the grammars to it.
            SpeechRecognitionEngine recoEngine = new SpeechRecognitionEngine();

            // Add Custom List of Commands to be sure they're in context
            Choices commands = new Choices();
            commands.Add(new string[] { "weather", "shopping list", "music", "news", "start", "stop", "voice" });
            
            // Create a GrammarBuilder object and append the Choices object.
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(commands);
            //gb.AppendDictation();

            // Create the Grammar instance and load it into the speech recognition engine.
            Grammar g = new Grammar(gb);
            recoEngine.LoadGrammar(g);

            return recoEngine;
        }

        static readonly string[] settings = new string[]
        {
                "ResourceUsage",
                "ResponseSpeed",
                "CopmplexResponseSpeed",
                "AdaptationOn",
                "PersistedBackgroundAdaptation",
        };

        private SpeechRecognitionEngine LoadDictationGrammars2()
        {
            Console.WriteLine("Settings for recognizer {0}:",
  eardrum.RecognizerInfo.Name);
            Console.WriteLine();

            // List the current settings.  
            ListSettings(eardrum);

            return eardrum;
        }

        private static void ListSettings(SpeechRecognitionEngine recognizer)
        {
            foreach (string setting in settings)
            {
                try
                {
                    object value = recognizer.QueryRecognizerSetting(setting);
                    Console.WriteLine("  {0,-30} = {1}", setting, value);
                }
                catch
                {
                    Console.WriteLine("  {0,-30} is not supported by this recognizer.",
                      setting);
                }
            }
            Console.WriteLine();
        }
    

    void Listen(string _textreceived)
        { 
           
        }

        private void HeardSomething(object sender, SpeechRecognizedEventArgs e)
        {
            //const int five = 5;
        }

        // Handle the AudioStateChanged event.  
        static void AudioStateChangedHandler(
          object sender, AudioStateChangedEventArgs e)
        {
            Console.WriteLine("AudioStateChanged ({0}): {1}", DateTime.Now.ToString("mm:ss.f"), e.AudioState);
        } 

    }
}
