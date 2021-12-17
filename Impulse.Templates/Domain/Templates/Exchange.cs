namespace Impulse.Shared.Domain.Templates
{
    public class Exchange
    { 
        /// <summary>
        /// Exchange platform naem
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Api key from platform
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Api secret key from platform
        /// </summary>
        public string ApiSecret { get; set; }

        public bool IsInTestMode
            => string.IsNullOrWhiteSpace(ApiKey) || string.IsNullOrWhiteSpace(ApiSecret);
    }
}