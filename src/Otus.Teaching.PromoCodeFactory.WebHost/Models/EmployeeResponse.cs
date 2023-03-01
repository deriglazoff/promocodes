using System;
using System.Collections.Generic;
using Otus.Teaching.PromoCodeFactory.Core.Domain.Administration;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public struct EmployeeDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public List<RoleItemDto> Roles { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}