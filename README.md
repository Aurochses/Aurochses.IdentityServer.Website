# Aurochses.IdentityServer.Website

Aurochses.IdentityServer.Website is IdentityServer website.
created based on code: https://github.com/IdentityServer/IdentityServer4.Templates/commit/830000c05360b82421764b4b5962cb003b18d977

## Web

Type        | Web | Api 
------------|-----|-----
Test        | [web](http://identityserver.test.csharp.aurochses.demo.by) | [api](http://api.identityserver.test.csharp.aurochses.demo.by/swagger)
Staging     | [web](http://identityserver.staging.csharp.aurochses.demo.by) | [api](http://api.identityserver.staging.csharp.aurochses.demo.by/swagger)
Production  | [web](http://identityserver.production.csharp.aurochses.demo.by) | [api](http://api.identityserver.production.csharp.aurochses.demo.by/swagger)

## Users

Login | Password | Role | Permissions
------|----------|------|-------------
test@aurochses.com | Aurochses1234! | - | -
user@aurochses.com | Aurochses1234! | user | user
admin@aurochses.com | Aurochses1234! | admin | user, admin

## Clients

Name | Web | Source Code
-----|-----|-------------
.NET Core Console | - | [source code](https://github.com/Aurochses/Aurochses.IdentityServer.WebSite/tree/master/clients/Aurochses.IdentityServer.Client.Console)
Aurochses.Angular.Auth | [web](http://auth.production.angular.aurochses.demo.by) | [source code](https://github.com/Aurochses/Aurochses.Angular.Auth)
Aurochses.Angular.Template | [web](http://template.production.angular.aurochses.demo.by) | [source code](https://github.com/Aurochses/Aurochses.Angular.Template)

**test credentials:**  / 

## Builds

Type            | Status 
----------------|--------
CI (develop)    | [![Visual Studio Team services](https://img.shields.io/vso/build/aurochses/784be346-9d3f-458f-95d8-5f1a8b5e1227/121.svg?style=flat-square)](https://aurochses.visualstudio.com/Aurochses.CSharp/_build/index?definitionId=121)
CI (pre-master) | [![Visual Studio Team services](https://img.shields.io/vso/build/aurochses/784be346-9d3f-458f-95d8-5f1a8b5e1227/325.svg?style=flat-square)](https://aurochses.visualstudio.com/Aurochses.CSharp/_build/index?definitionId=325)
PR (latest)     | [![Visual Studio Team services](https://img.shields.io/vso/build/aurochses/784be346-9d3f-458f-95d8-5f1a8b5e1227/326.svg?style=flat-square)](https://aurochses.visualstudio.com/Aurochses.CSharp/_build/index?definitionId=326)
CI (master)     | [![Visual Studio Team services](https://img.shields.io/vso/build/aurochses/784be346-9d3f-458f-95d8-5f1a8b5e1227/327.svg?style=flat-square)](https://aurochses.visualstudio.com/Aurochses.CSharp/_build/index?definitionId=327)