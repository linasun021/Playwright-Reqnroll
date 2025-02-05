using Reqnroll.BoDi;
using Microsoft.Extensions.Configuration;
using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using EquipmentStoresTests.Drivers;
using EquipmentStoresTests.Configuration;
using Reqnroll.Infrastructure;

namespace EquipmentStoresTests.StepDefinitions
{
    [Binding]
    public class StepHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IObjectContainer _objectContainer;
        private IAPIRequestContext _apiRequestContext;
        private IPage _page;
        private IPage _adminPage;
        private Settings _settings;

        public StepHooks(IObjectContainer objectContainer, ScenarioContext scenarioContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
            _settings = UIDriverFactory.GetSettings();
        }

        [BeforeScenario]
        public void Setup()
        {
            if (_scenarioContext.ScenarioInfo.Tags.Contains("store"))
            {
                _page = UIDriverFactory.GetPageAsync().Result;
                _objectContainer.RegisterInstanceAs(_page);
            }
            else
            {
                _adminPage = UIDriverFactory.GetPageAsync().Result;
                _objectContainer.RegisterInstanceAs(_adminPage);
            }
            if (_scenarioContext.ScenarioInfo.Tags.Contains("api"))
            {
                _apiRequestContext = APIDriverFactory.CreateAPIConext(_settings.APIBaseUrl).GetAwaiter().GetResult();
                _objectContainer.RegisterInstanceAs(_apiRequestContext);
            }
        }

        [AfterScenario]
        public void Shutdown()
        {
            var filePath = _scenarioContext.ScenarioInfo.Arguments.Count > 0 ? _scenarioContext.ScenarioInfo.Title + "_" + _scenarioContext.ScenarioInfo.Arguments[0] : _scenarioContext.ScenarioInfo.Title;
            UIDriverFactory.CloseAsync(filePath).Wait();
        }

    }

}
