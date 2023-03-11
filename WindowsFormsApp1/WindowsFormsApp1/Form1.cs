using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using static System.Windows.Forms.LinkLabel;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();
        SpeechSynthesizer Samuel = new SpeechSynthesizer();
        SpeechRecognitionEngine startlistening = new SpeechRecognitionEngine();
        Random rnd = new Random();
        int RecTimeOut = 0;
        DateTime TimeNow = DateTime.Now;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _recognizer.SetInputToDefaultAudioDevice();
            _recognizer.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Default_SpeechRecognized);
            _recognizer.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(_recognizer_SpeechRecognized);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);

            startlistening.SetInputToDefaultAudioDevice();
            startlistening.LoadGrammarAsync(new Grammar(new GrammarBuilder(new Choices(File.ReadAllLines(@"DefaultCommands.txt")))));
            startlistening.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(startlistening_SpeechRecognized);
        }

        private void Default_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            int ranNum;
            string speech = e.Result.Text;

            //same in the DefaultCommands.txt attention EXACTEMENT PAREILLE
            if (speech == "Salut")
            {
                Samuel.SpeakAsync("Bonjour");
            }
            if (speech == "Comment vas tu")
            {
                Samuel.SpeakAsync("Je vais bien");
            }
            if (speech == "Dis l'heure")
            {

            }
            if (speech == "What day is it")
            {
                Samuel.SpeakAsync(DateTime.Now.ToString("D"));
            }
            if (speech == "Ouvre Youtube")
            {
                Samuel.SpeakAsync("J'ouvre youtube");
                System.Diagnostics.Process.Start("https://www.youtube.com/");

            }
            if (speech == "Ouvre Unity Hub")
            {
                Samuel.SpeakAsync("J'ouvre unity eub");
                System.Diagnostics.Process.Start(@"C:\\Program Files\\Unity Hub\\Unity Hub.exe\");

            }
            if (speech == "Open Krunker")
            {
                Samuel.SpeakAsync("J'ouvre kreunekeur");
                System.Diagnostics.Process.Start(@"C:\\Program Files\\Unity Hub\\Unity Hub.exe\");

            }

            if (speech == "Shut up")
            {
                Samuel.SpeakAsyncCancelAll();
                ranNum = rnd.Next(1);
                if (ranNum == 1)
                {
                    Samuel.SpeakAsync("Oui monsieur");
                }
            }
            if (speech == "Stop Listening")
            {
                Samuel.SpeakAsync("Si vous avez besoin de moi, demandez juste");
                _recognizer.RecognizeAsyncCancel();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
            }

            if (speech == "Show Commands")
            {
                string[] commands = (File.ReadAllLines(@"DefaultCommands.txt"));
                LstCommands.Items.Clear();
                LstCommands.SelectionMode = SelectionMode.None;
                LstCommands.Visible = true;
                foreach (string command in commands)
                {
                    LstCommands.Items.Add(command);
                }
            }
            if (speech == "Hide Commands")
            {
                LstCommands.Visible = false;
            }
        }
            private void _recognizer_SpeechRecognized(object sender, SpeechDetectedEventArgs e)
        {
            RecTimeOut = 0;
        }
        private void startlistening_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string speech = e.Result.Text;

            if (speech == "Wake up")
            {
                startlistening.RecognizeAsyncCancel();
                Samuel.SpeakAsync("Je suis présent");
                _recognizer.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        private void TmrSpeaking_Tick(object sender, EventArgs e)
        {
            if (RecTimeOut == 10)
            {
                _recognizer.RecognizeAsyncCancel();
            }
            else if (RecTimeOut == 11)
            {
                TmrSpeaking.Stop();
                startlistening.RecognizeAsync(RecognizeMode.Multiple);
                RecTimeOut = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                // BALEC FRERE Samuel.Volume = trackBar1.Value;
                Samuel.Speak(textBox1.Text);
            }
            else
            {
                MessageBox.Show("Please write smt first");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SpeechRecognitionEngine sr = new SpeechRecognitionEngine();
            Grammar words = new DictationGrammar();
            //either the 1st or the 2nd speechrecognizer engine(
            sr.LoadGrammar(words);
            try
            {
                textBox2.Text = "Listening Now...";
                sr.SetInputToDefaultAudioDevice();
                RecognitionResult result = sr.Recognize();
                textBox2.Clear();
                textBox2.Text = result.Text;
            }
            catch
            {
                textBox2.Text = "";
                MessageBox.Show("Mic not found");
            }
            finally
            {
                sr.UnloadAllGrammars();
            }
        }
    }
}

