using System;

namespace SLO.MobileApp.Core.Models.Foundations.ShoppingLists;

public class ShoppingList : IAuditable
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
