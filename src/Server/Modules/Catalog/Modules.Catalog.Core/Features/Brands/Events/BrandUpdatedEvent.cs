﻿using FluentPOS.Shared.Application.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentPOS.Modules.Catalog.Core.Features.Brands.Events
{
    public class BrandUpdatedEvent : Event
    {
        public BrandUpdatedEvent(Guid id, string name, string imageUrl, string detail)
        {
            Name = name;
            ImageUrl = imageUrl;
            Detail = detail;
            Id = id;
            AggregateId = id;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Detail { get; set; }
    }
}