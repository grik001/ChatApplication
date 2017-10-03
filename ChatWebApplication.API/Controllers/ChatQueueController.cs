using ChatWebApplication.API.Models;
using Common.Helpers.IHelpers;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ChatWebApplication.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ChatQueueController : ApiController
    {
        IMessageQueueHelper _messageQueueHelper = null;
        IApplicationConfig _applicationConfig = null;

        public ChatQueueController(IMessageQueueHelper messageQueueHelper, IApplicationConfig applicationConfig)
        {
            this._messageQueueHelper = messageQueueHelper;
            this._applicationConfig = applicationConfig;
        }

        [HttpPost]
        public dynamic Post([FromBody]ChatQueuePostRequest request)
        {
            try
            {
                if (request.ChatQueueKey != Guid.Empty)
                {
                    QueueMetaData queueMeta = new QueueMetaData();
                    queueMeta.ClientID = request.ChatQueueKey;
                    queueMeta.Function = Common.Constants.MessageFunctionType.Start;

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

        [HttpDelete]
        public dynamic Delete(Guid id)
        {
            try
            {
                if (id != Guid.Empty)
                {
                    QueueMetaData queueMeta = new QueueMetaData();
                    queueMeta.ClientID = id;
                    queueMeta.Function = Common.Constants.MessageFunctionType.Stop;
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
    }
}
