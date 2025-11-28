import React from "react";
import MyStepper from "./components/MyStepper";
import MyAppBar from "../../components/appbar";

const ResetPasswordPage: React.FC = () => {
  return (
    <div>
      <MyAppBar title={"Reset Password"} backUrl={"/"} />
      <MyStepper />
    </div>
  );
};

export default ResetPasswordPage;
