using System;
using System.Collections.Generic;
using System.Text;

namespace MobieBotVK.BotTools.SessionControl.Commands
{
    public abstract class ACommand
    {
        public string CommandName { get; set; }
        public string AboutCommand { get; set; }
        public int RightValue { get; set; }
    }
}
