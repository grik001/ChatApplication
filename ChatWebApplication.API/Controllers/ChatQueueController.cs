using ChatWebApplication.API.Models;
using Common.Helpers.IHelpers;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatWebApplication.API.Controllers
{
    public class ChatQueueController : ApiController
    {
        IMessageQueueHelper _messageQueueHelper = null;
        IApplicationConfig _applicationConfig = null;

        public ChatQueueController(IMessageQueueHelper messageQueueHelper, IApplicationConfig applicationConfig)
        {
            this._messageQueueHelper = messageQueueHelper;
            this._applicationConfig = applicationConfig;
        }

        [HttpGet]
        public dynamic Get(Guid? key)
        {
            try
            {
                if (key != Guid.Empty)
                {

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return BadRequest();
        }

        [HttpPost]
        public dynamic Post([FromBody]ChatQueuePostRequest request)
        {
            try
            {
                if (request.ChatQueueKey != Guid.Empty)
                {
                    QueueMetaData queueMeta = new QueueMetaData();
                    queueMeta.ChatEntryTime = DateTime.UtcNow;
                    queueMeta.CurrentAgent = null;
                    queueMeta.ExpiryTime = DateTime.UtcNow.AddMinutes(30);
                    queueMeta.IsActive = false;
                    queueMeta.ClientID = request.ChatQueueKey;

                    _messageQueueHelper.PushMessage<QueueMetaData>(_applicationConfig, queueMeta);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return BadRequest();
        }

        [HttpPut]
        public dynamic Put(Guid key)
        {
            try
            {
                if (key != Guid.Empty)
                {

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return BadRequest();
        }

        [HttpDelete]
        public dynamic Delete(Guid key)
        {
            try
            {
                if (key != Guid.Empty)
                {
                  
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }

            return BadRequest();
        }
    }
}
