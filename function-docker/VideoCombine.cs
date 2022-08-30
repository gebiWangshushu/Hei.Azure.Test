using CliWrap;
using CliWrap.Buffered;
using FFMpegCore;
using System.IO;
using System.Threading.Tasks;

namespace function_docker
{
    public class VideoCombine
    {
        public async Task Execute(string combineName)
        {
            var tempPath = Path.Combine(Keys.LocalPath, combineName);
            Directory.CreateDirectory(tempPath);
            var listTxt = $"{tempPath}/video_list.txt";

            for (int i = 1; i <= 8; i++)
            {
                FFMpegArguments.FromFileInput($"{Keys.LocalPath}/C000{i}.MP4").OutputToFile($"{tempPath}/C000{i}.ts", true).ProcessSynchronously();//ffmpeg -i 1.mp4 -c:v copy 1.ts
                File.AppendAllText(listTxt, $"file '{tempPath}/C000{i}.ts'\n");
            }

            await Cli.Wrap("ffmpeg").WithArguments($"-f concat -safe 0 -i {listTxt} -c copy {combineName}_output.mp4 -y").WithWorkingDirectory(Keys.LocalPath).ExecuteBufferedAsync();
            await Cli.Wrap("ffmpeg").WithArguments($"-y -i {combineName}_output.mp4 -i C0000.jpg -map 0 -map 1 -c copy -disposition:v:1 attached_pic {combineName}_fanal.mp4 -y").WithWorkingDirectory(Keys.LocalPath).ExecuteBufferedAsync();

            Directory.Delete(tempPath, true);
        }
    }
}