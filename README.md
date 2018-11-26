# Broadridge_TasksApi project
Instructure describes API deployment via IIS.
Prerequisites: IIS, MS SQL Express Server.

## How to manually deploy the API

#### 1. Download the Publish_Binaries content to your machine.
https://github.com/Nikitas63/Broadridge_TasksApi/tree/master/Publish_Binaries
This is the latest release of API.

#### 2. Setting up the Application pool in IIS.
https://i.imgur.com/HmIGMOS.png
###### 2.1 Change the user of application pool (the user must can access to ms sql express server (Windows domain user - good choise)).
https://i.imgur.com/sNF3arC.png

#### 3. Add the new web site in IIS.
https://i.imgur.com/WpuFOsI.png
 - Physical path - path to downloaded content from p.1.
 - Application pool - pool that created in p.2.

Expected result: https://i.imgur.com/bmYbjuW.png

#### 4. Browse the http://localhost:5055/swagger/index.html
 - Front end application use the http://localhost:5055 url for backend.
 - First time you access the url, database (TasksDb) will be authomatically created in your SQLExpress server.

Expected result: https://i.imgur.com/gShovsX.png
