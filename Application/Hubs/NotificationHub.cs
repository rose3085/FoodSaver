using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces.Food;
using Domain.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace Application.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly SharedDb _sharedDb;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFoodService _foodService;


       
        public NotificationHub(SharedDb sharedDb, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager,IFoodService foodService)
        {
            _sharedDb = sharedDb;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _foodService = foodService;
        }

        public override async Task OnConnectedAsync()
        {
            var userInfo = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var userId = userInfo.Id;
            var connectionId = Context.ConnectionId;
            var userConnection = new UserConnection()
            { 
                UserId = userId,
                ConnectionId = connectionId,
            };
           // _sharedDb.connections[Context.ConnectionId] = userConnection;
            _sharedDb.AddConnection(connectionId,userConnection);
           // return base.OnConnectedAsync();
        }

        public async Task SendNotification(string productId,string message)
        {

            var product = await _foodService.GetProductById(productId);
            var userId = product.Seller.Id;
            if (userId != null)

            {
                var userConnection = _sharedDb.GetByUserId(userId);
                var connectionId = userConnection.ConnectionId;
                if (connectionId != null)
                {
                    await Clients.Client(connectionId).SendAsync("SendNotification", message);
                }
            }
        }
    }
}
