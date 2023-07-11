using Microsoft.AspNetCore.Mvc.ModelBinding;
using SharedKernel.Application.Cqrs.Queries.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using FilterOperator = SharedKernel.Application.Cqrs.Queries.Entities.FilterOperator;

namespace SharedKernel.Api.Binders;

/// <summary>  </summary>
public abstract class PageOptionsBinder
{
    /// <summary>  </summary>
    protected PageOptions GetPagedOptions(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
            throw new ArgumentNullException(nameof(bindingContext));

        var pageOptions = new PageOptions();

        var skip = bindingContext.ValueProvider.GetValue("pageOptions[skip]");
        if (skip.FirstValue != default)
            pageOptions.Skip = int.Parse(skip.FirstValue);

        var take = bindingContext.ValueProvider.GetValue("pageOptions[take]");
        if (take.FirstValue != default)
            pageOptions.Take = int.Parse(take.FirstValue);

        var searchText = bindingContext.ValueProvider.GetValue("pageOptions[searchText]");
        if (searchText.FirstValue != default)
            pageOptions.SearchText = searchText.FirstValue;

        var showDeleted = bindingContext.ValueProvider.GetValue("pageOptions[showDeleted]");
        if (showDeleted.FirstValue != default)
            pageOptions.ShowDeleted = bool.Parse(showDeleted.FirstValue);

        var showOnlyDeleted = bindingContext.ValueProvider.GetValue("pageOptions[showOnlyDeleted]");
        if (showOnlyDeleted.FirstValue != default)
            pageOptions.ShowOnlyDeleted = bool.Parse(showOnlyDeleted.FirstValue);

        var ordersFieldsValues = bindingContext.ValueProvider.GetValue("pageOptions[orders][field]");
        var ordersAscendingValues = bindingContext.ValueProvider.GetValue("pageOptions[orders][ascending]");
        var orders = new List<Order>();
        for (var i = 0; i < ordersFieldsValues.Values.Count; i++)
        {
            var field = ordersFieldsValues.ToArray()[i];
            var ok = bool.TryParse(ordersAscendingValues.ToArray()[i], out var ascending);
            if (field != default)
                orders.Add(new Order(field, !ok || ascending));
        }
        pageOptions.Orders = orders;

        var filtersFieldsValues = bindingContext.ValueProvider.GetValue("pageOptions[filterProperties][field]");
        var filtersAscendingValues = bindingContext.ValueProvider.GetValue("pageOptions[filterProperties][value]");
        var filtersOperatorValues = bindingContext.ValueProvider.GetValue("pageOptions[filterProperties][operator]");
        var filtersIgnoreCaseValues = bindingContext.ValueProvider.GetValue("pageOptions[filterProperties][ignoreCase]");
        var filters = new List<FilterProperty>();
        for (var i = 0; i < filtersFieldsValues.Values.Count; i++)
        {
            var field = filtersFieldsValues.ToArray()[i];
            var operatorValue = filtersOperatorValues.ToArray()[i];
            FilterOperator? filterOperator = default;
            if (operatorValue != default)
            {
                var okOperator = Enum.TryParse(typeof(FilterOperator), filtersOperatorValues.ToArray()[i], out var @operator);
                if (okOperator)
                    filterOperator = (FilterOperator?)@operator;
            }

            var ok = bool.TryParse(filtersIgnoreCaseValues.ToArray()[i], out var ignoreCase);
            if (field != default)
                filters.Add(new FilterProperty(field, filtersAscendingValues.ToArray()[i], filterOperator,
                    !ok || ignoreCase));
        }
        pageOptions.FilterProperties = filters;

        return pageOptions;
    }
}

// Testing input
// {
//"pageOptions": {
//    "skip": 1,
//    "take": 2,
//    "searchText": "string3",
//    "showDeleted": false,
//    "showOnlyDeleted": true,
//    "orders": [
//    {
//        "field": "id",
//        "ascending": false
//    },
//    {
//        "field": "name",
//        "ascending": true
//    }

//    ],
//    "filterProperties": [
//    {
//        "field": "id",
//        "value": "1",
//        "operator": 1,
//        "ignoreCase": true
//    },
//    {
//        "field": "id2",
//        "value": "12",
//        "operator": 2,
//        "ignoreCase": false
//    }

//    ]
//}
//}