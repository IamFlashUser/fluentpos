﻿// <copyright file="PaginatedBrandFilterValidator.cs" company="Fluentpos">
// --------------------------------------------------------------------------------------------------
// Copyright (c) Fluentpos. All rights reserved.
// The core team: Mukesh Murugan (iammukeshm), Chhin Sras (chhinsras), Nikolay Chebotov (unchase).
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------
// </copyright>

using System;
using FluentPOS.Modules.Catalog.Core.Entities;
using FluentPOS.Shared.Core.Features.Common.Queries.Validators;
using FluentPOS.Shared.DTOs.Catalogs.Brands;
using Microsoft.Extensions.Localization;

namespace FluentPOS.Modules.Catalog.Core.Features.Brands.Queries.Validators
{
    public class PaginatedBrandFilterValidator : PaginatedFilterValidator<Guid, Brand, PaginatedBrandFilter>
    {
        public PaginatedBrandFilterValidator(IStringLocalizer<PaginatedBrandFilterValidator> localizer)
            : base(localizer)
        {
            // you can override the validation rules here
        }
    }
}