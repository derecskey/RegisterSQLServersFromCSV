# RegisterSQLServersFromCSV
Registers SQL Server Databases within SSMS from a CSV file.  Creates a root ServerGroup, a ServerGroup per logical application database, and a ServerGroup per environment tier, before finally registering a server per application database. 

Configurable custom colors are used to visually designate query windows by environment tier (red for production, orange for uat, yellow for qa, green for dev), intended to convey the level of caution that should be exercised when running queries against each environment.

**Note: To-date, this has only been tested against SSMS 18.7.1**
