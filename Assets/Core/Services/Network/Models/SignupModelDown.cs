using Newtonsoft.Json;
using System;


namespace StarterCore.Core.Services.Network.Models
{
    [Serializable]
    public class SignupModelDown
    {
        [JsonProperty("code")]
        public string Code;
    }
}
