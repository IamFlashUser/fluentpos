﻿using AutoMapper;
using FluentPOS.Shared.Core.Contracts;
using FluentPOS.Shared.Core.Domain;
using FluentPOS.Shared.Core.Exceptions;
using FluentPOS.Shared.Core.Extensions;
using FluentPOS.Shared.Core.Interfaces;
using FluentPOS.Shared.Core.Wrapper;
using FluentPOS.Shared.DTOs.ExtendedAttributes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentPOS.Shared.Core.Mappings.Converters;

namespace FluentPOS.Shared.Core.Features.ExtendedAttributes.Queries
{
    public class ExtendedAttributeQueryHandler
    {
        // for localization
    }

    public class ExtendedAttributeQueryHandler<TEntityId, TEntity, TExtendedAttribute> :
        IRequestHandler<GetAllPagedExtendedAttributesQuery<TEntityId, TEntity>, PaginatedResult<GetExtendedAttributesResponse<TEntityId>>>,
        IRequestHandler<GetExtendedAttributeByIdQuery<TEntityId, TEntity>, Result<GetExtendedAttributeByIdResponse<TEntityId>>>
            where TEntity : class, IEntity<TEntityId>
            where TExtendedAttribute : ExtendedAttribute<TEntityId, TEntity>
    {
        private readonly IExtendedAttributeDbContext<TEntityId, TEntity, TExtendedAttribute> _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ExtendedAttributeQueryHandler> _localizer;

        public ExtendedAttributeQueryHandler(IExtendedAttributeDbContext<TEntityId, TEntity, TExtendedAttribute> context, IMapper mapper, IStringLocalizer<ExtendedAttributeQueryHandler> localizer)
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedResult<GetExtendedAttributesResponse<TEntityId>>> Handle(GetAllPagedExtendedAttributesQuery<TEntityId, TEntity> request, CancellationToken cancellationToken)
        {
            Expression<Func<TExtendedAttribute, GetExtendedAttributesResponse<TEntityId>>> expression = e => new GetExtendedAttributesResponse<TEntityId>(e.Id, e.EntityId, e.Type, e.Key, e.Decimal, e.Text, e.DateTime, e.Json, e.Boolean, e.Integer, e.ExternalId, e.Group, e.Description, e.IsActive);

            var queryable = _context.ExtendedAttributes.OrderBy(x => x.Id).AsQueryable();

            var ordering = new OrderByConverter().Convert(request.OrderBy);
            queryable = !string.IsNullOrWhiteSpace(ordering) ? queryable.OrderBy(ordering) : queryable.OrderBy(a => a.Id);

            // apply filter parameters
            if (request.EntityId != null && !request.EntityId.Equals(default(TEntityId))) queryable = queryable.Where(b => b.EntityId.Equals(request.EntityId));
            if (request.Type != null) queryable = queryable.Where(b => b.Type == request.Type);
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                var lowerSearchString = request.SearchString.ToLower();
                queryable = queryable.Where(x => EF.Functions.Like(x.Key.ToLower(), $"%{lowerSearchString}%")
                        || x.Type == ExtendedAttributeType.Decimal && x.Decimal != null && EF.Functions.Like(x.Decimal.ToString().ToLower(), $"%{lowerSearchString}%")
                        || x.Type == ExtendedAttributeType.Text && x.Text != null && EF.Functions.Like(x.Text.ToLower(), $"%{lowerSearchString}%")
                        || x.Type == ExtendedAttributeType.DateTime && x.DateTime != null && EF.Functions.Like(x.DateTime.ToString().ToLower(), $"%{lowerSearchString}%")
                        || x.Type == ExtendedAttributeType.Json && x.Json != null && EF.Functions.Like(x.Json.ToLower(), $"%{lowerSearchString}%")
                        || x.Type == ExtendedAttributeType.Integer && x.Integer != null && EF.Functions.Like(x.Integer.ToString().ToLower(), $"%{lowerSearchString}%")
                        || x.ExternalId != null && EF.Functions.Like(x.ExternalId.ToLower(), $"%{lowerSearchString}%")
                        || x.Group != null && EF.Functions.Like(x.Group.ToLower(), $"%{lowerSearchString}%")
                        || x.Description != null && EF.Functions.Like(x.Description.ToLower(), $"%{lowerSearchString}%")
                        || EF.Functions.Like(x.Id.ToString().ToLower(), $"%{lowerSearchString}%")
                    );
            }

            var extendedAttributeList = await queryable
                .Select(expression)
                .AsNoTracking()
                .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            if (extendedAttributeList == null) throw new CustomException(string.Format(_localizer["{0} Extended Attributes Not Found!"], typeof(TEntity).Name), statusCode: HttpStatusCode.NotFound);

            var mappedExtendedAttributes = _mapper.Map<PaginatedResult<GetExtendedAttributesResponse<TEntityId>>>(extendedAttributeList);

            return mappedExtendedAttributes;
        }

        public async Task<Result<GetExtendedAttributeByIdResponse<TEntityId>>> Handle(GetExtendedAttributeByIdQuery<TEntityId, TEntity> query, CancellationToken cancellationToken)
        {
            var extendedAttribute = await _context.ExtendedAttributes.Where(b => b.Id == query.Id).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
            if (extendedAttribute == null) throw new CustomException(string.Format(_localizer["{0} Extended Attribute Not Found!"], typeof(TEntity).Name), statusCode: HttpStatusCode.NotFound);
            var mappedExtendedAttribute = _mapper.Map<GetExtendedAttributeByIdResponse<TEntityId>>(extendedAttribute);
            return await Result<GetExtendedAttributeByIdResponse<TEntityId>>.SuccessAsync(mappedExtendedAttribute);
        }
    }
}