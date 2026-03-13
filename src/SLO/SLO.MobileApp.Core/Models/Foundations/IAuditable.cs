using System;

namespace SLO.MobileApp.Core.Models.Foundations;

internal interface IAuditable
{
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
