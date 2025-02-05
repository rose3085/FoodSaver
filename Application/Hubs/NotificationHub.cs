
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Hubs.InMemoryDb;
using Application.Hubs.Models;
using Application.Interfaces.Food;
using Domain.Entities.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Hubs
{
   
    public class NotificationHub : Hub
    {
        private readonly UserConnectionDb _sharedDb;
        private readonly NotificationDb _notificationDb;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFoodService _foodService;


       
        public NotificationHub(UserConnectionDb sharedDb,NotificationDb notificationDb, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager,IFoodService foodService)
        {
            _sharedDb = sharedDb;
            _notificationDb = notificationDb;
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
           
            _sharedDb.AddConnection(userId,userConnection);

            //var pendingNotification = await CheckPendingNotification(userId);
            //if (pendingNotification != null)
            //{
            //   var result = await SendPendingNotification(connectionId,pendingNotification.Message);
            //    if (result == true)
            //    {
            //        _notificationDb.RemoveNotification(userId);
            //    }
            //}



             var sendNotification =  await SendPendingNotification(userId , connectionId);
            if (sendNotification == true)
            {

                _notificationDb.RemoveNotification(userId);
            }
          
        }
        //public async override Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var userInfo = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
        //    var userId = userInfo.Id;
        //    string connectionId = Context.ConnectionId;
        //    var connection = _sharedDb.GetConnection(userId);
        //    if (connection != null)
        //    {
        //       _sharedDb.RemoveConnection(userId);

        //    }


        //}

        //public async Task<NotificationDto> CheckPendingNotification(string userId)
        //{
        //    try {

        //        var notification = _notificationDb.GetByUserId(userId);
        //        if (notification != null)
        //        {
        //            return notification;
        //        }
        //        else { return null; }
        //    }
        //    catch { return null; }

        //}
        //public async Task<bool> SendPendingNotification(string connectionId,string message)
        //{
        //    if (connectionId != null && message != null)
        //    {
        //        await Clients.Client(connectionId).SendAsync("ReceivePendingMessage", message);
        //        return true;
        //    }
        //    else {
        //        return false;
        //    }
        //}





        public async Task<bool> SendPendingNotification(string userId,string connectionId)
        {

            if (userId != null)
            {

            var notifications =await _notificationDb.GetByUserId(userId);
                if (notifications != null && notifications.Any())
                {
                    List<string> message = new List<string>();

                    foreach (var notification in notifications)
                    {
                        //string message = notification.Message;
                        message.Add(notification.Message);
                        await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                       
                      
                    }
                    return true;

                }
                else { return false; }
            }
            else { return false; }

        }



        public async Task SendNotification(string sellerId,string message)
        {
            var userInfo = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            var buyerId = userInfo.Id;

            //var connection = _sharedDb.GetConnection(sellerId);
           
            //if (connection == null)
            //{


                if (sellerId != null && message != null && buyerId != null)
                {
                    var notification = new NotificationDto()
                    {
                        BuyerId = buyerId,
                        SellerId = sellerId,
                        Message = message
                    };

                    _notificationDb.AddNotification(sellerId, notification);
                }
            //}
            //else 
            //{
            //    var connectionId = connection.ConnectionId;
            //    await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            //}






            //var userConnection =  _sharedDb.GetByUserId(sellerId);
         
            //    var connectionId = userConnection.ConnectionId;
            //    if (connectionId != null)
            //    {
            //        await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            //    }
            

        }
    }
}
