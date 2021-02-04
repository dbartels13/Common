//using System;
//using System.Collections.Generic;
//using Newtonsoft.Json;
//using Sphyrnidae.Logging;
//using Sphyrnidae.Logging.LogInformation;
//using Sphyrnidae.Logging.Models;
//using Sphyrnidae.Utilities;
//using Sphyrnidae.Utilities.Extensions;

//namespace Sphyrnidae.Implementations.Common.Loggers
//{
//    // See:
//    // https://aws.amazon.com/developers/getting-started/net/
//    // https://docs.aws.amazon.com/sdkfornet/v3/apidocs/Index.html
//    // https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/CloudWatchLogs/TCloudWatchLogsClient.html
//    public class AwsLogger : BaseLogger
//    {
//        public override string Name => "AWS";
//        public override bool IncludeHigh => true;
//        public override bool IncludeMed => true;
//        public override bool IncludeLow => true;

//        private AmazonCloudWatchLogsClient Aws { get; }
//        private JsonSerializerSettings JsonSettings { get; }
//        private string LockSuffix { get; }
//        private string Token { get; set; }
//        private static string GroupName => "Sphyrnidae-Api";
//        private string StreamName => GroupName + "-" + LockSuffix;

//        /// <summary>
//        /// Creates a new Aws Logger
//        /// </summary>
//        /// <param name="client">The cloud watch client created as a singleton at startup</param>
//        /// <param name="json">The default serializer settings created as a singleton at startup</param>
//        /// <param name="lockSuffix">You should build this to be unique, recommend "Host-Application"</param>
//        public AwsLogger(AmazonCloudWatchLogsClient client, JsonSerializerSettings json, string lockSuffix)
//        {
//            Aws = client;
//            JsonSettings = json;
//            LockSuffix = lockSuffix;
//        }

//        protected override void DoInsert(LogInsert model, BaseLogInformation info, int maxLength)
//            => SendToAws(model.SerializeJson(JsonSettings));

//        protected override void DoUpdate(LogUpdate model, TimerBaseInformation info, int maxLength)
//            => SendToAws(model.SerializeJson(JsonSettings));

//        private async void SendToAws(string message)
//        {
//            // TODO: We should probably batch calls instead of sending each and every one every time
//            if (Token.IsDefault())
//            {
//                var logGroups = await Aws.DescribeLogGroupsAsync();
//                if (!logGroups.LogGroups.Any(x => x.LogGroupName.Equals(GroupName, StringComparison.InvariantCultureIgnoreCase)))
//                    await Aws.CreateLogGroupAsync(new CreateLogGroupRequest(GroupName));
//            }

//            // This has to be locked to allow the SequenceToken and Timestamp to be proper
//            // The lock/stream will be machine/application specific (machine - we can't lock across systems; application - there could be multiple applications on the same server)
//            NamedLocker.Lock("AwsLog-" + StreamName, num => WriteToStream(message));
//        }

//        private async void WriteToStream(string message)
//        {
//            if (Token.IsDefault())
//            {
//                // Per API/XML comments: This operation has a limit of five transactions per second, after which transactions are throttled
//                var streams = await Aws.DescribeLogStreamsAsync(new DescribeLogStreamsRequest(GroupName));
//                var stream = streams.LogStreams.FirstOrDefault(x => x.LogStreamName.Equals(StreamName, StringComparison.InvariantCultureIgnoreCase));
//                if (stream.IsDefault())
//                    await Aws.CreateLogStreamAsync(new CreateLogStreamRequest(GroupName, StreamName));
//                Token = stream?.UploadSequenceToken;
//            }

//            // TODO: The put requires the timestamp to be synchronous, and the sequence token to be accurate - don't know why? Can't they just accept a log statement??? This would SO much simplify things
//            /*
//            From: https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/CloudWatchLogs/TCloudWatchLogsClient.html

//            You must include the sequence token obtained from the response of the previous call.
//            An upload in a newly created log stream does not require a sequence token.
//            You can also get the sequence token using DescribeLogStreams.
//            If you call PutLogEvents twice within a narrow time period using the same value for sequenceToken, both calls may be successful, or one may be rejected.

//            The batch of events must satisfy the following constraints:
//                1) The maximum batch size is 1,048,576 bytes, and this size is calculated as the sum of all event messages in UTF-8, plus 26 bytes for each log event.
//                2) None of the log events in the batch can be more than 2 hours in the future.
//                3) None of the log events in the batch can be older than 14 days or older than the retention period of the log group.
//                4) The log events in the batch must be in chronological ordered by their timestamp. The timestamp is the time the event occurred, expressed as the number of milliseconds after Jan 1, 1970 00:00:00 UTC. (In AWS Tools for PowerShell and the AWS SDK for .NET, the timestamp is specified in .NET format: yyyy-mm-ddThh:mm:ss. For example, 2017-09-15T13:45:30.)
//                5) The maximum number of log events in a batch is 10,000.

//            A batch of log events in a single request cannot span more than 24 hours. Otherwise, the operation fails.             
//            */
//            var result = await Aws.PutLogEventsAsync(
//                new PutLogEventsRequest(
//                    GroupName,
//                    StreamName,
//                    new List<InputLogEvent>
//                    {
//                        new InputLogEvent
//                        {
//                            Message = message,
//                            Timestamp = DateTime.UtcNow
//                        }
//                    }
//                )
//                {
//                    SequenceToken = Token
//                });
//            Token = result.NextSequenceToken;
//        }
//    }
//}
