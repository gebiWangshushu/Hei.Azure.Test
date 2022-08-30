using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

class Program
{
    static string YourSubscriptionKey = "f83702bb4db7493fa9bea859e8d525ce";
    static string YourServiceRegion = "chinaeast2";

    async static Task Main(string[] args)
    {
        var speechConfig = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);
        speechConfig.SpeechRecognitionLanguage = "zh-CN";

        //识别麦克风的输入
        //using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        //
        using var audioConfig = AudioConfig.FromWavFileInput();

        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        Console.WriteLine("Speak into your microphone.");
        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        OutputSpeechRecognitionResult(speechRecognitionResult);
    }

    static void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
    {
        switch (speechRecognitionResult.Reason)
        {
            case ResultReason.RecognizedSpeech:
                Console.WriteLine($"RECOGNIZED: Text={speechRecognitionResult.Text}");
                break;

            case ResultReason.NoMatch:
                Console.WriteLine($"NOMATCH: Speech could not be recognized.");
                break;

            case ResultReason.Canceled:
                var cancellation = CancellationDetails.FromResult(speechRecognitionResult);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails={cancellation.ErrorDetails}");
                    Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                }
                break;
        }
    }
}