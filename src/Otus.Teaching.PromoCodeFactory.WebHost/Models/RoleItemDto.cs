using System;

namespace Otus.Teaching.PromoCodeFactory.WebHost.Models
{
    public struct RoleItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }
    }
}