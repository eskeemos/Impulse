namespace Impulse.Shared.Templates
{
    public class Exchange
    { 
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public bool ApiKeyProvided
            => !string.IsNullOrWhiteSpace(ApiKey) && !string.IsNullOrWhiteSpace(ApiSecret);
    }
}