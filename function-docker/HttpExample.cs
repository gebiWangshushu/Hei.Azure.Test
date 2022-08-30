using CliWrap;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace function_docker
{
    public static class HttpExample
    {
        [FunctionName("HttpExample")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string cusmsg = $"version:v1.0.1-ffmpeg installed 2022.8.22 {Environment.CurrentDirectory}";
            string responseMessage = $"Hello, {name ?? "nobody"}. This HTTP triggered function executed successfully." + cusmsg;

            try
            {
                var cmd = await Cli.Wrap("ffmpeg").WithArguments("-version").ExecuteAsync();
                //File.WriteAllText(Path.Combine(Keys.LocalPath, "111111.txt"), DateTimeOffset.Now.ToString());

                //FFMpeg.JoinImageSequence($"{Keys.LocalPath}/1.mp4", frameRate: 1, ImageInfo.FromPath($"{Keys.LocalPath}/1.jpg"), ImageInfo.FromPath($"{Keys.LocalPath}/2.jpg"), ImageInfo.FromPath($"{Keys.LocalPath}/3.jpg"));

                ////先把mp4转成ts
                //var result = FFMpegArguments.FromFileInput($"{Keys.LocalPath}/1.mp4").OutputToFile($"{Keys.LocalPath}/1.ts", true).ProcessSynchronously(); //ffmpeg -i 1.mp4 -c:v copy 1.ts
                //FFMpegArguments.FromFileInput($"{Keys.LocalPath}/2.mp4").OutputToFile($"{Keys.LocalPath}/2.ts", true).ProcessSynchronously();
                //FFMpegArguments.FromFileInput($"{Keys.LocalPath}/3.mp4").OutputToFile($"{Keys.LocalPath}/3.ts", true).ProcessSynchronously();

                ////拼接ts视频会比较平顺(不然有视频卡顿的问题)
                //cmd = await Cli.Wrap("ffmpeg").WithArguments("-f concat -safe 0 -i video_list_ts.txt -c copy combine.mp4 -y").WithWorkingDirectory(Keys.LocalPath).ExecuteBufferedAsync();

                ////修改封面
                //cmd = await Cli.Wrap("ffmpeg").WithArguments("-y -i combine.mp4 -i 1.jpg -map 0 -map 1 -c copy -disposition:v:1 attached_pic output_fanal.mp4 -y").WithWorkingDirectory(Keys.LocalPath).ExecuteBufferedAsync();
            }
            catch (System.Exception ex)
            {
                log.LogError(ex, "function-docker HttpExample exception");
                throw ex;
            }

            return new OkObjectResult(responseMessage);
        }
    }
}