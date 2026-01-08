import {
  Box,
  Container,
  Typography,
  Paper,
  Avatar,
  Button,
  Stack,
  useTheme,
  Divider,
  alpha,
} from "@mui/material";
import PersonIcon from "@mui/icons-material/Person";
import LogoutIcon from "@mui/icons-material/Logout";
import LockResetIcon from "@mui/icons-material/LockReset";
import QuestionAnswerIcon from "@mui/icons-material/QuestionAnswer";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";
import MyAppBar from "../../components/appbar";
import { useNavigate } from "react-router-dom";
import { useProfile } from "./hooks/useProfile";
import { loadingView } from "../../components/loading";

const ProfilePage = () => {
  const navigate = useNavigate();
  const theme = useTheme();
  const userHooks = useProfile();

  const menuButtonStyle = {
    py: 2,
    px: 2.5,
    borderRadius: 4,
    fontWeight: 600,
    justifyContent: "space-between",
    textTransform: "none",
  };

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        minHeight: "100dvh",
        bgcolor: theme.palette.background.default,
      }}
    >
      <MyAppBar title={"Profilo"} />

      <Container
        maxWidth="sm"
        sx={{
          flex: 1,
          display: "flex",
          flexDirection: "column",
          pt: { xs: 2, sm: 6 },
          pb: 12,
          alignItems: "center",
        }}
      >
        {userHooks.state.loading && loadingView}
        {(userHooks.state.error ?? "").length > 0 && (
          <Box
            sx={{
              width: "100%",
              mb: 3,
              p: 2,
              borderRadius: 2,
              backgroundColor: theme.palette.error.light,
              color: theme.palette.error.main,
              textAlign: "center",
            }}
          >
            <Typography variant="body1" fontWeight={600}>
              {userHooks.state.error}
            </Typography>
          </Box>
        )}
        {!userHooks.state.loading &&
          (userHooks.state.error ?? "").length == 0 && (
            <Paper
              elevation={0}
              sx={{
                p: { xs: 3, sm: 4 },
                width: "90%",
                borderRadius: 2,
                border: `1px solid ${theme.palette.divider}`,
                background: theme.palette.background.paper,
              }}
            >
              {/* Header Section */}
              <Box sx={{ textAlign: "center", mb: 4 }}>
                <Avatar
                  sx={{
                    width: 90,
                    height: 90,
                    bgcolor: alpha("#AD99FF", 0.1),
                    color: "#AD99FF",
                    mx: "auto",
                    mb: 2,
                    fontSize: "2.5rem",
                    border: `2px solid ${alpha("#AD99FF", 0.2)}`,
                  }}
                >
                  <PersonIcon fontSize="inherit" />
                </Avatar>
                <Typography variant="h6" fontWeight="700">
                  Il mio Account
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  {userHooks.state.user?.email}
                </Typography>
              </Box>

              <Divider sx={{ mb: 3 }} />

              {/* Action List */}
              <Stack spacing={2}>
                <Button
                  variant="outlined"
                  disableElevation
                  endIcon={<ChevronRightIcon />}
                  startIcon={<QuestionAnswerIcon />}
                  fullWidth
                  sx={{
                    ...menuButtonStyle,
                    color: theme.palette.text.primary,
                    borderColor: theme.palette.divider,
                  }}
                  onClick={() => navigate("/survey")}
                >
                  <Box
                    component="span"
                    sx={{ flexGrow: 1, textAlign: "left", ml: 1 }}
                  >
                    User Questionnaire
                  </Box>
                </Button>

                <Button
                  variant="outlined"
                  endIcon={<ChevronRightIcon />}
                  startIcon={<LockResetIcon />}
                  fullWidth
                  sx={{
                    ...menuButtonStyle,
                    color: theme.palette.text.primary,
                    borderColor: theme.palette.divider,
                  }}
                  onClick={() => navigate("/resetPassword")}
                >
                  <Box
                    component="span"
                    sx={{ flexGrow: 1, textAlign: "left", ml: 1 }}
                  >
                    Reset Password
                  </Box>
                </Button>
                <Divider />
                <Button
                  variant="text"
                  color="error"
                  startIcon={<LogoutIcon />}
                  fullWidth
                  size="large"
                  sx={{ textTransform: "none" }}
                  onClick={userHooks.actions.logout}
                >
                  Esci (Logout)
                </Button>
              </Stack>
            </Paper>
          )}
      </Container>
    </Box>
  );
};

export default ProfilePage;
