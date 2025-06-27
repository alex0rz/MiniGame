using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Manager
{
    public class MessageManager
    {
        public IReadOnlyCollection<string> Message => _messages;
        private readonly List<string> _messages = new List<string>();
        internal void LogMessage(string msg)
        {
            if(!string.IsNullOrWhiteSpace(msg))
                _messages.Add(msg);
        }
    }
}
