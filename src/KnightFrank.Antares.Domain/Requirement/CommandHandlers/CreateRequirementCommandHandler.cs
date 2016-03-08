﻿namespace KnightFrank.Antares.Domain.Requirement.CommandHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using Dal.Model;
    using Dal.Repository;
    using Commands;

    using MediatR;

    public class CreateRequirementCommandHandler : IRequestHandler<CreateRequirementCommand, int>
    {
        private readonly IGenericRepository<Requirement> requirementRepository;

        private readonly IReadGenericRepository<Contact> contactRepository;

        public CreateRequirementCommandHandler(IGenericRepository<Requirement> requirementRepository, IReadGenericRepository<Contact> contactRepository)
        {
            this.requirementRepository = requirementRepository;
            this.contactRepository = contactRepository;
        }

        public int Handle(CreateRequirementCommand message)
        {
            var requirement = AutoMapper.Mapper.Map<Requirement>(message);

            List<int> ids = message.Contacts.Select(x => x.Id).ToList();
            List<Contact> existingContacts = this.contactRepository.FindBy(x => ids.Any(id => id == x.Id)).ToList();
            requirement.Contacts = existingContacts;

            this.requirementRepository.Add(requirement);
            this.requirementRepository.Save();

            return requirement.Id;
        }
    }
}