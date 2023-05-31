using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Condom.Domain.Global.TrackLog;

namespace Condom.Domain.Global
{
    public class Tracker
    {
        [JsonPropertyName("logs")]
        public List<TrackLog> Logs { get; set; } = new List<TrackLog>();

        public void AddLog(SignInResult result)
        {
            if (result.Succeeded)
            {
                AddLog(MessageTypeEnum.Success, "Acesso liberado!");
            } else if (result.IsLockedOut)
            {
                AddLog(MessageTypeEnum.Error, "Usuário bloqueado");
            } else if (result.IsNotAllowed)
            {
                AddLog(MessageTypeEnum.Error, "Usuário ou senha incorreto!");
            }
            else
            {
                AddLog(MessageTypeEnum.Error, "Usuário ou senha incorreto!");
            }
        }

        public void AddLog(IdentityResult result)
        {
            if (result.Succeeded)
            {
                AddLog(MessageTypeEnum.Success, "Ação realizada com sucesso!");
            }
            else
            {
                AddLog(MessageTypeEnum.Error, "Não foi possivel realizar essa operação!");
            }
        }
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

        [JsonPropertyName("hasError")]
        public bool Error { get => HasError(); }

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
        [JsonPropertyName("message")]
        public string Message { get; private set; }
        [JsonPropertyName("type")]
        public string MessageType { get => Type.ToString(); }

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
