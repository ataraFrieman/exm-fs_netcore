using System;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.TextToSpeech.V1;

namespace Quze.API.IVR
{
    class GoogleTextToSpeech
    {
        /// <summary>
        /// Set text to speech audio file
        /// </summary>
        /// <param name="messageContent">message to speech</param>
        /// <returns>file name</returns>
        public string FileFromText(string messageContent)
        {
            var fileName = getVoiceFile(messageContent);
            return fileName;
        }
        public string getVoiceFile(string text = "example")
        {
            // Instantiate a client
            //GoogleCredential credential=GoogleCredential.FromAccessToken("",);
            GoogleCredential creds = GoogleCredential.FromFile(@"C:\GoogleAuth\google.json");
            TextToSpeechClient client = TextToSpeechClient.Create(creds);



            // Set the text input to be synthesized.
            SynthesisInput input = new SynthesisInput
            {
                Text = text
            };

            // Build the voice request, select the language code ("en -US"),
            // and the SSML voice gender ("neutral").
            VoiceSelectionParams voice = new VoiceSelectionParams
            {
                LanguageCode = "en-US",
                SsmlGender = SsmlVoiceGender.Female
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
            var rand = new Random();
            var fileName = @"C:\GoogleAuth\" +rand.Next(1000).ToString() + ".mp3";
            File.WriteAllBytes(fileName, response.AudioContent);
            return fileName;
        }
    }
}
