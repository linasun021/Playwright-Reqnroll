using System;
using System.Linq;
using Reqnroll.BoDi;
using Microsoft.Playwright;
using Reqnroll.Infrastructure;

namespace EquipmentStoresTests.StepDefinitions
{
    [Binding]
    public abstract class BaseSteps
    {
        protected readonly IPage page;
        protected IPage adminPage;
        protected readonly IAPIRequestContext apiRestquestContext;
        protected IObjectContainer objectContainer;
        protected ScenarioContext scenarioContext;

        protected BaseSteps(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
            this.objectContainer = objectContainer;
            this.scenarioContext = scenarioContext;
            if (scenarioContext.ScenarioInfo.Tags.Contains("store"))
            {
                page = objectContainer.Resolve<IPage>();
            }
            else
            {
                adminPage = objectContainer.Resolve<IPage>();
            }
            if (scenarioContext.ScenarioInfo.Tags.Contains("api"))
            {
                apiRestquestContext = objectContainer.Resolve<IAPIRequestContext>();
            }
        }
    }
}
