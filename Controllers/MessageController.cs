﻿using AiursoftBase;
using AiursoftBase.Attributes;
using AiursoftBase.Models;
using AiursoftBase.Models.MessageQueue;
using AiursoftBase.Models.MessageQueue.MessageAddressModels;
using AiursoftBase.Services;
using AiursoftBase.Services.ToAPIServer;
using MessageQueue.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Controllers
{
    [AiurRequireHttps]
    [AiurExceptionHandler]
    public class MessageController : AiurController
    {
        private MessageQueueDbContext _dbContext;
        public MessageController(MessageQueueDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> PushMessage(PushMessageAddressModel model)
        {
            //Ensure app is created
            var app = await ApiService.ValidateAccessTokenAsync(model.AccessToken);
            var appLocal = await _dbContext.Apps.SingleOrDefaultAsync(t => t.Id == app.AppId);
            if (appLocal == null)
            {
                appLocal = new MessageQueueApp
                {
                    Id = app.AppId,
                    Channels = new List<Channel>()
                };
                _dbContext.Apps.Add(appLocal);
                await _dbContext.SaveChangesAsync();
            }
            //Ensure channel
            var channel = await _dbContext.Channels.SingleOrDefaultAsync(t => t.Id == model.ChannelId);
            if (channel == null)
            {
                return Json(new AiurProtocal
                {
                    code = ErrorType.NotFound,
                    message = "We can not find your channel!"
                });
            }
            if (channel.AppId != app.AppId)
            {
                return Json(new AiurProtocal
                {
                    code = ErrorType.Unauthorized,
                    message = "The channel you wanna create message is not your app's channel!"
                });
            }
            //Create Message
            var message = new Message
            {
                Id = Startup.MessageIdCounter.GetUniqueNo,
                ChannelId = channel.Id,
                Content = model.MessageContent
            };
            MessageQueueMemory.Messages.Add(message);
            return Json(new AiurProtocal
            {
                code = ErrorType.Success,
                message = $"You have successfully created a message at channel:{channel.Id}!"
            });
        }
    }
}
