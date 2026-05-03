using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;
using System;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;

internal partial class ShoppingListItemService
{
    private void ValidateShoppingListItemOnAdd(
        ShoppingListItem shoppingListItem)
    {
        ValidateShoppingListItem(shoppingListItem);

        Validate(
            (Rule: Invalid(shoppingListItem.Id),
            Parameter: nameof(ShoppingListItem.Id)),

            (Rule: Invalid(shoppingListItem.ShoppingListId),
            Parameter: nameof(ShoppingListItem.ShoppingListId)),

            (Rule: Invalid(shoppingListItem.Name),
            Parameter: nameof(ShoppingListItem.Name)),

            (Rule: Invalid(shoppingListItem.CreatedBy),
            Parameter: nameof(ShoppingListItem.CreatedBy)),

            (Rule: Invalid(shoppingListItem.UpdatedBy),
            Parameter: nameof(ShoppingListItem.UpdatedBy)),

            (Rule: Invalid(shoppingListItem.CreatedAt),
            Parameter: nameof(ShoppingListItem.CreatedAt)),

            (Rule: Invalid(shoppingListItem.UpdatedAt),
            Parameter: nameof(ShoppingListItem.UpdatedAt)));
    }

    private void ValidateShoppingListItem(
        ShoppingListItem shoppingListItem)
    {
        if (shoppingListItem is null)
        {
            throw new NullShoppingListItemException(
                exceptionMessage: "Shopping list item is null.");
        }
    }

    private dynamic Invalid(Guid id) =>
        new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required."
        };

    private dynamic Invalid(DateTimeOffset dateTime) =>
        new
        {
            Condition = dateTime == default,
            Message = "Date is required."
        };

    private dynamic Invalid(string text) =>
        new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required."
        };

    private void Validate(params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidShoppingListItemException =
            new InvalidShoppingListItemException(
                exceptionMessage: "Invalid shopping list item error occurred, " +
                "fix the errors and try again please!");

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidShoppingListItemException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }

        invalidShoppingListItemException.ThrowIfContainsErrors();
    }
}
