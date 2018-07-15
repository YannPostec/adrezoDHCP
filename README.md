# AdrezoDHCP : Windows DHCP Server API Service

Designed for use with Adrezo web application.  
Installed on Windows Server, it provide api endpoints used by Adrezo to inject DHCP IP Addresses into the database.

## Environment

The application is under development and tested only on :
- Windows Server 2016

## Installation

Use the MSI provided to install the service.
The service AdrezoDHCP will be in automatic mode and launched after install.

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
  </appSettings>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.6.1" />
  </startup>
</configuration>
````

The parameters are :
- web_host : String. Listening service hostname or IP address
- web_port : Integer. Listening port, default to 6660
- web_auth : Boolean. Listening server requires Basic Authentication
- web_user : String. Username for basic authentication
- web_pwd : String. Password for basic authentication
- web_ssl : Boolean. Listening server on HTTP (false) or HTTPS (true)

### SSL
No certificate is provided with the application.  
You must generate and install a certificate in computer store.  

Retrieve the certificate hash in Windows MMC Certificates, thumbprint property of your certificate.  

And bind it to the listening port of the application :  
`netsh http add sslcert ipport=*:<application port> certhash=<thumbprint> appid={<application guid>}`

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
_MAC Value are represented by 12 hexadecimal characters without separation, example : 1a2b3c4d5e6f_  
_Date value are in format "yyyy-MM-dd HH:mm:ss", example 2018-07-15 09:17:32_  
_Name represents only the host part of hostname and are truncated to 20 characters_  

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
