﻿namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;

    using MediatR;

    public class UpdatePropertyCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public CreateOrUpdatePropertyAddress Address { get; set; }
    }
}