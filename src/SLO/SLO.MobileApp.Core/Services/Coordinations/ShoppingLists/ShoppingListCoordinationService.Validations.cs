using SLO.MobileApp.Core.Models.Coordinations.ShoppingLists.Exceptions;
using System;

namespace SLO.MobileApp.Core.Services.Coordinations.ShoppingLists;

internal partial class ShoppingListCoordinationService
{
    private void ValidateShoppingListId(Guid shoppingListId)
    {
        Validate(
            (Rule: Invalid(shoppingListId), Parameter: nameof(shoppingListId)));
    }

    private static dynamic Invalid(Guid id) =>
        new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required."
        };

    private static void Validate(params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidShoppingListCoordinationException =
            new InvalidShoppingListCoordinationException(
                exceptionMessage: "Invalid shopping list coordination error occurred, " +
                "fix the errors and try again please!");

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidShoppingListCoordinationException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }

        invalidShoppingListCoordinationException.ThrowIfContainsErrors();
    }
}
