using System;

namespace Aya.Config
{
    public class BotConfig
    {
        public string Token { get; set; } = "your-token";
        public bool IsInitialized() => !String.IsNullOrEmpty(Token) && Token != "your-token";
    }
}

