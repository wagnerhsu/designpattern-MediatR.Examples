﻿namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class CreateCompanySteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;

        private string url;

        public CreateCompanySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;

            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [When(@"User navigates to create company page")]
        public void OpenCreateCompanyPage()
        {
            CreateCompanyPage page = new CreateCompanyPage(this.driverContext).OpenCreateCompanyPage();
            this.scenarioContext["CreateCompanyPage"] = page;
        }

        [When(@"User fills in company details on create company page")]
        public void FillInCompanyData(Table table)
        {
            var details = table.CreateInstance<Company>();
            this.url = details.WebsiteUrl;
            this.scenarioContext.Get<CreateCompanyPage>("CreateCompanyPage").SetCompanyName(details.Name);
            this.scenarioContext.Get<CreateCompanyPage>("CreateCompanyPage").SetWebsite(details.WebsiteUrl);
        }

        [When(@"User clicks save company button on create company page")]
        public void SaveCompany()
        {
            this.scenarioContext.Get<CreateCompanyPage>("CreateCompanyPage").SaveCompany();
        }

        [When(@"User selects contacts on create company page")]
        public void SelectContactsForCompany(Table table)
        {
            var page = this.scenarioContext.Get<CreateCompanyPage>("CreateCompanyPage");
            page.AddContactToCompany().WaitForSidePanelToShow();

            IEnumerable<Contact> contacts = table.CreateSet<Contact>();

            foreach (Contact contact in contacts)
            {
                page.ContactsList.WaitForContactsListToLoad().SelectContact(contact.FirstName, contact.Surname);
            }
            page.ContactsList.SaveContact();
            page.WaitForSidePanelToHide();
        }

        [When(@"User clicks on website url icon")]
        public void WhenUserClicksOnWebsiteUrlIcon()
        {
            var page = this.scenarioContext.Get<CreateCompanyPage>("CreateCompanyPage");
            page.ClickOnWebsiteLink();
        }


        [Then(@"List of company contacts should contain following contacts")]
        public void CheckContactsList(Table table)
        {
            var page = this.scenarioContext.Get<CreateCompanyPage>("CreateCompanyPage");

            List<string> contacts =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

            List<string> selectedContacts = page.Contacts;

            Assert.Equal(contacts.Count, selectedContacts.Count);
            contacts.ShouldBeEquivalentTo(selectedContacts);
        }

        [Then(@"Company form on create company page should be diaplyed")]
        public void CheckIfCreateContactIsDisplayed()
        {
            Assert.True(new CreateCompanyPage(this.driverContext).IsCompanyFormPresent());
        }

        [Then(@"New company should be created")]
        public void CheckIfCompanyCreated()
        {
            //TODO implement check if contact was created
        }

        [Then(@"url opens in new tab")]
        public void ThenUrlOpensInNewTab()
        {
            var page = this.scenarioContext.Get<CreateCompanyPage>("CreateCompanyPage");

            Assert.True(page.CheckNewTab(this.url));

        }

    }
}
