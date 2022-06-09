using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Sender
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> SendMessage(string input, ILambdaContext context)
        {
            var json = JsonConvert.SerializeObject(new { Success = true, Data = input, Log = context.LogGroupName });
            var client = new AmazonSQSClient(RegionEndpoint.USEast1);
            var request = new SendMessageRequest
            {
                //You have to create a SQS in AWS and include the URL here
                //https://docs.aws.amazon.com/AWSSimpleQueueService/latest/SQSDeveloperGuide/sqs-configure-create-queue.html

                QueueUrl = "https://sqs.us-east-1.amazonaws.com/430525806551/FirstQueueOtt",
                MessageBody = json
            };

            await client.SendMessageAsync(request);
            return input;
        }
    }
}