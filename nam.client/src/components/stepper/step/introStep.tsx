import {
  Box,
  Button,
  Typography,
  Stack,
  Container,
  useTheme,
  alpha,
} from "@mui/material";
import * as Icons from "@mui/icons-material";
import MyButton from "../../button";

export interface InfoItem {
  iconName: keyof typeof Icons;
  text: string;
}

export interface SurveyIntroProps {
  title: string;
  headerTitle: string;
  description: string;
  imageSrc?: string;
  infoItems: InfoItem[];
  buttonLabel: string;
  buttonSkipLabel?: string;
  onStart: () => void;
  onSkip?: () => void;
}

export function SurveyIntro({
  title,
  headerTitle,
  buttonSkipLabel = "Skip",
  description,
  imageSrc,
  infoItems,
  buttonLabel,
  onStart,
  onSkip,
}: SurveyIntroProps) {
  const theme = useTheme();

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        pt: "env(safe-area-inset-top)",
        pb: "env(safe-area-inset-bottom)",
      }}
    >
      <Container
        maxWidth="sm"
        sx={{
          display: "flex",
          flexDirection: "column",
          flexGrow: 1,
          px: { xs: 3, sm: 4 },
        }}
      >
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            position: "relative",
            py: { xs: 2, sm: 3 },
          }}
        >
          <Typography variant="subtitle1">{headerTitle}</Typography>
          {onSkip && (
            <Button
              onClick={onSkip}
              variant="outlined"
              size="small"
              sx={{
                position: "absolute",
                right: 0,
                color: "text.secondary",
                textTransform: "none",
                borderRadius: 2,

                borderColor: "text.secondary",
              }}
            >
              {buttonSkipLabel}
            </Button>
          )}
        </Box>
        <Box
          sx={{
            width: "100%",
            aspectRatio: { xs: "4/3", sm: "16/9" },
            borderRadius: 2,
            overflow: "hidden",
            mt: 2,
            mb: { xs: 4, sm: 5 },
            bgcolor: "action.hover",
            ...(!imageSrc && {
              backgroundImage: `radial-gradient(${theme.palette.divider} 15%, transparent 15%)`,
              backgroundSize: "24px 24px",
            }),
          }}
        >
          {imageSrc && (
            <img
              src={imageSrc}
              alt="Survey Intro"
              style={{ width: "100%", height: "100%", objectFit: "cover" }}
            />
          )}
        </Box>

        <Box sx={{ mb: 4 }}>
          <Typography
            variant="h4"
            component="h1"
            sx={{
              fontWeight: 800,
              mb: 1.5,
              fontSize: { xs: "1.75rem", sm: "2.25rem" },
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
              lineHeight: 1.6,
              fontSize: { xs: "1rem", sm: "1.1rem" },
            }}
          >
            {description}
          </Typography>
        </Box>

        {/* Benefits */}
        <Stack spacing={2.5} sx={{ mb: 6 }}>
          {infoItems.map((item, index) => {
            const IconComponent = Icons[
              item.iconName
            ] as Icons.SvgIconComponent;
            return (
              <Stack
                key={index}
                direction="row"
                spacing={2}
                alignItems="center"
              >
                <Box
                  sx={{
                    display: "flex",
                    p: 1,
                    borderRadius: 2,
                    bgcolor: alpha(theme.palette.primary.main, 0.08),
                    color: "primary.main",
                  }}
                >
                  {IconComponent && <IconComponent fontSize="small" />}
                </Box>

                <Typography
                  variant="body2"
                  sx={{
                    fontWeight: 600,
                    fontSize: "0.95rem",
                    color: "text.primary",
                  }}
                >
                  {item.text}
                </Typography>
              </Stack>
            );
          })}
        </Stack>
        {/* CTA */}
        <MyButton label={buttonLabel} action={onStart} />
      </Container>
    </Box>
  );
}
