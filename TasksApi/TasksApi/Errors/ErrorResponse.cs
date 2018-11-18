using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TasksApi.Errors
{
    /// <summary>
    /// Represents an error response
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// An HTTP status code value, without the textual description.
        /// Example values include: 400 (Bad Request), 401 (Unauthorized), 404 (Not Found), etc…
        /// </summary>
        [Required]
        public int Code { get; set; }

        /// <summary>
        /// Description of the error
        /// </summary>
        public string Message { get; set; }

        public List<ErrorDetail> Details { get; set; }
    }
}
