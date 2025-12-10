import { Link, Stack, Typography } from "@mui/material";

interface DetailRowProps {
  label: string;
  value: string | undefined | null;
  isLink?: boolean;
  IconComponent?: React.ElementType;
}

export const DetailRow: React.FC<DetailRowProps> = ({
  label,
  value,
  isLink = false,
  IconComponent,
}) => {
  if (!value) {
    return null;
  }

  return (
    <Stack direction="row" alignItems="center" spacing={1} mb={1}>
      {/* 1. Optional Icon */}
      {IconComponent && <IconComponent color="action" fontSize="small" />}
      {/* Field label */}
      <Typography sx={{ fontWeight: "bold" }}>{label}</Typography>

      {/* Value */}
      {isLink ? (
        <Link href={value} target="_blank" rel="noopener">
          {value}
        </Link>
      ) : (
        <Typography>{value}</Typography>
      )}
    </Stack>
  );
};
