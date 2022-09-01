using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.Extensions.Configuration;

class SpeechRecognizerTest
{
    private static string YourSubscriptionKey;
    private static string YourServiceRegion = "chinaeast2";
    private static IConfiguration _configuration;

    private static async Task Main1(string[] args)
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environmentName}.json", true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .Build();

        YourSubscriptionKey = _configuration["AzureCognitive:SpeechService:SubscriptionKey"];

        var speechConfig = SpeechConfig.FromSubscription(YourSubscriptionKey, YourServiceRegion);
        speechConfig.SpeechRecognitionLanguage = "zh-CN";

        //识别麦克风的输入
        using var audioConfig = AudioConfig.FromDefaultMicrophoneInput();
        //
        // using var audioConfig = AudioConfig.FromWavFileInput();

        using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

        Console.WriteLine("Speak into your microphone.");
        var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();
        OutputSpeechRecognitionResult(speechRecognitionResult);
    }

    private static void OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
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