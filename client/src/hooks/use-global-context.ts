import { useContext } from "react";
import GlobalContext from '../context/global-context'

const useGlobalContext = () => {
	return useContext(GlobalContext);
};

export default useGlobalContext;