import {
  Box,
  Typography,
  Container,
  Stack,
  CircularProgress,
  useTheme,
} from "@mui/material";
import * as Icons from "@mui/icons-material";
import MyButton from "../../button";

export interface SurveyCompletionProps {
  title?: string;
  subtitle?: string;
  buttonLabel?: string;
  loadingMessage?: string;
  onFinish: () => void;
  isLoading?: boolean;
}

export function SurveyCompletion({
  title = "",
  subtitle = "",
  buttonLabel = "",
  loadingMessage = "Analyzing your preferences...",
  onFinish,
  isLoading = false,
}: SurveyCompletionProps) {
  const theme = useTheme();

  return (
    <Box
      sx={{
        height: { xs: "100dvh", sm: "100vh" },
        width: "100%",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        bgcolor: "background.default",
        overflow: "hidden",
        position: "fixed",
        top: 0,
        left: 0,
        zIndex: theme.zIndex.modal,
      }}
    >
      <Container
        maxWidth="sm"
        sx={{
          textAlign: "center",
          px: 3,
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          justifyContent: "center",
          height: "100%",
        }}
      >
        {isLoading ? (
          <Stack alignItems="center" spacing={3}>
            <CircularProgress size={60} thickness={4} color="primary" />
            <Typography
              variant="h6"
              fontWeight={600}
              sx={{ color: "text.primary" }}
            >
              {loadingMessage}
            </Typography>
          </Stack>
        ) : (
          <Box
            sx={{
              animation: "fadeInUp 0.8s ease-out",
              "@keyframes fadeInUp": {
                from: { opacity: 0, transform: "translateY(20px)" },
                to: { opacity: 1, transform: "translateY(0)" },
              },
            }}
          >
            <Box
              sx={{
                position: "relative",
                display: "inline-flex",
                mb: 4,
              }}
            >
              <Box
                sx={{
                  width: 100,
                  height: 100,
                  borderRadius: "50%",
                  bgcolor: "success.main",
                  opacity: 0.12,
                  position: "absolute",
                  left: "50%",
                  top: "50%",
                  transform: "translate(-50%, -50%)",
                }}
              />
              <Icons.CheckCircle
                sx={{
                  fontSize: 80,
                  color: "success.main",
                }}
              />
            </Box>

            <Typography
              variant="h3"
              sx={{
                fontWeight: 800,
                mb: 2,
                fontSize: { xs: "2rem", sm: "2.5rem" },
                lineHeight: 1.1,
                color: "text.primary",
              }}
            >
              {title}
            </Typography>

            <Typography
              variant="body1"
              sx={{
                color: "text.secondary",
                mb: 6,
                fontSize: { xs: "1.1rem", sm: "1.2rem" },
                maxWidth: "450px",
                mx: "auto",
              }}
            >
              {subtitle}
            </Typography>

            <MyButton label={buttonLabel} action={onFinish} />
          </Box>
        )}
      </Container>
    </Box>
  );
}
