import { Box, Typography, Button } from "@mui/material";
import WarningAmberIcon from "@mui/icons-material/WarningAmber";
import SentimentVeryDissatisfiedIcon from "@mui/icons-material/SentimentVeryDissatisfied";

const StatusCard = ({
  type,
  title,
  message,
  buttonText,
  onAction,
  details,
}: {
  type: "empty" | "error";
  title: string;
  message: string;
  buttonText: string;
  onAction: () => void;
  details?: string;
}) => {
  const isError = type === "error";

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        alignItems: "center",
        minHeight: "100vh",
        textAlign: "center",
        padding: 2,
      }}
    >
      <Box
        sx={{
          maxWidth: 500,
          width: "100%",
        }}
      >
        {/* ICON */}
        <Box
          sx={{
            color: isError ? "error.main" : "warning.main",
            fontSize: 80,
            marginBottom: 2,
          }}
        >
          {isError ? (
            <SentimentVeryDissatisfiedIcon fontSize="inherit" />
          ) : (
            <WarningAmberIcon fontSize="inherit" />
          )}
        </Box>

        {/* TITLE */}
        <Typography variant="h5" component="div" gutterBottom>
          {title}
        </Typography>

        {/* MESSAGE */}
        <Typography variant="body1" color="text.secondary" sx={{ mb: 3 }}>
          {message}
        </Typography>

        {/* BUTTON */}
        <Button
          variant="contained"
          color={isError ? "error" : "primary"}
          onClick={onAction}
          sx={{ padding: "10px 30px" }}
        >
          {buttonText}
        </Button>
        {/* DETAILS ERROR */}
        {isError && details && (
          <Box
            sx={{
              mt: 3,
              p: 2,
              bgcolor: "grey.100",
              borderRadius: 1,
              textAlign: "left",

              maxWidth: 450,
              margin: "24px auto 0 auto",
            }}
          >
            <Typography variant="body2" sx={{ fontWeight: "bold" }}>
              Details error:
            </Typography>
            <Typography variant="caption" sx={{ wordBreak: "break-all" }}>
              {details}
            </Typography>
          </Box>
        )}
      </Box>
    </Box>
  );
};

export default StatusCard;
