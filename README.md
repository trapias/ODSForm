ODSForm
============

ODS Form is a DotNetNuke module that allows you to publish simple feedback forms with a variable list of custom fields.

## Links
[ODSForm GitHub repository (this page)](https://github.com/trapias/ODSForm/)

[ODSForm homepage](http://trapias.github.io/dnn/odsform/)

[ODSForm Trello support board](https://trello.com/b/vZuoVDnV/ods-form)

## Latest version: 01.00.09

### New Request.Servervariables custom token
Adding a new custom token named SERVERVAR, will allow to get value (e.g. in a hidden field on form) from Request variables.

For example: [SERVERVAR:REMOTE_ADDR] will get as value the IP address of the remote party.

### SubmissionManager: see submissions as full table
SubmissionManager now parses submissions XML data to view submissions as a full table, with each field as a column.
This allows for nicer (and more useful) CSV export.
Updated to use latest version of DataTables (1.10.6).

### Import & Export module
Implemented the IPortable DNN interface, that allows to export and import module content (form fields).


## Newtonsoft.Json.dll
Note that since version 01.00.07 ODSForm does not include Newtonsoft library anymore, so if you are to install on DNN 6 you have to get it separately (download from [http://json.codeplex.com](http://json.codeplex.com/) ).

You don't need it if you're installing on DNN 7, that already ships the library in standard distribution.

