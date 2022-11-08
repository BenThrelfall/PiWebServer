using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using WebCore.Services;


namespace WebCore.Pages {

    public class IndexModel : PageModel {

#if !DEBUG
        const string mediaFolder = "/home/ownerpi/media";
#else
        const string mediaFolder = "C:/Users/Owner/Videos";
#endif

        private readonly ILogger<IndexModel> _logger;
        private readonly VLCService VLC;

        public float usedStorage;
        public float totalStorage;

        public string currentTitle = "";
        public int currentTime = 0;
        public int currentLength = 0;

        public List<string> mediaFiles;
        public List<string> mediaFileNames;

        public IndexModel(ILogger<IndexModel> logger, VLCService vLC) {
            mediaFiles = new List<string>();
            mediaFileNames = new List<string>();
            _logger = logger;
            VLC = vLC;
        }

        public void OnGet() {

            GetPlayerInfo();

            GetStorageInfo();

            GetMediaFileInfo();

        }

        private void GetPlayerInfo() {
            currentTime = VLC.GetTime();
            currentLength = VLC.GetLength();
            currentTitle = VLC.GetTitle();
        }

        public void OnPost() {
            HandleInput();
            GetStorageInfo();
            GetMediaFileInfo();
            GetPlayerInfo();
        }

        private void GetMediaFileInfo() {
            mediaFiles.AddRange(Directory.GetFiles(mediaFolder).OrderBy(x => x, StringComparer.Ordinal));
            mediaFileNames.AddRange(mediaFiles.Select(x => Path.GetFileName(x)));
        }

        private void GetStorageInfo() {
            long totalStorageBytes = DriveInfo.GetDrives().First().TotalSize;
            long usedStorageBytes = totalStorageBytes - DriveInfo.GetDrives().First().AvailableFreeSpace;

            usedStorage = usedStorageBytes / (1024 * 1024 * 1024);
            totalStorage = totalStorageBytes / (1024 * 1024 * 1024);
        }

        private void HandleInput() {
            string playValue = HttpContext.Request.Form["play"];

            if (string.IsNullOrEmpty(playValue) == false) {
                VLC.Play(playValue);
            }

            string cmdValue = HttpContext.Request.Form["cmd"];

            if (string.IsNullOrEmpty(cmdValue) == false) {
                VLC.Write(cmdValue);
            }

            string skipValue = HttpContext.Request.Form["skip"];

            if (string.IsNullOrEmpty(skipValue) == false && int.TryParse(skipValue, out int result)) {
                VLC.Skip(result);
            }

        }

    }
}