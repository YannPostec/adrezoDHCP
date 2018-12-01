# AdrezoDHCP : Windows DHCP Server API Service

Designed for use with Adrezo web application.  
Installed on Windows Server as a service, it provides api endpoints used by Adrezo to inject DHCP IP Addresses into the database.

## Environment

The application has been tested on :
- Windows Server 2008 R2
- Windows Server 2012 R2
- Windows Server 2016

The application needs .NET Framework 4.5 minimum.

## Installation

Use the MSI provided to install the service.  
The service AdrezoDHCP will be in automatic mode and launched after install.

## Upgrade

You must uninstall the previous version first to upgrade.  
The configuration file will be back to default.

## Configuration
In application directory, you will find an XML configuration file `AdrezoDHCP.exe.config`  

````
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <appSettings>
    <add key="web_host" value="localhost" />
    <add key="web_port" value="6660" />
    <add key="web_auth" value="false" />
    <add key="web_user" value="adrezo" />
    <add key="web_pwd" value="changeme" />
    <add key="web_ssl" value="false" />
    <add key="dhcp_host" value="localhost" />
    <add key="adrezo_format" value="true" />
  </appSettings>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
  </startup>
</configuration>
````

The parameters are :
- web_host : String. Listening service hostname or IP address, localhost is fine
- web_port : Integer. Listening port, default to 6660
- web_auth : Boolean. Listening server requires Basic Authentication
- web_user : String. Username for basic authentication
- web_pwd : String. Password for basic authentication
- web_ssl : Boolean. Listening server on HTTP (false) or HTTPS (true)
- dhcp_host : String. DHCP Server hostname, usually localhost
- adrezo_format : Boolean. Modify name and hardware adress to follow adrezo formats

### SSL
No certificate is provided with the application.  
You must generate and install a certificate in computer store.  

Retrieve the certificate thumbprint property in Windows MMC with Certificates Snap-in.

And bind it to the listening port of the application :  
`netsh http add sslcert ipport=*:<application port> certhash=<thumbprint> appid={<application guid>}`

application guid is an id like 00112233-4455-6677-8899-AABBCCDDEEFF

## Usage

List of endpoints :

- GET /dhcp/scopelist  
`{ "scopes" : [ { "ip" : "<scope ip>" } , ... ] }`
- GET /dhcp/range/&lt;scope&gt;  
`{ "start" : "<start ip>", "end" : "<end ip>" }`
- GET /dhcp/exclude/&lt;scope&gt;  
`{ "excluderanges" : [ { "start" : "<start ip>", "end" : "<end ip>" } , ... ] }`
- GET /dhcp/reserve/&lt;scope&gt;  
`{ "leases" : [ {"ip" : "<reservation ip>", "mac" : "<reservation mac>", "name" : "<reservation name>" } , ... ] }`
- GET /dhcp/lease/&lt;scope&gt;  
`{ "leases" : [ {"ip" : "<lease ip>", "mac" : "<lease mac>", "stamp" : "<Lease expiry date>", "name" : "<lease name>" } , ... ] }`

_IP values are represented as usual, example : 192.168.0.1_  
_Date value are in format "yyyy-MM-dd HH:mm:ss", example 2018-07-15 09:17:32_  
_MAC values are represented by 12 hexadecimal characters without separation, example : 1a2b3c4d5e6f_  
_(adrezo format parameter to true will remove : character and truncate hardware address to 12 characters)_  
_Name values, if empty on dhcp server, take value EMPTYNAME_  
_(adrezo format parameter to true will take only the host part of hostname and truncate it to 20 characters)_

## License

AdrezoDHCP is released under the [Apache 2.0 license](./LICENSE).

````
Copyright 2018 POSTEC Yann

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
````

## Libraries

- DHCP Server API 0.3 by garysharp under MIT license, [Github repository](https://github.com/garysharp/DhcpServerApi)
- Json.NET by Newtonsoft under MIT license, [Webpage](https://www.newtonsoft.com/json)
