import { BrowserRouter, Route, Routes } from "react-router-dom";
import HomePage from "./pages/home/homepage";
import NotFoundPage from "./pages/notFound/not-found";
import ResetPasswordPage from "./pages/reset-password/ResetPasswordPage";
import RegistrationPage from "./pages/registration/RegistrationPage";
import LoginPage from "./pages/login/LoginPage";
import MainContentsPage from "./pages/maincontents/MainContentsPage";
import VerifyEmailPage from "./pages/verifymail/VerifyEmailPage";
import { EventDetail } from "./pages/detail_element/detail_element";
import ChoicePageTest from "./pages/detail_element/choice";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/signup" element={<RegistrationPage />} />
        <Route path="/resetPassword" element={<ResetPasswordPage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/maincontents" element={<MainContentsPage />} />
        <Route path="/verify-email" element={<VerifyEmailPage />} />
        <Route path="*" element={<NotFoundPage />} />
        <Route path="/detail-element" element={<EventDetail />} />
        {/* TODO: remove route */}
        <Route path="/choice-test" element={<ChoicePageTest />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
