# RESTful-SQL-Service
This is a simple and generic REST service for SQL Server
It is built in windows forms using C# and .net 4.5.1

Run as administrator

Currently only GET is implemented.

It supports json and xml

usage:
http://*:80/SQLServer/[database]/[schema]/[table]/[KeyValue][KeyField1/KeyValue1[/KeyField2/KeyValue2...]]?option1&option2...

options:
	includecount	=fieldname
	page          =n
	perpage       =recordsperpage
	limit         =perpage
	start         =firstrecord
	finish        =lastrecord

	showquery     =true

	database	    =DBName
	schema	      =Schema
	table	        =TableName
	q	            =SQLQuery
	keyvalue	    =Id
	keyfield	    =12
	condition	    =sqlcondition
	fields	      =fieldlist
	orderby	      =fields
	xmlsuffix	    =FOR XML PATH('Record'), ELEMENTS XSINIL
	xmlroot	      =Data
	xmlrecord	    =Record

	format	      =xml|json
	mime	        =text/xml

	prepend	      =sometext
	append	      =sometext
	callback	    =jsoncallback
	



RESTful SQL Service
