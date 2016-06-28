﻿namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Contacts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Contact.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ContactsSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/contacts";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public ContactsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"All contacts have been deleted")]
        public void DeleteContacts()
        {
            this.fixture.DataContext.Contacts.RemoveRange(this.fixture.DataContext.Contacts.ToList());
        }

        [Given(@"Contacts exists in database")]
        public void CreateContactsInDb(Table table)
        {
            List<Contact> contacts = table.CreateSet<Contact>().ToList();

            this.fixture.DataContext.Contacts.AddRange(contacts);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(contacts, "Contacts");
        }

        [When(@"User creates contact using api with max length required fields")]
        public void CreateContactsWithMaxRequiredFields()
        {
            const int max = 128;

            Guid leadNegotiatorId = this.GetCurrentUserId();
            var contact = new CreateContactCommand()
            {
                LastName = StringExtension.GenerateMaxAlphanumericString(max),
                Title = StringExtension.GenerateMaxAlphanumericString(max),
                LeadNegotiator = new ContactUserCommand() { UserId = leadNegotiatorId }
            };

           this.CreateContactWithApi(contact);         
        }

        [Given(@"User creates contact using api with max length all fields")]
        public void CreateContactsWithMaxAllFields(Table table)
        {
            const int max = 128;

            string defaultMailingSalutation = table.Rows[0]["MailingSalutation"];
            Guid defaultMailingSalutationId =
                this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[defaultMailingSalutation];

            string defaultEventSalutation = table.Rows[0]["EventSalutation"];
            Guid defaultEventSalutationId =
                this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")[defaultEventSalutation];

            //get negotiator ids
            Guid leadNegotiatorId = this.GetCurrentUserId();
            var userList = this.scenarioContext.Get<IEnumerable<User>>("User List");
            Guid secondaryNegotiatorId = userList.First().Id;

            var contact = new CreateContactCommand()
            {
                Title = StringExtension.GenerateMaxAlphanumericString(max),
                FirstName = StringExtension.GenerateMaxAlphanumericString(max),
                LastName = StringExtension.GenerateMaxAlphanumericString(max),
                MailingFormalSalutation = StringExtension.GenerateMaxAlphanumericString(max),
                MailingSemiformalSalutation = StringExtension.GenerateMaxAlphanumericString(max),
                MailingInformalSalutation = StringExtension.GenerateMaxAlphanumericString(max),
                MailingPersonalSalutation = StringExtension.GenerateMaxAlphanumericString(max),
                MailingEnvelopeSalutation = StringExtension.GenerateMaxAlphanumericString(max),
                DefaultMailingSalutationId = defaultMailingSalutationId,
                EventInviteSalutation = StringExtension.GenerateMaxAlphanumericString(max),
                EventSemiformalSalutation = StringExtension.GenerateMaxAlphanumericString(max),
                EventInformalSalutation = StringExtension.GenerateMaxAlphanumericString(max),
                EventPersonalSalutation = StringExtension.GenerateMaxAlphanumericString(max),
                EventEnvelopeSalutation = StringExtension.GenerateMaxAlphanumericString(max),
                DefaultEventSalutationId = defaultEventSalutationId,
                LeadNegotiator = new ContactUserCommand() { UserId = leadNegotiatorId },
                SecondaryNegotiators = new List<ContactUserCommand>() { new ContactUserCommand() { UserId= secondaryNegotiatorId } }
            };

            this.CreateContactWithApi(contact);
        }

        [When(@"User creates contact using api with following data")]
        public void CreateContact(Table table)
        {
            Guid leadNegotiatorId = this.GetCurrentUserId();

            var contact = table.CreateInstance<CreateContactCommand>();
            contact.LeadNegotiator = new ContactUserCommand() { UserId = leadNegotiatorId };
            this.CreateContactWithApi(contact);
        }

        [When(@"User creates contact using api with same negotiators")]
        public void WhenUserCreatesContactUsingApiWithSameNegotiators(Table table)
        {
            var userId= this.scenarioContext.Get<List<User>>("User List").First().Id;
           
            var contact = table.CreateInstance<CreateContactCommand>();
            contact.LeadNegotiator = new ContactUserCommand() { UserId = userId };
            contact.SecondaryNegotiators = new List<ContactUserCommand>()
            {
                new ContactUserCommand() { UserId = userId }
            };
            this.CreateContactWithApi(contact);
        }


        private Guid GetCurrentUserId()
        {
           return this.fixture.DataContext.Users.First().Id;
        }

        [When(@"User retrieves all contact details")]
        public void GetAllContactsDetails()
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves contact details for (.*) id")]
        public void GetContactDetailsUsingId(string contactId)
        {
            if (contactId.Equals("latest"))
            {
                contactId = this.scenarioContext.Get<List<Contact>>("Contacts")[0].Id.ToString();
            }
            else if (contactId.Equals("invalid"))
            {
                contactId = Guid.NewGuid().ToString();
            }

            string requestUrl = $"{ApiUrl}/{contactId}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves all contact's titles")]
        public void GetAllContactsTitles()
        {
            string requestUrl = $"{ApiUrl}/titles";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Contact details should be the same as already added")]
        public void CheckContactDetailsFromPost()
        {
            var contactId = JsonConvert.DeserializeObject<Guid>(this.scenarioContext.GetResponseContent());
            var contactCommand = this.scenarioContext.Get<CreateContactCommand>("Contact");
         
            Contact expectedContact = this.fixture.DataContext.Contacts.Single(x => x.Id.Equals(contactId));
            this.CheckAllFieldsHaveExpectedValues(contactCommand, expectedContact);
        }

        [Then(@"Contact details required fields should be the same as already added")]
        public void CheckContactDetailsRequiredFieldsFromPost()
        {
            var contactId = JsonConvert.DeserializeObject<Guid>(this.scenarioContext.GetResponseContent());
            var contact = this.scenarioContext.Get<CreateContactCommand>("Contact");
            
            Contact expectedContact = this.fixture.DataContext.Contacts.Single(x => x.Id.Equals(contactId));

            contact.Title.ShouldBeEquivalentTo(expectedContact.Title);
            contact.LastName.ShouldBeEquivalentTo(expectedContact.LastName);

            var leadNegotiatorUserTypeId = this.fixture.DataContext.EnumTypeItems.Single(
                i => i.EnumType.Code.Equals(nameof(UserType)) && i.Code.Equals(nameof(UserType.LeadNegotiator))).Id;
            expectedContact.ContactUsers.First(x=>x.UserTypeId == leadNegotiatorUserTypeId).UserId.ShouldBeEquivalentTo(contact.LeadNegotiator.UserId);
        }

        [Then(@"Contact details all fields should be the same as already added")]
        public void CheckContactDetailsAllFieldsFromPost()
        {
            var contactId = JsonConvert.DeserializeObject<Guid>(this.scenarioContext.GetResponseContent());
            var contactCommand = this.scenarioContext.Get<CreateContactCommand>("Contact");
          

            Contact expectedContact = this.fixture.DataContext.Contacts.Single(x => x.Id.Equals(contactId));

            AssertionOptions.AssertEquivalencyUsing(options =>
               options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, 5000)).WhenTypeIs<DateTime>());

            this.CheckAllFieldsHaveExpectedValues(contactCommand, expectedContact);
        }

        private void CheckAllFieldsHaveExpectedValues(CreateContactCommand contactCommand, Contact expectedContact)
        {
            contactCommand.Title.ShouldBeEquivalentTo(expectedContact.Title);
            contactCommand.FirstName.ShouldBeEquivalentTo(expectedContact.FirstName);
            contactCommand.LastName.ShouldBeEquivalentTo(expectedContact.LastName);
            contactCommand.MailingFormalSalutation.ShouldBeEquivalentTo(expectedContact.MailingFormalSalutation);
            contactCommand.MailingSemiformalSalutation.ShouldBeEquivalentTo(expectedContact.MailingSemiformalSalutation);
            contactCommand.MailingInformalSalutation.ShouldBeEquivalentTo(expectedContact.MailingInformalSalutation);
            contactCommand.MailingPersonalSalutation.ShouldBeEquivalentTo(expectedContact.MailingPersonalSalutation);
            contactCommand.MailingEnvelopeSalutation.ShouldBeEquivalentTo(expectedContact.MailingEnvelopeSalutation);
            contactCommand.DefaultMailingSalutationId.ShouldBeEquivalentTo(expectedContact.DefaultMailingSalutationId);
            contactCommand.EventInviteSalutation.ShouldBeEquivalentTo(expectedContact.EventInviteSalutation);
            contactCommand.MailingSemiformalSalutation.ShouldBeEquivalentTo(expectedContact.MailingSemiformalSalutation);
            contactCommand.EventSemiformalSalutation.ShouldBeEquivalentTo(expectedContact.EventSemiformalSalutation);
            contactCommand.EventInformalSalutation.ShouldBeEquivalentTo(expectedContact.EventInformalSalutation);
            contactCommand.EventPersonalSalutation.ShouldBeEquivalentTo(expectedContact.EventPersonalSalutation);
            contactCommand.EventEnvelopeSalutation.ShouldBeEquivalentTo(expectedContact.EventEnvelopeSalutation);
            contactCommand.DefaultEventSalutationId.ShouldBeEquivalentTo(expectedContact.DefaultEventSalutationId);

            //negotiators
            var negotiatorList = new List<ContactUserCommand>();
            negotiatorList.Add(contactCommand.LeadNegotiator);
            negotiatorList.AddRange(contactCommand.SecondaryNegotiators);
            
            expectedContact.ContactUsers.ShouldAllBeEquivalentTo(negotiatorList, opt => opt.Including(x => x.UserId));
          }

        [Then(@"Get contact details should be the same as already added")]
        public void CheckContactDetailsFromGet()
        {
            var contact = JsonConvert.DeserializeObject<Contact>(this.scenarioContext.GetResponseContent());

            Contact expectedContact = this.fixture.DataContext.Contacts.Single(x => x.Id.Equals(contact.Id));
            contact.ShouldBeEquivalentTo(expectedContact);
        }

        [Then(@"Contact details required fields should have expected values")]
        public void CheckContactDetailsRequiredFieldsHaveExpectedValues()
        {
            var contactList = this.scenarioContext.Get<List<Contact>>("Contacts");

            var currentContactsDetails =
                JsonConvert.DeserializeObject<List<Contact>>(this.scenarioContext.GetResponseContent());
            currentContactsDetails.ShouldBeEquivalentTo(contactList);

            contactList.Should()
                       .Equal(currentContactsDetails,
                           (c1, c2) =>
                               c1.Title == c2.Title &&
                               c1.LastName == c2.LastName);
        }

        [Then(@"Contact details all fields should have expected values")]
        public void CheckContactDetailsAllFieldsHaveExpectedValues()
        {
            //TODO compare entities with excluding option instead of comparing each field, use rather ShouldBeEquivalentTo

            var contactList = this.scenarioContext.Get<List<Contact>>("Contacts");

            var currentContactsDetails =
                JsonConvert.DeserializeObject<List<Contact>>(this.scenarioContext.GetResponseContent());
            currentContactsDetails.ShouldBeEquivalentTo(contactList);

            contactList.Should()
                       .Equal(currentContactsDetails,
                           (c1, c2) =>
                               c1.Title == c2.Title &&
                               c1.FirstName == c2.FirstName &&
                               c1.LastName == c2.LastName &&
                               c1.MailingFormalSalutation == c2.MailingFormalSalutation &&
                               c1.MailingSemiformalSalutation == c2.MailingSemiformalSalutation &&
                               c1.MailingInformalSalutation == c2.MailingInformalSalutation &&
                               c1.MailingPersonalSalutation == c2.MailingPersonalSalutation &&
                               c1.MailingEnvelopeSalutation == c2.MailingEnvelopeSalutation &&
                               c1.DefaultMailingSalutationId == c2.DefaultMailingSalutationId &&
                               c1.EventInviteSalutation == c2.EventInviteSalutation &&
                               c1.EventSemiformalSalutation == c2.EventSemiformalSalutation &&
                               c1.EventInformalSalutation == c2.EventInformalSalutation &&
                               c1.EventPersonalSalutation == c2.EventPersonalSalutation &&
                               c1.EventEnvelopeSalutation == c2.EventEnvelopeSalutation &&
                               c1.DefaultEventSalutationId == c2.DefaultEventSalutationId &&
                               c1.UserId == c2.UserId);
        }

        [Then(@"Contact titles list should have expected values")]
        public void ThenContactTitlesListsShouldHaveExpectedValues()
        {
            var response = JsonConvert.DeserializeObject<List<ContactTitle>>(this.scenarioContext.GetResponseContent());
            List<ContactTitle> expectedTitles = this.fixture.DataContext.ContactTitles.ToList();

            expectedTitles.ShouldAllBeEquivalentTo(response, opt => opt.Excluding(x => x.Locale));
        }

        private void CreateContactWithApi(CreateContactCommand contactCommand)
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, contactCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
            this.scenarioContext.Set(contactCommand, "Contact");
        }   
    }
}
