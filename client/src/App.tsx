import GlobalProvider from "./context/global-provider";
import  MainLayout  from "./components/MainLayout/mainLayout";

const App = () => {
  return (
    <GlobalProvider>
      <MainLayout/>
    </GlobalProvider>
  );
}

export default App;
