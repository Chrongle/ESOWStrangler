DROP DATABASE IF EXISTS CatalogDb;

GO

CREATE DATABASE CatalogDb;

GO

USE CatalogDb;

DECLARE @json NVARCHAR(MAX) = N'{
  "catalogTypes_data" : [
    {
      "Id": 1,
      "Type": "Mug"
    },
    {
      "Id": 2,
      "Type": "T-Shirt"
    },
    {
      "Id": 3,
      "Type": "Sheet"
    },
    {
      "Id": 4,
      "Type": "USB Memory Stick"
    }
  ],

  "catalogBrands_data" : [
    {
      "Id": 1,
      "Brand": "Azure"
    },
    {
      "Id": 2,
      "Brand": ".NET"
    },
    {
      "Id": 3,
      "Brand": "Visual Studio"
    },
    {
      "Id": 4,
      "Brand": "SQL Server"
    },
    {
      "Id": 5,
      "Brand": "Other"
    }
  ],

  "catalog_data" : [
    {
      "Id": 1,
      "Name": ".NET Bot Black Sweatshirt",
      "Description": ".NET Bot Black Sweatshirt",
      "Price": 19.50,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/1.png",
      "CatalogTypeId": 2,
      "CatalogBrandId": 2
    },
    {
      "Id": 2,
      "Name": ".NET Black & White Mug",
      "Description": ".NET Black & White Mug",
      "Price": 8.50,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/2.png",
      "CatalogTypeId": 1,
      "CatalogBrandId": 2
    },
    {
      "Id": 3,
      "Name": "Prism White T-Shirt",
      "Description": "Prism White T-Shirt",
      "Price": 12.00,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/3.png",
      "CatalogTypeId": 2,
      "CatalogBrandId": 5
    },
    {
      "Id": 4,
      "Name": ".NET Foundation Sweatshirt",
      "Description": ".NET Foundation Sweatshirt",
      "Price": 12.00,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/4.png",
      "CatalogTypeId": 2,
      "CatalogBrandId": 2
    },
    {
      "Id": 5,
      "Name": "Roslyn Red Sheet",
      "Description": "Roslyn Red Sheet",
      "Price": 8.50,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/5.png",
      "CatalogTypeId": 3,
      "CatalogBrandId": 5
    },
    {
      "Id": 6,
      "Name": ".NET Blue Sweatshirt",
      "Description": ".NET Blue Sweatshirt",
      "Price": 12.00,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/6.png",
      "CatalogTypeId": 2,
      "CatalogBrandId": 2
    },
    {
      "Id": 7,
      "Name": "Roslyn Red T-Shirt",
      "Description": "Roslyn Red T-Shirt",
      "Price": 12.00,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/7.png",
      "CatalogTypeId": 2,
      "CatalogBrandId": 5
    },
    {
      "Id": 8,
      "Name": "Kudu Purple Sweatshirt",
      "Description": "Kudu Purple Sweatshirt",
      "Price": 8.50,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/8.png",
      "CatalogTypeId": 2,
      "CatalogBrandId": 5
    },
    {
      "Id": 9,
      "Name": "Cup<T> White Mug",
      "Description": "Cup<T> White Mug",
      "Price": 12.00,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/9.png",
      "CatalogTypeId": 1,
      "CatalogBrandId": 5
    },
    {
      "Id": 10,
      "Name": ".NET Foundation Sheet",
      "Description": ".NET Foundation Sheet",
      "Price": 12.00,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/10.png",
      "CatalogTypeId": 3,
      "CatalogBrandId": 2
    },
    {
      "Id": 11,
      "Name": "Cup<T> Sheet",
      "Description": "Cup<T> Sheet",
      "Price": 8.50,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/11.png",
      "CatalogTypeId": 3,
      "CatalogBrandId": 2
    },
    {
      "Id": 12,
      "Name": "Prism White TShirt",
      "Description": "Prism White TShirt",
      "Price": 12.00,
      "PictureUri": "http://catalogbaseurltobereplaced/images/products/12.png",
      "CatalogTypeId": 2,
      "CatalogBrandId": 5
    }
  ],
}';

SELECT * INTO CatalogTypes
FROM OPENJSON(@json, '$.catalogTypes_data')
WITH (
  Id INT '$.Id',  
  TypeName NVARCHAR(MAX) '$.Type'   
);

SELECT * INTO CatalogBrands
FROM OPENJSON(@json, '$.catalogBrands_data')
WITH (
  Id INT '$.Id',  
  BrandName NVARCHAR(MAX) '$.Brand'
);

SELECT * INTO CatalogItems
FROM OPENJSON(@json, '$.catalog_data')
WITH (
  Id INT '$.Id',  
  Name NVARCHAR(MAX) '$.Name',
  Description NVARCHAR(MAX) '$.Description',
  Price DECIMAL (18,2) '$.Price', 
  PictureUri NVARCHAR(MAX) '$.PictureUri',
  CatalogTypeId INT '$.CatalogTypeId', 
  CatalogBrandId INT '$.CatalogBrandId'
);