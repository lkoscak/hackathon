import React from "react";
import { IGlobalContextType } from "../context/types"

const GlobalContext = React.createContext<IGlobalContextType>({
       teamCardStatsWindowState: {
              teamCardStatsWindow1: { components: [] },
              teamCardStatsWindow2: { components: [] }
       },
       reports: [],
       reportCount:0,
       dispatch: () => { }
});

export default GlobalContext;