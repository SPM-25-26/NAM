import { BrowserRouter, Route, Routes } from "react-router-dom";
import HomePage from "./pages/home/homepage";
import NotFoundPage from "./pages/notFound/not-found";
import ResetPasswordPage from "./pages/reset-password/ResetPasswordPage";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/resetPassword" element={<ResetPasswordPage />} />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
