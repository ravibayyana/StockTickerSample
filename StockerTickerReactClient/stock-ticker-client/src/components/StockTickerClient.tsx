import React, { useEffect, useMemo, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { AgGridReact } from "ag-grid-react";
import { ColDef, GridOptions, GridReadyEvent } from "ag-grid-community";
import "ag-grid-community/styles/ag-grid.css";
import "ag-grid-community/styles/ag-theme-alpine.css";
import { Stock } from "../Models/stock";
import PriceRenderer from "../renderers/PriceRenderer";

const HUB_URL = "https://localhost:60101/stockTickerHub";

const columnDefs: ColDef[] = [
  { headerName: "Symbol", field: "name", width: 100 },
  {
    headerName: "Price",
    field: "price",
    width: 200,
    cellRenderer: PriceRenderer,
  },
  {
    headerName: "Updated At",
    field: "updatedAt",
    width: 200,
    valueFormatter: (params) => {
      const date = new Date(params.data.updatedAt);
      return date.toLocaleString();
    },
  },
];

const gridOptions: GridOptions = { context: {} };

interface IData {
  previous: Map<string, Stock>;
  current: Stock[];
}

export const StockTickerClient = () => {
  const [gridData, setGridData] = useState<IData>({
    previous: new Map<string, Stock>(),
    current: [],
  });
  const [isConnected, setIsConnected] = useState(false);
  const [hubConnection, setHubConnection] = useState<signalR.HubConnection>(
    (null as unknown) as signalR.HubConnection
  );

  const gridStyle = useMemo(() => ({ height: "500px", width: "505px" }), []);

  useEffect(() => {
    if (!hubConnection) return;

    hubConnection
      .start()
      .then(() => {
        hubConnection.on("UpdateStockPrices", (stocks: Stock[]) => {
          // console.table(stocks);
          setGridData((previousData) => {
            const map = new Map<string, Stock>();
            previousData.current.forEach((x) => map.set(x.id, x));
            return {
              ...previousData,
              previous: map,
              current: stocks,
            };
          });
        });

        if (hubConnection.state === signalR.HubConnectionState.Connected) {
          setIsConnected(true);
        }
      })
      .catch((err) => {
        console.error("Failed to CONNECT to server");
        console.log(err);
        setIsConnected(false);
      });
  }, [hubConnection]);

  useEffect(() => {
    try {
      const connection = new signalR.HubConnectionBuilder()
        .withUrl(HUB_URL, {
          skipNegotiation: true,
          transport: signalR.HttpTransportType.WebSockets,
        })
        .withAutomaticReconnect()
        .build();

      setHubConnection(connection);
    } catch (e) {
      console.error("Failed to Create Hub Connection to Url: " + HUB_URL);
      console.dir(e);
      setIsConnected(false);
    }
  }, []);

  useEffect(() => {
    if (gridOptions) {
      gridOptions.context = { previous: gridData.previous };
    }
  }, [gridData]);

  const onGridReady = (params: GridReadyEvent<any>) => {
    params.api.sizeColumnsToFit();
  };

  return (
    <div>
      <div
        style={{
          display: "flex",
          justifyContent: "center",
          alignContent: "center",
        }}
      >
        <h1>Stock Price Ticker </h1>
      </div>

      <div
        style={{
          display: "grid",
          gridRow: "1fr",
          gridColumn: "1fr",
          placeContent: "center",
        }}
      >
        <div style={gridStyle} className="ag-theme-alpine">
          <AgGridReact
            columnDefs={columnDefs}
            gridOptions={gridOptions}
            rowData={gridData.current}
            onGridReady={onGridReady}
          ></AgGridReact>
        </div>
      </div>
    </div>
  );
};
