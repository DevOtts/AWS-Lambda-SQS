using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Newtonsoft.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ConsumeMessage
{
    public class Function
    {
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {

        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ConsumeSQS(SQSEvent evnt, ILambdaContext context)
        {
            //For this consumer get the SQS message, you have to Add the SQS as a Trigger on this Lambda
            if (evnt.Records.Count > 1)
                throw new InvalidOperationException("Only one msg can be handle at time");

            var message = evnt.Records.FirstOrDefault();
            await ProcessMessageAsync(message, context);
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<MyObject>(message.Body);
                Console.WriteLine(obj.Data);
                context.Logger.LogLine($"Processed message {message.Body}");
                context.Logger.LogLine($"Processed Status: {obj.Success}");
                context.Logger.LogLine($"Processed Data: {obj.Data}");
                context.Logger.LogLine($"Processed Log: {obj.Log}");
            }
            catch (Exception ex)
            {
                context.Logger.LogLine($"Processed ERROR: {ex.Message}");
            }
           

            // TODO: Do interesting work based on the new message
            await Task.CompletedTask;
        }
    }

    public class MyObject {
        public bool Success { get; set; }
        public string Data { get; set; }
        public string Log { get; set; }
    }
}
