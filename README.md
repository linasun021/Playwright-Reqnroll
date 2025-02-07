# Adobe Commerce Equipment Test Automation Repository

This Repository is a comprehensive suite of automated tests designed to ensure the reliability of Adobe Commerce Equipment functionalities, including Front End, Admin, and API components. 
Built using [.NET](https://dotnet.microsoft.com/), [Playwright](https://playwright.dev), [Reqnroll.Nunix](https://reqnroll.net/) and [Allure Report](https://allurereport.org/). Playwright enabling robust end-to-end testing across multiple web browsers, the integration with Reqnroll, Nunix enhances test management and execution efficiency, while Allure Report provides detailed and insightful test reports for easy tracking of results and identification of issues.

## Installation Guide

Prerequisites

Visual studio 2022 with Reqnroll Extension

Node.js (for Playwright)

[Allure Report](https://allurereport.org/docs/install/)

### Steps

Clone the repository:

➜ git clone https://gitlab.com/lesmills-international/adobe-commerce/equipment-stores-tests.git

Open the slution using Visual Studio 2022

Install Playwright browsers using Terminal: 

➜ npx playwright install

## Configration
Updated the Headless to false in appsetting.json

## Running Tests
Execute test scenarios using Test Explore 

## Reporting
Go to correct directory in Terminal and generate the reports

➜ cd bin\Debug\net8.0 

➜ allure serve allure-results
