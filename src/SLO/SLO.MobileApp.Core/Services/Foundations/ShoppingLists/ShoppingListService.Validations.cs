using SLO.MobileApp.Core.Models.Foundations.ShoppingLists;
using SLO.MobileApp.Core.Models.Foundations.ShoppingLists.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLO.MobileApp.Core.Services.Foundations.ShoppingLists;

internal partial class ShoppingListService
{
    private async ValueTask ValidateShoppingListOnAddAsync(
        ShoppingList shoppingList,
        CancellationToken cancellationToken)
    {
        ValidateShoppingList(shoppingList);

        Validate(
            (Rule: Invalid(shoppingList.Id),
            Parameter: nameof(ShoppingList.Id)),

            (Rule: Invalid(shoppingList.Name),
            Parameter: nameof(ShoppingList.Name)),

            (Rule: Invalid(shoppingList.CreatedBy),
            Parameter: nameof(ShoppingList.CreatedBy)),

            (Rule: Invalid(shoppingList.UpdatedBy),
            Parameter: nameof(ShoppingList.UpdatedBy)),

            (Rule: Invalid(shoppingList.CreatedAt),
            Parameter: nameof(shoppingList.CreatedAt)),

            (Rule: Invalid(shoppingList.UpdatedAt),
            Parameter: nameof(shoppingList.UpdatedAt)));

        Validate(
            (Rule: NotSameAs(
                firstId: shoppingList.UpdatedBy,
                secondId: shoppingList.CreatedBy,
                secondIdName: nameof(ShoppingList.CreatedBy)),
            Parameter: nameof(ShoppingList.UpdatedBy)),

            (Rule: NotSameAs(
                firstDate: shoppingList.UpdatedAt,
                secondDate: shoppingList.CreatedAt,
                secondDateName: nameof(ShoppingList.CreatedAt)),
            Parameter: nameof(ShoppingList.UpdatedAt)));

        Validate(
            (Rule: await NotRecentAsync(
                dateTime: shoppingList.CreatedAt,
                cancellationToken),
            Parameter: nameof(ShoppingList.CreatedAt)));
    }

    private static void ValidateShoppingList(
        ShoppingList shoppingList)
    {
        if (shoppingList is null)
        {
            throw new NullShoppingListException(
                exceptionMessage: "Shopping list is null.");
        }
    }

    private static dynamic Invalid(Guid id) =>
        new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required."
        };

    private static dynamic Invalid(string text) =>
        new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required."
        };

    private static dynamic Invalid(DateTimeOffset dateTime) =>
        new
        {
            Condition = dateTime == default,
            Message = "Date is required."
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
            Condition = await DateIsNotRecentAsync(
                dateTime,
                cancellationToken),

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
        TimeSpan difference = currentDateTime.Subtract(dateTime);

        return difference.Duration() > oneMinute;
    }

    private static void Validate(
        params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidShoppingListException =
            new InvalidShoppingListException(
                exceptionMessage: "Invalid shopping list error occurred, " +
                "fix the errors and try again please!");

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidShoppingListException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }

        invalidShoppingListException.ThrowIfContainsErrors();
    }
}
