﻿// <copyright file="PaginatedCategoryExtendedAttributeFilterValidator.cs" company="Fluentpos">
// --------------------------------------------------------------------------------------------------
// Copyright (c) Fluentpos. All rights reserved.
// The core team: Mukesh Murugan (iammukeshm), Chhin Sras (chhinsras), Nikolay Chebotov (unchase).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------
// </copyright>

using System;
using FluentPOS.Modules.Catalog.Core.Entities;
using FluentPOS.Shared.Core.Features.ExtendedAttributes.Queries.Validators;
using Microsoft.Extensions.Localization;

namespace FluentPOS.Modules.Catalog.Core.Features.ExtendedAttributes.Validators.Categories
{
    public class PaginatedCategoryExtendedAttributeFilterValidator : PaginatedExtendedAttributeFilterValidator<Guid, Category>
    {
        public PaginatedCategoryExtendedAttributeFilterValidator(IStringLocalizer<PaginatedCategoryExtendedAttributeFilterValidator> localizer)
            : base(localizer)
        {
            // you can override the validation rules here
        }
    }
}