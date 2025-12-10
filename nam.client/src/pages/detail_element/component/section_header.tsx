import { Stack, Typography } from "@mui/material";

const SectionHeader = ({
  title,
  IconComponent,
  color = "text.primary",
}: {
  title: string;
  IconComponent?: React.ElementType;
  color?: string;
}) => {
  const Icon = IconComponent;
  return (
    <Stack direction="row" alignItems="center" spacing={1} mb={1}>
      {Icon && <Icon sx={{ color: color }} />}
      <Typography
        variant="h6"
        component="h3"
        sx={{ fontWeight: "bold", color: "text.primary" }}
      >
        {title}
      </Typography>
    </Stack>
  );
};
export default SectionHeader;
