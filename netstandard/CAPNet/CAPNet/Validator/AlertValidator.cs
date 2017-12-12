using System.Collections.Generic;
using System.Linq;
using CAPNet.Models;

namespace CAPNet
{
    /// <summary>
    /// Validates an alert.
    /// </summary>
    public class AlertValidator
    {
        private readonly Alert alert;

        /// <summary>
        /// Initializes a new instance of the <see cref="AlertValidator"/> class.
        /// </summary>
        /// <param name="alert">The alert to validate.</param>
        public AlertValidator(Alert alert)
        {
            this.alert = alert;
        }

        /// <summary>
        /// Gets a value indicating whether the alert is valid.
        /// </summary>
        public bool IsValid => !Errors.Any();

        /// <summary>
        /// Gets the list of validation errors.
        /// </summary>
        public IEnumerable<Error> Errors => alert.GetErrorsFromAllEntityValidators();
    }
}