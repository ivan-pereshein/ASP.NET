using System;
using System.ComponentModel.DataAnnotations;

namespace PromoCodeFactory.Core.Domain
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}