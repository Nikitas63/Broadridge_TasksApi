using Newtonsoft.Json;

namespace TasksApi.Filters.Models
{
    [JsonObject]
    public class ValidationCustomErrorAsMessage
    {
        /// <summary>
        /// Format version (default value is the most recent)
        /// </summary>
        public int Version { get; set; } = 1;

        public int? StatusCode { get; set; }

        public string Message { get; set; }
    }
}
