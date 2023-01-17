
# Stock Price Ticker

Server publishes stock price ticks at 1 sec interval

Clients subscribed will display stock prices in a grid and shows an 
indicator wheter the price went up or down  

## Server (C#, .Net Core 6, SignalR)

Starts publishing ticks when first client connects and never ends ticking

## Client (React, Aggrid, SignalR, Typescript)

Subscribes to SignalR URL and displays data in grid format along with update at info.
An arrow is displayed based on the value is up or down when compared to previous value.

# How to run

- Clone git project

Server

- Open "StockTickerSample\StockTicker\StockTicker.sln" in Visual Studio 2022+
- Build the project
- Run (Ctrl + F5)

Client
- open "StockTickerSample\StockerTickerReactClient\stock-ticker-client" in VS code
- open terminal and run the following command 
- npm start

# Improvements 

- Previous values can be published from the server side rather than storing on the client
- Handle reconnect, onconnect, ondisconnect logic on the client elegantly
- Write react client side unit tests
- Need to add more logging on the server
- Few units tests are written for server need to write more

# ScreenShots 

# Real time gif


 

