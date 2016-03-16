﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.0.0.0
//      SpecFlow Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace KnightFrank.Antares.Api.IntegrationTests.Tests.ResidentialSalesRequirement
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class NoteOnResidentialSalesRequirementFeature : Xunit.IClassFixture<NoteOnResidentialSalesRequirementFeature.FixtureData>, System.IDisposable
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "NoteOnResidentialSalesRequirement.feature"
#line hidden
        
        public NoteOnResidentialSalesRequirementFeature()
        {
            this.TestInitialize();
        }
        
        public static void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Note on residential sales requirement", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        public static void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        public virtual void TestInitialize()
        {
        }
        
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        public virtual void SetFixture(NoteOnResidentialSalesRequirementFeature.FixtureData fixtureData)
        {
        }
        
        void System.IDisposable.Dispose()
        {
            this.ScenarioTearDown();
        }
        
        [Xunit.FactAttribute(Skip="Ignored")]
        [Xunit.TraitAttribute("FeatureTitle", "Note on residential sales requirement")]
        [Xunit.TraitAttribute("Description", "Get list of all notes for existing residential sales requirement")]
        [Xunit.TraitAttribute("Category", "ResidentialSalesRequirements")]
        public virtual void GetListOfAllNotesForExistingResidentialSalesRequirement()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get list of all notes for existing residential sales requirement", new string[] {
                        "ignore",
                        "ResidentialSalesRequirements"});
#line 5
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "FirstName",
                        "Surname",
                        "Title"});
            table1.AddRow(new string[] {
                        "Tomasz",
                        "Bien",
                        "Mister"});
#line 6
 testRunner.When("User creates a contact with following data", ((string)(null)), table1, "When ");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "MinPrice",
                        "MaxPrice",
                        "MinBedrooms",
                        "MaxBedrooms",
                        "MinReceptionRooms",
                        "MaxReceptionRooms",
                        "MinBathrooms",
                        "MaxBathrooms",
                        "MinParkingSpaces",
                        "MaxParkingSpaces",
                        "MinArea",
                        "MaxArea",
                        "MinLandArea",
                        "MaxLandArea",
                        "Description"});
            table2.AddRow(new string[] {
                        "1000000",
                        "4000000",
                        "1",
                        "5",
                        "0",
                        "2",
                        "1",
                        "3",
                        "1",
                        "2",
                        "1200",
                        "2000",
                        "10000",
                        "20000",
                        "RequirementDescription"});
#line 9
  testRunner.And("User creates following requirement with given contact", ((string)(null)), table2, "And ");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description"});
            table3.AddRow(new string[] {
                        "Test note description"});
            table3.AddRow(new string[] {
                        "Second test note description"});
#line 12
  testRunner.And("User creates note with following details for given residential sales requirement", ((string)(null)), table3, "And ");
#line 16
  testRunner.And("User retrieves notes for residential sales requirement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line 17
 testRunner.Then("User should get OK http status code", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 18
  testRunner.And("Notes details should be the same as already added", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "And ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(Skip="Ignored")]
        [Xunit.TraitAttribute("FeatureTitle", "Note on residential sales requirement")]
        [Xunit.TraitAttribute("Description", "Save note to non existing residential sales requirement")]
        [Xunit.TraitAttribute("Category", "ResidentialSalesRequirements")]
        public virtual void SaveNoteToNonExistingResidentialSalesRequirement()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Save note to non existing residential sales requirement", new string[] {
                        "ignore",
                        "ResidentialSalesRequirements"});
#line 22
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Description"});
            table4.AddRow(new string[] {
                        "Test note description"});
#line 23
 testRunner.When("User creates note with following details for non existing residential sales requi" +
                    "rement", ((string)(null)), table4, "When ");
#line 26
 testRunner.Then("User should get BadRequest http status code", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [Xunit.FactAttribute(Skip="Ignored")]
        [Xunit.TraitAttribute("FeatureTitle", "Note on residential sales requirement")]
        [Xunit.TraitAttribute("Description", "Get list of all notes for non existing residential sales requirement")]
        [Xunit.TraitAttribute("Category", "ResidentialSalesRequirements")]
        public virtual void GetListOfAllNotesForNonExistingResidentialSalesRequirement()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Get list of all notes for non existing residential sales requirement", new string[] {
                        "ignore",
                        "ResidentialSalesRequirements"});
#line 30
this.ScenarioSetup(scenarioInfo);
#line 31
 testRunner.When("User retrieves notes for non existing residential sales requirement", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "When ");
#line 32
 testRunner.Then("User should get BadRequest http status code", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : System.IDisposable
        {
            
            public FixtureData()
            {
                NoteOnResidentialSalesRequirementFeature.FeatureSetup();
            }
            
            void System.IDisposable.Dispose()
            {
                NoteOnResidentialSalesRequirementFeature.FeatureTearDown();
            }
        }
    }
}
#pragma warning restore
#endregion