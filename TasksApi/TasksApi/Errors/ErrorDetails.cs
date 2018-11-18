using System.ComponentModel.DataAnnotations;

namespace TasksApi.Errors
{
    /// <summary>
    /// Represents an error detail
    /// </summary>
    public class ErrorDetail
    {
        /// <summary>
        /// The error’s source
        /// </summary>
        [Required]
        public string Source { get; set; }

        /// <summary>
        /// Description of the error
        /// </summary>
        [Required]
        public string Message { get; set; }
    }
}
