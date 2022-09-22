# Intro

Website of DotnetWorks: https://www.dotnet-works.com

# Deployment

After initial setup (see below) you can create / update the aws lambda with this cli command:

```
dotnet lambda deploy-serverless --stack-name Dnw-Website --s3-bucket dnw-templates-2022
```

And to test your changes:

```
curl https://8tvctbmdz9.execute-api.ap-southeast-1.amazonaws.com/
```

# Aws Lambda setup

1. Configure AWS Lambda in the project:

Add nuget package to project:

```
Amazon.Lambda.AspNetCoreServer.Hosting
```

In Program.cs:

```
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
```

2. Add json file with defaults:

```
aws-lambda-tools-defaults.json
```

3. Add aws lambda template file:

```
serverless.template
```

4. Set RootNamespace and AssemblyName in project file:

The AssemblyName MUST be "bootstrap".

```
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <RootNamespace>Dnw.Website</RootNamespace>
    <AssemblyName>bootstrap</AssemblyName>
  </PropertyGroup>
</Project>
```

# Aws lambda arm64

At the moment running on arm64 aws graviton is about 20% cheaper.

To use arm64 instead of x86_64:

In aws-lambda-tools-defaults.json change these setings:

```
"msbuild-parameters": "--self-contained true --runtime linux-arm64",
```

And in the serverless.template file change these settings:

```
"Architectures": [ "arm64" ]
```

Also in the project file add the following conditional ItemGroup:

```
<ItemGroup Condition="'$(RuntimeIdentifier)' == 'linux-arm64'">
  <RuntimeHostConfigurationOption Include="System.Globalization.AppLocalIcu" Value="68.2.0.9" />
  <PackageReference Include="Microsoft.ICU.ICU4C.Runtime" Version="68.2.0.9" />
</ItemGroup>
```

Without this you will see errors like this in the CloudWatch logs: 'Cannot get symbol ucol_setMaxVariable_50 from libicui18n'.

More info on this issue can be found here: https://github.com/normj/LambdaNETCoreSamples/tree/master/ArmLambdaFunction

# AWS Lambda Custom Domains

When you deploy to aws lambda you will get a public url like this:

```
https://8tvctbmdz9.execute-api.ap-southeast-1.amazonaws.com/
```

Obviously not suitable for customer facing websites / apis. 

What we can do in configure AWS CloudFront in front of our website / api. In addition this allows us to use a custom domain. 

1. Create certificate

In the AWS console go to 'AWS Certificate Manager' and click on 'Request Certificate', choose 'Public' and click 'Next'.

In the 'Fully qualified domain name' field enter the common name and alternative names of the certificate. In this case I want to request a wildcard certificate:

```
*.dotnet-works.com
dotnet-works.com
```

For the validation method choose 'DNS' and click on 'Request'. 

The certificate request will now appear under 'List certificates' and its status will be something like 'Validation Pending'. 

We now have to go to our DNS provider (in my case CloudFlare) and add CNAME records with the values shown in the certificate details. In this case:

| Type  | Name                                                 | Value                                                              |
|-------|------------------------------------------------------|--------------------------------------------------------------------|
| CNAME | _bcb3d837c25f23a8d3dd658d7349764b.dotnet-works.com.  | _d0e9e8424667fa333921826ab771e188.rvctyfnwhz.acm-validations.aws.  |

After creating the CNAME record(s) the status of the certificate in the AWS console should change to 'Issued' after a while.

2. Create CloudFront Distribution

In the AWS console go to 'CloudFront' and click on 'Create distribution'.

For 'origin' enter the public url of the aws lambda function you want to expose without https://. So in this case I entered:

```
8tvctbmdz9.execute-api.ap-southeast-1.amazonaws.com
```

Under 'Custom SSL certificate' choose the certificate that was just issued.

Then under 'Alternate domain name (CNAME) - optional' enter the domain names for which you will create CNAME records with your DNS Provider later (CloudFlare in my case).

Then click 'Create distribution'.

3. Add CNAME record(s) with your DNS provider

Using the control panel of your DNS provider (CloudFlare in my case) create CNAME records pointing to your AWS CloudFront distribution.

The name of the CNAME record is your custom domain (test2.dotnet-works.com in my case) and the value is the 'Distribution domain name' on the general tab of the AWS CloudFront distribution.

In my case it looks like this:

| Type  | Name  | Value                                 |
|-------|-------|---------------------------------------|
| CNAME | test2 | https://d1w77qtjdsniry.cloudfront.net |

# CloudFront Cache Invalidation

When you release a new version, users will still see the old (cached) version for some time. That is after all the whole goal of caching :)

But in this case you usually want users to see the latest and greatest version immediately. You can invalidate the cache in your CloudFront distribution in that case. In the CloudFront UI its on the Invalidations tab, but a better way would be to do it using the AWS CLI after a new deployment. 

# Issues

I had issues with loading the fonts with font-awesome when using the vendor package, so I changed from:

```
~/vendor/fontawesome-free/css/all.min.css
```

To the CloudFlare CDN:

```
//cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css
```

# Azure App Service Deployment

to deploy successfully using github actions to the azure app service, you need to set the
SCM_DO_BUILD_DURING_DEPLOYMENT setting to false for the app service  