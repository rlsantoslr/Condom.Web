using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Condom.Domain.Global.TrackLog;

namespace Condom.Domain.Global
{
    public class Tracker
    {
        public List<TrackLog> Logs { get; set; } = new List<TrackLog>();

        public void AddLog(MessageTypeEnum messageTypeEnum, string message)
        {
            Logs.Add(new TrackLog(messageTypeEnum, message));
        }
        public void Merge(Tracker tracker)
        {
            if (tracker == null) return;

            foreach(var log in tracker.Logs)
            {
                Logs.Add(log);
            }
        }

        public bool HasError()
        {
            return Logs.Exists(x => x.GetTypeEnum() == MessageTypeEnum.Error);
        }

        public void Reset()
        {
            Logs.Clear();
        }
    }

    public class TrackLog
    {
        public TrackLog(MessageTypeEnum type, string message)
        {
            Type = type;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        protected MessageTypeEnum Type { get; set; }

        protected string Message { get; set; }

        public MessageTypeEnum GetTypeEnum() => Type;

        public string GetMessage() => Message;  
    }

    public enum MessageTypeEnum
    {
        None = ' ',
        Error = 'E',
        Info = 'I',
        Warning = 'W',
        Success = 'S'
    }
}
