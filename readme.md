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

## In depth explanation

### Product component

The Product component library contains the "App.razor" component.
This component is responsible for rendering the product, product picker and
the other components (Related and Order).

This component also contains the logic to communicate with the other components.
It communicates the ProductId with the Related and Order components and it also
communicates the selected product with the Order and Related components.

### Related component

The Related component library contains the Recos component.
This component is responsible for rendering the related products.
This component expects a TractorId as parameter.
The TractorId is used to fetch the related products from the server
(in this demo it's hard coded, but in practice you'll want to fetch the data from an actual api).

### Order component

The Order component library contains the `Buy` and `Basket` component.
The `Buy` component is responsible for rendering the buy button.
This component expects a TractorId as parameter.
This TractorId is used to fetch the price of the product from the server (again, hardcoded in this demo).

Both the `Buy` and the `Basket` components have a BasketService service which is injected via Dependancy Injection.
The `Buy` service will add the product.
The `Basket` service will listen to the `OnChange` event and update the basket accordingly.

This comminucation is done via a service because the `Buy` and `Basket` components are not in the same component tree.
Attributes won't work in this case, because we do not want to delegate state management to the `Product` component.

### Server

The `Server` project is just a shell application which renders the `App` component which
can be found in the `Product` component library.
