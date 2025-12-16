import { Stack, Typography, useTheme } from "@mui/material";

interface SectionHeaderProps {
  title: string;
  IconComponent?: React.ElementType;
  color?: string;
}

const SectionHeader: React.FC<SectionHeaderProps> = ({
  title,
  IconComponent,
}) => {
  const theme = useTheme();
  const Icon = IconComponent;

  return (
    <Stack direction="row" alignItems="center" spacing={1} mb={1}>
      {Icon && (
        <svg
          width={28}
          height={28}
          viewBox="0 0 24 24"
          xmlns="http://www.w3.org/2000/svg"
        >
          <defs>
            <linearGradient id="grad" x1="0%" y1="0%" x2="100%" y2="0%">
              <stop offset="0%" stopColor="#8aaefe" />
              <stop offset="100%" stopColor="#cc88fd" />
            </linearGradient>
          </defs>
          <Icon sx={{ fill: "url(#grad)" }} />
        </svg>
      )}
      <Typography
        variant="h6"
        component="h3"
        sx={{ fontWeight: "bold", color: theme.palette.text.primary }}
      >
        {title}
      </Typography>
    </Stack>
  );
};

export default SectionHeader;
