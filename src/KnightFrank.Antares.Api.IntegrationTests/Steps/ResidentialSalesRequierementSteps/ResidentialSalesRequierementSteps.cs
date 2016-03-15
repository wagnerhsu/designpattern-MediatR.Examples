﻿namespace KnightFrank.Antares.Api.IntegrationTests.Steps.ResidentialSalesRequierementSteps
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ResidentialSalesRequierementSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/requirement";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public ResidentialSalesRequierementSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [When(@"User creates following requirement with given contact")]
        public void UserCreatesFollowingRequirementWithGivenContact(Table table)
        {
            string requestUrl = $"{ApiUrl}";
            
            var contacts = this.scenarioContext.Get<List<Contact>>("Contact List");

            var requirement = table.CreateInstance<Requirement>();
            requirement.Contacts.Add(contacts[0]);
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, requirement);
            this.scenarioContext.SetHttpResponseMessage(response);
            this.scenarioContext.Set(requirement, "Requirement");
        }

        [When(@"User creates following requirement without contact")]
        public void UserCreatesFollowingRequirementWithoutContact(Table table)
        {
            string requestUrl = $"{ApiUrl}";

            var requirement = table.CreateInstance<Requirement>();
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, requirement);
            this.scenarioContext.SetHttpResponseMessage(response);
            this.scenarioContext.Set(requirement, "Requirement");
        }

        [When(@"User retrieves requirement that he saved")]
        public void WhenUserRetrievesContactsDetailsWith()
        {
             string id = this.scenarioContext.GetResponseContent().Replace("\"", "");

             string requestUrl = $"{ApiUrl}/" + id + "";

             HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
             this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Requirement should be the same as added")]
        public void ThenRequierementShouldBeTheSameAsAdded()
        {
            var tableRequirement = this.scenarioContext.Get<Requirement>("Requirement");
            var databaseRequirement = JsonConvert.DeserializeObject<Requirement>(this.scenarioContext.GetResponseContent());
            databaseRequirement.ShouldBeEquivalentTo(tableRequirement,
                opt => opt.Excluding(req => req.Id).Excluding(req => req.CreateDate));
        }

        [When(@"User retrieves requirement for (.*) id")]
        public void WhenUserRetrievesRequirementForId(string id)
        {
            string requestUrl = $"{ApiUrl}/" + id + "";

            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

    }
}
