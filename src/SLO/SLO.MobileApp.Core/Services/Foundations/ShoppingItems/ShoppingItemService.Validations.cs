using SLO.MobileApp.Core.Models.Foundations.ShoppingItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingItems.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingItems;

internal sealed partial class ShoppingItemService
{
    private async ValueTask ValidateShoppingItemOnAddAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken)
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

        Validate(
            (Rule: await NotRecentAsync(
                dateTime: shoppingItem.CreatedAt,
                cancellationToken),
            Parameter: nameof(shoppingItem.CreatedAt)));
    }

    private async ValueTask ValidateShoppingItemOnModifyAsync(
        ShoppingItem shoppingItem,
        CancellationToken cancellationToken)
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
            (Rule: SameAs(
                firstDate: shoppingItem.UpdatedAt,
                secondDate: shoppingItem.CreatedAt,
                secondDateName: nameof(ShoppingItem.CreatedAt)),
            Parameter: nameof(ShoppingItem.UpdatedAt)));

        Validate(
            (Rule: await NotRecentAsync(
                dateTime: shoppingItem.UpdatedAt,
                cancellationToken),
            Parameter: nameof(shoppingItem.UpdatedAt)));
    }

    private static void ValidateShoppingItemOnRetrieveById(
        Guid shoppingItemId) =>
        Validate((Rule: Invalid(shoppingItemId),
            Parameter: nameof(shoppingItemId)));

    private static void ValidateShoppingItemOnRemoveById(
        Guid shoppingItemId) =>
        Validate((Rule: Invalid(shoppingItemId),
            Parameter: nameof(shoppingItemId)));

    private static void ValidateAgainstStorageShoppingItem(
        ShoppingItem shoppingItem,
        ShoppingItem storageShoppingItem)
    {
        Validate(
            (Rule: NotSameAs(
                firstId: shoppingItem.CreatedBy,
                secondId: storageShoppingItem.CreatedBy,
                secondIdName: nameof(ShoppingItem.CreatedBy)),
            Parameter: nameof(ShoppingItem.CreatedBy)),

            (Rule: NotSameAs(
                firstDate: shoppingItem.CreatedAt,
                secondDate: storageShoppingItem.CreatedAt,
                secondDateName: nameof(ShoppingItem.CreatedAt)),
            Parameter: nameof(ShoppingItem.CreatedAt)),

            (Rule: SameAs(
                firstDate: shoppingItem.UpdatedAt,
                secondDate: storageShoppingItem.UpdatedAt,
                secondDateName: nameof(ShoppingItem.UpdatedAt)),
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

    private static void ValidateStorageShoppingItem(
        ShoppingItem storageShoppingItem,
        Guid shoppingItemId)
    {
        if (storageShoppingItem is null)
        {
            throw new NotFoundShoppingItemException(
                exceptionMessage: $"A shopping item with Id: {shoppingItemId}, " +
                $"could not be found.");
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

    private static dynamic SameAs(
        Guid firstId,
        Guid secondId,
        string secondIdName) =>
        new
        {
            Condition = firstId == secondId,
            Message = $"Id is same as {secondIdName}."
        };

    private static dynamic SameAs(
        DateTimeOffset firstDate,
        DateTimeOffset secondDate,
        string secondDateName) =>
        new
        {
            Condition = firstDate == secondDate,
            Message = $"Date is same as {secondDateName}."
        };

    private static dynamic NotSameAs(
        Guid firstId,
        Guid secondId,
        string secondIdName) =>
        new
        {
            Condition = firstId != secondId,
            Message = $"Id is not same as {secondIdName}."
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

    private async ValueTask<dynamic> NotRecentAsync(
        DateTimeOffset dateTime,
        CancellationToken cancellationToken) =>
        new
        {
            Condition = await DateIsNotRecentAsync(dateTime, cancellationToken),
            Message = "Date is not recent."
        };

    private async ValueTask<bool> DateIsNotRecentAsync(
        DateTimeOffset dateTime,
        CancellationToken cancellationToken)
    {
        DateTimeOffset currentDateTime =
            await _dateTimeBroker.GetCurrentDateTimeAsync(
                cancellationToken);

        TimeSpan oneMinute = TimeSpan.FromMinutes(1);
        TimeSpan difference = dateTime.Subtract(currentDateTime);

        return difference.Duration() > oneMinute;
    }

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
