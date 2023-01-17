import React from "react";
import { ICellRendererParams } from "ag-grid-community";

const PriceRenderer = (props: ICellRendererParams) => {
  const currentValue = Math.round(props.data.price * 100000);
  if (props.context.previous.size !== 0) {
    const symbol = props.context.previous.get(props.data.id);
    const previousValue = Math.round(symbol.price * 100000);

    if (currentValue > previousValue) {
      return (
        <span>
          <span style={{ fontSize: "35px", color: "green" }}>&#8593;</span>
          <span>{props.data.price}</span>
        </span>
      );
    } else {
      return (
        <span>
          <span style={{ fontSize: "35px", color: "red" }}>&#8595;</span>
          <span>{props.data.price}</span>
        </span>
      );
    }
  }

  return <span>{props.data.price}</span>;
};

export default PriceRenderer;
