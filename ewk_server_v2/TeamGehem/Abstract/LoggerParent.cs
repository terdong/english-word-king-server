using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using EWK_Server.TeamGehem.Manager;

namespace EWK_Server.TeamGehem.Abstract
{
    public interface LoggerParent
    {
        Log_Collection Log_Collection_ { get; }
        LogLevel Log_Level_ { get; }
    }
}
