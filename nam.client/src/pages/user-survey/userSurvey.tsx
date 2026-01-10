import React from "react";
import MyAppBar from "../../components/appbar";
import { useTheme, Box, Card, Container } from "@mui/material";
import { useUserSurvey } from "./hooks/useUserSurvey";
import {
  surveyCompleteConfig,
  surveyIntroConfig,
  surveySteps,
} from "./surveyStepsConfig";

import { SurveyIntro } from "../../components/stepper/step/introStep";
import { SurveyCompletion } from "../../components/stepper/step/completeStep";
import { MyStepper } from "../../components/stepper/myStepper";
import { useNavigate } from "react-router-dom";
import ImageSurveyIntro from "../../assets/survey.png";
const UserSurvey: React.FC = () => {
  const state = useUserSurvey();
  const steps = surveySteps(state);
  const navigate = useNavigate();
  const theme = useTheme();

  return (
    <Box
      sx={{
        backgroundColor: theme.palette.background.default,
        minHeight: "100dvh",
      }}
    >
      <MyAppBar title={"User survey"} back />
      <Container maxWidth="sm">
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            paddingY: 4,
          }}
        >
          <Card
            sx={{
              width: "90%",
              padding: 3,
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
            }}
          >
            {state.initSurvey ? (
              <SurveyIntro
                title={surveyIntroConfig.title}
                description={surveyIntroConfig.description}
                buttonLabel={surveyIntroConfig.buttonLabel}
                onStart={state.start}
                //onSkip={state.skip}
                headerTitle={""}
                imageSrc={ImageSurveyIntro}
                infoItems={[
                  {
                    iconName: surveyIntroConfig.infoItems[0]
                      .iconName as keyof typeof import("@mui/icons-material"),
                    text: surveyIntroConfig.infoItems[0].text,
                  },
                  {
                    iconName: surveyIntroConfig.infoItems[1]
                      .iconName as keyof typeof import("@mui/icons-material"),
                    text: surveyIntroConfig.infoItems[1].text,
                  },
                ]}
              />
            ) : state.activeStep < steps.length ? (
              <MyStepper
                state={state}
                steps={steps}
                onNext={state.next}
                onBack={state.back}
                onsubmit={state.submit}
              />
            ) : (
              <SurveyCompletion
                title={surveyCompleteConfig.title}
                subtitle={surveyCompleteConfig.subtitle}
                buttonLabel={surveyCompleteConfig.buttonLabel}
                onFinish={() => {
                  navigate("/maincontents");
                }}
              />
            )}
          </Card>
        </Box>
      </Container>
    </Box>
  );
};

export default UserSurvey;
