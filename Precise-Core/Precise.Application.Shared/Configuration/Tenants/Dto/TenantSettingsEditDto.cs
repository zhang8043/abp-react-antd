using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Runtime.Validation;
using Abp.Timing;
using Precise.Configuration.Host.Dto;

namespace Precise.Configuration.Tenants.Dto
{
    public class TenantSettingsEditDto
    {
        public GeneralSettingsEditDto General { get; set; }

        [Required]
        public TenantUserManagementSettingsEditDto UserManagement { get; set; }

        public EmailSettingsEditDto Email { get; set; }

        public LdapSettingsEditDto Ldap { get; set; }

        [Required]
        public SecuritySettingsEditDto Security { get; set; }

        public TenantBillingSettingsEditDto Billing { get; set; }


        /// <summary>
        /// This validation is done for single-tenant applications.
        /// Because, these settings can only be set by tenant in a single-tenant application.
        /// </summary>
        public void ValidateHostSettings()
        {
            var validationErrors = new List<ValidationResult>();
            if (Clock.SupportsMultipleTimezone && General == null)
            {
                validationErrors.Add(new ValidationResult("General settings can not be null", new[] { "General" }));
            }

            if (Email == null)
            {
                validationErrors.Add(new ValidationResult("Email settings can not be null", new[] { "Email" }));
            }

            if (validationErrors.Count > 0)
            {
                throw new AbpValidationException("Method arguments are not valid! See ValidationErrors for details.", validationErrors);
            }
        }
    }
}