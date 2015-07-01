# RESTful-SQL-Service
This is a simple and generic REST service for SQL Server
It is built in windows forms using C# and .net 4.5.1

Here is an installer: 
https://app.box.com/s/smk4p9t71wnz3t0hcq0sbynj7y9uj9ii

Run as administrator

Currently only GET is implemented.

It supports json and xml. It uses Newtonsoft.json.

usage:
http://*:80/SQLServer/[database]/[schema]/[table][/[KeyValue|KeyField1/KeyValue1[/KeyField2/KeyValue2...]]]?option1&option2...

options:

	includecount	=fieldname
	
	page		=n
	perpage		=recordsperpage
	limit		=perpage
	start		=firstrecord
	finish		=lastrecord

	showquery	=true

	database	=DBName
	schema		=Schema
	table		=TableName
	q		=SQLQuery
	keyvalue	=Id
	keyfield	=12
	condition	=sqlcondition
	fields		=fieldlist
	orderby		=fields
	xmlsuffix	=FOR XML PATH('Record'), ELEMENTS XSINIL
	xmlroot		=Data
	xmlrecord	=Record

	format		=xml|json
	mime	        =text/xml

	prepend		=sometext
	append		=sometext
	callback	=jsoncallback
	

Examples:

	http://localhost/SQLServer/AdventureWorks/Sales/Customer?includecount=numRecords&format=xml
	http://localhost/SQLServer/AdventureWorks/Sales/Customer/213?showquery=false&format=xml
	http://localhost/SQLServer/AdventureWorks/Sales/Customer/CustomerType/S?includecount=nRecs&showquery=false&format=xml
	http://localhost/SQLServer/AdventureWorks/Sales/Customer/CustomerType/S/TerritoryID/3?includecount=count&format=xml
	http://localhost/SQLServer/AdventureWorks/Sales/Customer/CustomerType/S?condition=TerritoryID%3C=3&includecount=n
	http://localhost/SQLServer/AdventureWorks/Sales/Customer?fields=distinct%20CustomerType,%20n=count(*)%20over%20(partition%20by%20CustomerType)&includecount=count&format=xml
	
	http://localhost/SQLServer/AdventureWorks///StateProvinceCode/CA/City/Los%20Angeles?includecount=numRecords&showquery=false&format=xml&q=SELECT%20C.*,%20A.AddressLine1,%20A.AddressLine2,%20A.City,%20A.PostalCode,%20P.StateProvinceCode,%20P.CountryRegionCode%20FROM%20Sales.Customer%20C%20LEFT%20JOIN%20Sales.CustomerAddress%20K%20ON%20K.CustomerID%20=%20C.CustomerID%20LEFT%20JOIN%20Person.Address%20A%20ON%20A.AddressID%20=%20K.AddressID%20LEFT%20JOIN%20Person.StateProvince%20P%20ON%20P.StateProvinceID%20=%20A.StateProvinceID


RESTful SQL Service
