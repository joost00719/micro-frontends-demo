# Micro-frontends Blazor demo 

This demo is a proof of concept for a micro-frontends architecture using Blazor.
The original application is written in javascript and was the inspiration to create this demo.

## Architecture

The application is composed of 3 Blazor Component Libraries and a Blazor Server Hosted application.
The hierarchy is as follows:
* Product
	* Related
	* Order

The Product component is the main component and it is responsible for rendering the product and the other components.  
The Related component is responsible for rendering the related products.  
The Order component is responsible for rendering the buy button and the basket.  

![Architecture](/imgs/Screenshot_490.png)
* Red: Product component
* Green: Related component
* Blue: Order component

## Running the application
To start the project, run the following command:
```bash
dotnet run --project Server
```
After that, the project can be accessed at http://localhost:5068

