using SLO.MobileApp.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Models.Foundations.ShoppingItems.Exceptions;
using System;

namespace SLO.MobileApp.Services.Foundations.ShoppingItems;

internal sealed partial class ShoppingItemService
{
    private static void ValidateShoppingItemOnAdd(
        ShoppingItem shoppingItem)
    {
        ValidateShoppingItem(shoppingItem);

        Validate(
            (Rule: Invalid(shoppingItem.Id),
            Parameter: nameof(ShoppingItem.Id)),

            (Rule: Invalid(shoppingItem.CreatedBy),
            Parameter: nameof(ShoppingItem.CreatedBy)),

            (Rule: Invalid(shoppingItem.UpdatedBy),
            Parameter: nameof(ShoppingItem.UpdatedBy)),

            (Rule: Invalid(shoppingItem.Name),
            Parameter: nameof(ShoppingItem.Name)),

            (Rule: Invalid(shoppingItem.CreatedAt),
            Parameter: nameof(ShoppingItem.CreatedAt)),

            (Rule: Invalid(shoppingItem.UpdatedAt),
            Parameter: nameof(ShoppingItem.UpdatedAt)));

        Validate(
            (Rule: NotSameAs(
                firstId: shoppingItem.UpdatedBy,
                secondId: shoppingItem.CreatedBy,
                secondIdName: nameof(ShoppingItem.CreatedBy)),
            Parameter: nameof(ShoppingItem.UpdatedBy)),

            (Rule: NotSameAs(
                firstDate: shoppingItem.UpdatedAt,
                secondDate: shoppingItem.CreatedAt,
                secondDateName: nameof(ShoppingItem.CreatedAt)),
            Parameter: nameof(ShoppingItem.UpdatedAt)));
    }

    private static void ValidateShoppingItem(
        ShoppingItem shoppingItem)
    {
        if (shoppingItem is null)
        {
            throw new NullShoppingItemException(
                exceptionMessage: "Shopping item is null.");
        }
    }

    private static dynamic Invalid(Guid id) =>
        new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required."
        };

    private static dynamic Invalid(DateTimeOffset dateTime) =>
        new
        {
            Condition = dateTime == default,
            Message = "Date is required."
        };

    private static dynamic Invalid(string text) =>
        new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required."
        };

    private static dynamic NotSameAs(
        Guid firstId,
        Guid secondId,
        string secondIdName) =>
        new
        {
            Condition = firstId != secondId,
            Message = $"Id not same as {secondIdName}."
        };

    private static dynamic NotSameAs(
        DateTimeOffset firstDate,
        DateTimeOffset secondDate,
        string secondDateName) =>
        new
        {
            Condition = firstDate != secondDate,
            Message = $"Date is not same as {secondDateName}."
        };

    private static void Validate(
        params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidShoppingItemException =
            new InvalidShoppingItemException(
                exceptionMessage: "Invalid shopping item error occurred, " +
                "fix the errors and try again please!");

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidShoppingItemException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }

        invalidShoppingItemException.ThrowIfContainsErrors();
    }
}
