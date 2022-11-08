using System.Diagnostics;

namespace WebCore.Services {

#if !DEBUG

    public class VLCService {

        Process VLCProcess;
        StreamWriter standardInput;
        StreamReader standardOutput;


        public VLCService() {

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false; //required to redirect standart input/output

            // redirects on your choice
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;

            startInfo.FileName = "vlc";
            startInfo.Arguments = "-I rc --fullscreen";

            VLCProcess = new Process();
            VLCProcess.StartInfo = startInfo;
            VLCProcess.Start();

            standardInput = VLCProcess.StandardInput;
            standardOutput = VLCProcess.StandardOutput;

            standardOutput.ReadLine();
            standardOutput.ReadLine();
            standardOutput.Read();
            standardOutput.Read();

        }

        public void Play(string path) {
            Write($"add {path}");
        }

        public void Write(string text) {
            standardInput.WriteLine(text);
            standardOutput.Read();
            standardOutput.Read();
        }

        public bool IsPlaying() {
            Write("is_playing");
            string? input = standardOutput.ReadLine();
            return (input ?? "0") != "0" ? true : false;
        }

        public int GetTime() {
            if (IsPlaying() == false) return 0;
            Write("get_time");
            string? input = standardOutput.ReadLine();
            if (string.IsNullOrEmpty(input)) return 0;
            else return Convert.ToInt32(input);
        }

        public int GetLength() {
            if (IsPlaying() == false) return 0;
            Write("get_length");
            string? input = standardOutput.ReadLine();
            if (string.IsNullOrEmpty(input)) return 0;
            else return Convert.ToInt32(input);
        }

        public string GetTitle() {
            if (IsPlaying() == false) return "None";
            Write("get_title");
            string? input = standardOutput.ReadLine();
            return input ?? "title_error";
        }

        public void Skip(int amount) {
            int curTime = GetTime();
            int newTime = curTime + amount;
            Write($"seek {newTime}");
        }

    }

#else

    public class VLCService {

        public void Write(string text) {
            Console.WriteLine(text);
        }

        public int GetTime() {
            return 42;
        }

    }

#endif

}
