ODSForm
============

ODS Form is a DotNetNuke module that allows you to publish simple feedback forms with a variable list of custom fields.

## Links
[ODSForm GitHub repository (this page)](https://github.com/trapias/ODSForm/)

[ODSForm homepage](http://trapias.github.io/dnn/odsform/)

[ODSForm Trello support board](https://trello.com/b/vZuoVDnV/ods-form)

## Version: 01.00.11
**Cascading SQL dropdowns**: it is now possible to link two dropdowns, that is to populate a second dropdown when the user selects a value in a first dropdown.

For example, here are a "country" dropdown linked to a "region" one: when you select a country, the second dropdown is populated with regions from the selected country.

![Cascading dropdown demo](http://trapias.github.io/images/ods_cascading_demo.gif)

In order to obtain this, you have to leave the second dropdown empty, and configure the first like in the following figure:

![How to link dropdowns](http://trapias.github.io/images/odsform_sql_cascading_dropdowns.png)

## Version: 01.00.10
New capability to **populate DropDownList** form item types **with an SQL query**.

To use just flag the new checkbox "SQL Query" and write a simple SQL query as the form item value.

The query runs over DNN database, and should return only **two columns**: the first used as **value** and the second as the **text** of the dropdown options.

For example a valid query would be:

```
SELECT userid,displayname FROM users
```


## Version: 01.00.09

### New Request.Servervariables custom token
Adding a new custom token named SERVERVAR, will allow to get value (e.g. in a hidden field on form) from Request variables.

For example: [SERVERVAR:REMOTE_ADDR] will get as value the IP address of the remote party.

### SubmissionManager: see submissions as full table
SubmissionManager now parses submissions XML data to view submissions as a full table, with each field as a column.
This allows for nicer (and more useful) CSV export.
Updated to use latest version of DataTables (1.10.6).

### Import & Export module
Implemented the IPortable DNN interface, that allows to export and import module content (form fields).

### MSCaptcha
In order to use MSCaptcha you have to add an handler to web.config:

```
	<add name="MSCaptcha.captchaImageHandler" verb="GET" path="CaptchaImage.axd" type="MSCaptcha.captchaImageHandler, MSCaptcha" resourceType="Unspecified" />
```

(to be added to installation script)

## Newtonsoft.Json.dll
Note that since version 01.00.07 ODSForm does not include Newtonsoft library anymore, so if you are to install on DNN 6 you have to get it separately (download from [http://json.codeplex.com](http://json.codeplex.com/) ).

You don't need it if you're installing on DNN 7, that already ships the library in standard distribution.

