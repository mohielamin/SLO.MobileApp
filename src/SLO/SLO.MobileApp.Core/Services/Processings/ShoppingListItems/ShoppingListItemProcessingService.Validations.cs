using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Processings.ShoppingListItems.Exceptions;
using System;

namespace SLO.MobileApp.Core.Services.Processings.ShoppingListItems;

internal partial class ShoppingListItemProcessingService
{
    private void ValidateShoppingListItemOnUpsert(
        ShoppingListItem shoppingListItem)
    {
        ValidateShoppingListItem(shoppingListItem);

        Validate(
            (Rule: Invalid(shoppingListItem.Id),
            Parameter: nameof(ShoppingListItem.Id)));
    }

    private void ValidateShoppingListItemOnRetrieveAllByShoppingListId(
        Guid shoppingListId)
    {
        Validate(
            (Rule: Invalid(shoppingListId), Parameter: nameof(shoppingListId)));
    }

    private void ValidateShoppingListItem(
        ShoppingListItem shoppingListItem)
    {
        if (shoppingListItem is null)
        {
            throw new NullShoppingListItemProcessingException(
                exceptionMessage: "Shopping list item is null.");
        }
    }

    private static dynamic Invalid(Guid id) =>
        new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required."
        };

    private void Validate(params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidShoppingListItemProcessingException =
            new InvalidShoppingListItemProcessingException(
                exceptionMessage: "Invalid shopping list item processing error occurred, " +
                "fix the errors and try again please!");

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidShoppingListItemProcessingException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }

        invalidShoppingListItemProcessingException.ThrowIfContainsErrors();
    }
}
