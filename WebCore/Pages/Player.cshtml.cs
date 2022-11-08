using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebCore.Services;

namespace WebCore.Pages {
    public class PlayerModel : PageModel {

        private readonly VLCService VLC;

        public int currentTime = 0;
        public int currentLength = 0;
        public string currentTitle = "";

        public PlayerModel(VLCService vLC) {
            VLC = vLC;
        }

        public void OnGet() {
            currentTime = VLC.GetTime();
            currentLength = VLC.GetLength();
            currentTitle = VLC.GetTitle();
        }
    }
}
