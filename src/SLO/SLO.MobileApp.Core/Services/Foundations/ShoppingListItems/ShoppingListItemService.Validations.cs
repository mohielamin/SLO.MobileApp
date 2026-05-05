using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems;
using SLO.MobileApp.Core.Models.Foundations.ShoppingListItems.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingListItems;

internal partial class ShoppingListItemService
{
    private async ValueTask ValidateShoppingListItemOnAddAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken)
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

        Validate(
            (Rule: NotSameAs(
                firstId: shoppingListItem.UpdatedBy,
                secondId: shoppingListItem.CreatedBy,
                secondIdName: nameof(ShoppingListItem.CreatedBy)),
            Parameter: nameof(ShoppingListItem.UpdatedBy)),

            (Rule: NotSameAs(
                firstDate: shoppingListItem.UpdatedAt,
                secondDate: shoppingListItem.CreatedAt,
                secondDateName: nameof(ShoppingListItem.CreatedAt)),
            Parameter: nameof(ShoppingListItem.UpdatedAt)));

        Validate(
            (Rule: await NotRecentAsync(
                dateTime: shoppingListItem.CreatedAt,
                cancellationToken),
            Parameter: nameof(shoppingListItem.CreatedAt)));
    }

    private void ValidateShoppingListItemOnRetrieveById(
        Guid shoppingListItemId)
    {
        Validate(
            (Rule: Invalid(shoppingListItemId),
            Parameter: nameof(shoppingListItemId)));
    }

    private async ValueTask ValidateShoppingListItemOnModifyAsync(
        ShoppingListItem shoppingListItem,
        CancellationToken cancellationToken)
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

        Validate(
            (Rule: SameAs(
                firstDate: shoppingListItem.UpdatedAt,
                secondDate: shoppingListItem.CreatedAt,
                secondDateName: nameof(ShoppingListItem.CreatedAt)),
            Parameter: nameof(ShoppingListItem.UpdatedAt)));

        Validate(
            (Rule: await NotRecentAsync(
                dateTime: shoppingListItem.UpdatedAt,
                cancellationToken),

            Parameter: nameof(ShoppingListItem.UpdatedAt)));
    }

    private void ValidateShoppingListItemOnRemoveById(
        Guid shoppingListItemId)
    {
        Validate(
            (Rule: Invalid(shoppingListItemId),
            Parameter: nameof(shoppingListItemId)));
    }

    private void ValidateStorageShoppingListItem(
        ShoppingListItem storageShoppingListItem,
        Guid shoppingListItemId)
    {
        if (storageShoppingListItem is null)
        {
            throw new NotFoundShoppingListItemException(
                exceptionMessage: $"A shopping list item with Id: " +
                $"{shoppingListItemId}, could not be found.");
        }
    }

    private void ValidateAgainstStorageShoppingListItem(
        ShoppingListItem storageShoppingListItem,
        ShoppingListItem inputShoppingListItem)
    {
        Validate(
            (Rule: NotSameAs(
                firstId: storageShoppingListItem.CreatedBy,
                secondId: inputShoppingListItem.CreatedBy,
                secondIdName: nameof(ShoppingListItem.CreatedBy)),
            Parameter: nameof(ShoppingListItem.CreatedBy)),

            (Rule: NotSameAs(
                firstDate: storageShoppingListItem.CreatedAt,
                secondDate: inputShoppingListItem.CreatedAt,
                secondDateName: nameof(ShoppingListItem.CreatedAt)),
            Parameter: nameof(ShoppingListItem.CreatedAt)),

            (Rule: SameAs(
                firstDate: storageShoppingListItem.UpdatedAt,
                secondDate: inputShoppingListItem.UpdatedAt,
                secondDateName: nameof(ShoppingListItem.UpdatedAt)),
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

    private dynamic NotSameAs(
        Guid firstId,
        Guid secondId,
        string secondIdName) => new
        {
            Condition = firstId != secondId,
            Message = $"Id is not same as {secondIdName}."
        };

    private dynamic NotSameAs(
        DateTimeOffset firstDate,
        DateTimeOffset secondDate,
        string secondDateName) =>
        new
        {
            Condition = firstDate != secondDate,
            Message = $"Date is not same as {secondDateName}."
        };

    private dynamic SameAs(
        DateTimeOffset firstDate,
        DateTimeOffset secondDate,
        string secondDateName) =>
        new
        {
            Condition = firstDate == secondDate,
            Message = $"Date is same as {secondDateName}."
        };

    private async ValueTask<dynamic> NotRecentAsync(
        DateTimeOffset dateTime,
        CancellationToken cancellationToken) =>
        new
        {
            Condition =
            await DateNotRecentAsync(
                dateTime,
                cancellationToken),

            Message = "Date is not recent."
        };

    private async ValueTask<bool> DateNotRecentAsync(
        DateTimeOffset dateTime,
        CancellationToken cancellationToken)
    {
        DateTimeOffset currentDateTime =
            await _dateTimeBroker.GetCurrentDateTimeAsync(
                cancellationToken);

        TimeSpan oneMinute = TimeSpan.FromMinutes(minutes: 1);
        TimeSpan difference = currentDateTime - dateTime;

        return difference.Duration() > oneMinute;
    }

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
