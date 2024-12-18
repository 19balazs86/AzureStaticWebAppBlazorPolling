# Azure Static WebApp with Blazor WASM

After watching a video by James Montemagno, I was amazed that it only took a few clicks to get up and running. So, I started this repository for a small project and to have fun with Static WebApp.

Unfortunately, the API function does not support QueueTrigger. **Only HTTP triggers are allowed** to work with. [See the constraints](https://learn.microsoft.com/en-us/azure/static-web-apps/apis-functions#constraints).

I did not continue working on it...

## Resources

- [Blazor-starter template](https://github.com/staticwebdev/blazor-starter) 👤*StaticWebDev*
- [Deploying, Custom domain (DNS zones), Authentication, Preview environments](https://youtu.be/igkqYNnO8Xg) 📽️*39 min - MS Azure Developers*
- [Fallen in love Azure Static Web Apps](https://youtu.be/AgP4p8qhi4s) 📽️*26 min - James Montemagno*
- [Authenticate and Authorize](https://learn.microsoft.com/en-us/azure/static-web-apps/authentication-authorization) 📚*Microsoft learn*
  - [Authentication in Azure Static Web Apps](https://youtu.be/SqgSUgDlgLM) 📽*️35 min - dotnetFlix - Stacy Cashmore*
  - [Auth0 in Azure SWA for Blazor WASM](https://auth0.com/blog/support-auth0-in-azure-static-web-apps-for-blazor-wasm) 📓*Auth0 blog*
- [#30DaysOfSWA](https://www.azurestaticwebapps.dev/) 📓
- [Azure Static Web Apps CLI](https://azure.github.io/static-web-apps-cli) 📓*GitHub documentation*
  - Visual Studio can run the Client and API as multiple startup projects. If you are using the built-in authentication, you will need to start the Static WebApp with the following command:
    - `swa start http://localhost:5000 --api-location http://localhost:7071`
  - Other option: [Configure SWA CLI swa-cli.config.json file](https://learn.microsoft.com/en-us/azure/static-web-apps/static-web-apps-cli-configuration) when you use `swa start`
  - The SWA CLI tool requires Node.js, and it was working with version 14.
  - [Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools/releases) 👤*Azure, install it if needed*
- Connecting to a database with SWA
  - [Database connection overview](https://learn.microsoft.com/en-us/azure/static-web-apps/database-overview) 📚*Microsoft learn*
  - [Best practices with database connections](https://techcommunity.microsoft.com/t5/apps-on-azure-blog/building-static-web-apps-with-database-connections-best/ba-p/3777155) 📓*Tech Community*
  - [Connect to database directly from SWA](https://youtu.be/vGOnh0UrADg) 📽️*6 min - Azure Tips and Tricks - Microsoft Developer*
- Data API Builder (DAP)
  - [Data API builder documentation](https://learn.microsoft.com/en-us/azure/data-api-builder) 📚*Microsoft learn*
  - [Repository for DAB](https://github.com/Azure/data-api-builder) 👤*Azure*
  - [DAB samples](https://github.com/Azure-Samples/data-api-builder) 👤*Azure Samples*
  - [EF Power Tools to generate DAB-config](https://erikej.github.io/dotnet/sqlserver/powertools/2024/08/05/powertools-dab.html) 📓*ErikEJ*
  - Quick-start: [Try out the Data API Builder](https://bartwullems.blogspot.com/2023/03/azure-data-api-builder.html) 📓*Bart Wullems*
  - 📽 Microsoft Zero to Hero Community
    - [Instant API using Data API builder](https://youtu.be/FsE4LVr2xQI) 📽*️1h:16m*
    - [Using DAB locally and in Azure Container Apps](https://youtu.be/IHgeNJgnDm4) 📽*️58 min*
- GitHub actions and SWA [using Lighthouse](https://johnnyreilly.com/lighthouse-meet-github-actions) and [Playwright for SWA staging environments](https://johnnyreilly.com/playwright-github-actions-and-azure-static-web-apps-staging-environments) 📓*Johnny Reilly*

## Other resources

- [Deploying Blazor WASM to AWS S3 – Static Website Hosting with AWS + CDN with CloudFront](https://codewithmukesh.com/blog/deploying-blazor-webassembly-to-aws-s3) 📓*CodeWithMukesh* 
- [Building website without installing anything](https://youtu.be/1Vg7bNjJY-0) *(Blazor WASM, GitHub Codespaces, GitHub Pages)* 📽️*8 min - dotnet*