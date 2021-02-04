//using System.Text;
//using Microsoft.AspNetCore.Mvc.Diagnostics;
//using Sphyrnidae.Logging;
//using Sphyrnidae.Logging.LogInformation;
//using Sphyrnidae.Logging.Models;
//using Sphyrnidae.Utilities;
//using Sphyrnidae.Utilities.Interfaces;

//namespace Sphyrnidae.Implementations.Common.Loggers
//{
//    public class AzureLogger : BaseLogger
//    {
//        public override string Name => "Azure";
//        public override bool IncludeHigh => true;
//        public override bool IncludeMed => true;
//        public override bool IncludeLow => true;

//        private static EventHubClient _eventHubClient;
//        private IEnvironmentSettings Env { get; }
//        private ISerializationSettings Json { get; }
//        public AzureLogger(IEnvironmentSettings env, ISerializationSettings json)
//        {
//            Env = env;
//            Json = json;
//        }

//        protected override void DoInsert(LogInsert model, BaseLogInformation info, int maxLength)
//            => SendToAzure(model);

//        protected override void DoUpdate(LogUpdate model, TimerBaseInformation info, int maxLength)
//            => SendToAzure(model);

//        private void SendToAzure(object model)
//        {
//            if (_eventHubClient.IsDefault())
//            {
//                var cnn = SettingsEnvironmental.Get(Env, "Azure:EventHub:Cnn");
//                var name = SettingsEnvironmental.Get(Env, "Azure:EventHub:Name");
//                _eventHubClient = EventHubClient.CreateFromConnectionString(cnn, name);
//            }
//            _eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(model.SerializeJson(Json.JsonSettings))));
//        }

//    }
//}
