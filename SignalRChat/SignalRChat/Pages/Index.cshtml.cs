using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Hubs;

namespace SignalRChat.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHubContext<ChatHub> chatHub;

        public IndexModel(ILogger<IndexModel> logger
            ,IHubContext<ChatHub> chatHub)
        {
            _logger = logger;
            this.chatHub = chatHub;
        }

        public void OnGet()
        {

        }

        public async void OnGetSend()
        {
            await chatHub.Clients.All.SendAsync("ReceiveMessage", "server", "hellow, clients!!");
        }
    }
}