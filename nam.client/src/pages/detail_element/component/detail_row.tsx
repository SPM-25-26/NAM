import { Link, Stack, Typography } from "@mui/material";

interface DetailRowProps {
  label: string;
  value: string | undefined | null;
  isLink?: boolean;
  text?: string;
  IconComponent?: React.ElementType;
  isEmail?: boolean;
}

export const DetailRow: React.FC<DetailRowProps> = ({
  label,
  value,
  isLink = false,
  IconComponent,
  text,
  isEmail,
}) => {
  if (!value) {
    return null;
  }

  return (
    <Stack direction="row" alignItems="center" spacing={1} mb={1} ml={5}>
      {/* 1. Optional Icon */}
      {IconComponent && <IconComponent fontSize="small" />}
      {/* Field label */}
      <Typography sx={{ fontWeight: "bold" }}>{label}</Typography>

      {/* Value */}
      {isLink ? (
        <Link
          href={isEmail == true ? `mailto:${value}` : value}
          target="_blank"
          rel="noopener"
          style={{
            display: "block",
            whiteSpace: "normal",
            wordBreak: "break-word",
            overflowWrap: "anywhere",
          }}
        >
          {text ?? value}
        </Link>
      ) : (
        <Typography>{value}</Typography>
      )}
    </Stack>
  );
};
