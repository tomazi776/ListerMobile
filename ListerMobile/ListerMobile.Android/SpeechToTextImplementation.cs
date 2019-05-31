using Android.App;
using Android.Content;
using Android.Speech;
using ListerMobile.Droid;
using ListerMobile.Services;
using Plugin.CurrentActivity;
using System;
using System.Globalization;

[assembly: Xamarin.Forms.Dependency(typeof(SpeechToTextImplementation))]
namespace ListerMobile.Droid
{

    public class SpeechToTextImplementation : ISpeechToText
    {
        private readonly int VOICE = 10;
        private Activity activity;
        public SpeechToTextImplementation()
        {

            activity = CrossCurrentActivity.Current.Activity;

        }

        public void StartSpeechToText()
        {
            StartRecordingAndRecognizing();
        }

        private void StartRecordingAndRecognizing()
        {
            string rec = global::Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec == "android.hardware.microphone")
            {
                var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                //voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                SetPromptMessage(voiceIntent);
                //voiceIntent.PutExtra(RecognizerIntent.ExtraOnlyReturnLanguagePreference, "pl-PL");

                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 2500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, "pl-PL");
                activity.StartActivityForResult(voiceIntent, VOICE);

            }
            else
            {
                throw new Exception("No mic found man, I'm sorry dude.");
            }
        }

        private void SetPromptMessage(Intent intent)
        {
            if (CultureInfo.CurrentCulture.Name == "pl-PL")
            {
                intent.PutExtra(RecognizerIntent.ExtraPrompt, "Mów teraz zią");

            }

            else
            {
                intent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now dumbass!");

            }
        }

        public void StopSpeechToText()
        {

        }


    }
}