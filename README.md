# Azure Static WebApp with Blazor WASM

After watching a video by James Montemagno, I was amazed that it only took a few clicks to get up and running. So, I started this repository for a small project and to have fun with Static WebApp.

Unfortunately, the API function **does not support QueueTrigger**, so I did not continue working on it...

##### Resources

- [Blazor-starter template](https://github.com/staticwebdev/blazor-starter) 👤*StaticWebDev*
- [Fallen in love Azure Static Web Apps](https://youtu.be/AgP4p8qhi4s) 📽️*26m - James Montemagno*
- [Authentication in Azure Static Web Apps](https://youtu.be/SqgSUgDlgLM) 📽*️35m - dotnetFlix - Stacy Cashmore*
- [Authenticate and Authorize](https://learn.microsoft.com/en-us/azure/static-web-apps/authentication-authorization) 📚*Microsoft learn*
- [Deploying Blazor WASM to Azure Static Web Apps](https://www.dotnetcurry.com/aspnet-core/deploy-blazor-webassembly-azure-static-web-apps) 📓*DotnetCurry*
- [#30DaysOfSWA](https://www.azurestaticwebapps.dev/) 📓
- [Azure Static Web Apps CLI](https://azure.github.io/static-web-apps-cli) 📓*GitHub documentation*
  - Visual Studio can run the Client and API as multiple startup projects. If you are using the built-in authentication, you will need to start the Static WebApp with the following command:
    - swa start http://localhost:5000 --api-location http://localhost:7071
  - The SWA CLI tool requires Node.js, and it was working with version 14.


###### Other resources

- [Deploying Blazor WASM to AWS S3 – Static Website Hosting with AWS + CDN with CloudFront](https://codewithmukesh.com/blog/deploying-blazor-webassembly-to-aws-s3) 📓*CodeWithMukesh* 
- [Building website without installing anything](https://youtu.be/1Vg7bNjJY-0) *(Blazor WASM, GitHub Codespaces, GitHub Pages)* 📽️*8min-dotnet*