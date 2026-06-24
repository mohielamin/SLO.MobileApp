using SLO.MobileApp.Core.Models.Processings.ShoppingLists.Exceptions;
using System;

namespace SLO.MobileApp.Core.Services.Processings.ShoppingLists;

internal partial class ShoppingListProcessingService
{
    private void ValidateShoppingListOnRetrieveAllByUserId(
        Guid userId)
    {
        Validate(
            (Rule: Invalid(userId), Parameter: nameof(userId)));
    }

    private dynamic Invalid(Guid id) =>
        new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required."
        };

    private void Validate(params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidShoppingListProcessingException =
            new InvalidShoppingListProcessingException(
                exceptionMessage: "Invalid shopping list processing error occurred, " +
                "fix the errors and try again please!");

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidShoppingListProcessingException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }

        invalidShoppingListProcessingException.ThrowIfContainsErrors();
    }
}
