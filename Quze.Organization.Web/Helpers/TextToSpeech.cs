using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;

namespace Quze.Models.Logic
{
    class TextToSpeech
    {
        public string getVoiceFile(string text="example") {
            // Instantiate a client
            //GoogleCredential credential=GoogleCredential.FromAccessToken("",);
            GoogleCredential creds = GoogleCredential.GetApplicationDefault();
            TextToSpeechClient client = TextToSpeechClient.Create();

            // Set the text input to be synthesized.
            SynthesisInput input = new SynthesisInput
            {
                Text = text
            };

            // Build the voice request, select the language code ("en-US"),
            // and the SSML voice gender ("neutral").
            VoiceSelectionParams voice = new VoiceSelectionParams
            {
                LanguageCode = "en-US",
                SsmlGender = SsmlVoiceGender.Neutral
            };

            // Select the type of audio file you want returned.
            AudioConfig config = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3
            };

            // Perform the Text-to-Speech request, passing the text input
            // with the selected voice parameters and audio file type
            var response = client.SynthesizeSpeech(
            
                  input,
                 voice,
                 config
            );

            // Write the binary AudioContent of the response to an MP3 file.
            using (Stream output = File.Create("sample.mp3"))
            {
                File.WriteAllBytes("somefile.mp3", response.AudioContent);
                Console.WriteLine($"Audio content written to file 'sample.mp3'");
            }
            return "somefile.mp3";
        }
    }
}
