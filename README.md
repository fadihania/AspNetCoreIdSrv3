# AspNetCoreIdSrv3
An example on how to use IdentityServer3 with the new ASP.NET Core 1.0 RC1.

##Quick Start
1. Clone Git repo and `cd` into new directory
```bash
git clone https://github.com/fadihania/AspNetCoreIdSrv3.git
cd AspNetCoreIdSrv3
```
2. Install NuGet packages dependencies
```bash
dnu restore
```
3. Run the application
```bash
dnx web
```
4. Authenticate using POST request to get the JWT token:
```bash
http://localhost:5000/auth/connect/token
client_id: test
client_secret: secret
scope: openid email
grant_type: password
username: alice
password: alice
```
5. Try to access values API controller and you will get 401 Unauthorized
```bash
http://localhost:5000/api/values/
```
6. Try again with Authorization Bearer token you get from 4.
