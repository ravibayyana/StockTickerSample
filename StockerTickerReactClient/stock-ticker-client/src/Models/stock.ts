import React, { useEffect, useState } from "react";

export interface Stock {
  id: string;
  name: string;
  price: number;
  updatedAt: Date;
}
